using CpuSchedulingSim.Hardware;
using CpuSchedulingSim.Metrics;
using CpuSchedulingSim.Models;
using CpuSchedulingSim.Scheduling;
using CpuSchedulingSim.Workload;

namespace CpuSchedulingSim.Simulation;

public sealed class SimulationEngine
{
    private readonly SimConfig _config;
    private readonly IScheduler _scheduler;
    private readonly WorkloadGenerator _generator;

    private readonly Random _rng;
    private readonly CacheModel _cache;
    private readonly MetricsCollector _metrics = new();

    private readonly EventQueue _eventQueue = new();

    private int _time;
    private int _pidCounter = 1;
    private int _generatedCount;

    private SimProcess? _running;
    private bool _cpuBusy;

    public SimulationEngine(SimConfig config, IScheduler scheduler, WorkloadGenerator generator)
    {
        _config = config;
        _scheduler = scheduler;
        _generator = generator;

        _rng = new Random(config.Seed);
        _cache = new CacheModel(config.CacheHitProbability, config.CacheMissPenalty);
    }

    public SimulationReport Run()
    {
        Initialize();

        while (_eventQueue.Count > 0 && _time <= _config.EndTime)
        {
            SimEvent e = _eventQueue.Dequeue();
            _time = e.Time;
            e.Execute(this);
        }

        return _metrics.BuildReport(_scheduler.Name, _config.EndTime);
    }

    private void Initialize()
    {
        _time = 0;
        _generatedCount = 0;

        // Schedule first arrival + initial dispatch
        ScheduleNextArrival(startTime: 0);
        _eventQueue.Enqueue(new DispatchEvent(_time));
    }

    private void ScheduleNextArrival(int startTime)
    {
        if (_generatedCount >= _config.ProcessCountTarget) return;

        int inter = _generator.NextInterarrival();
        int arrivalTime = startTime + inter;

        var p = _generator.CreateProcess(_pidCounter++, arrivalTime);
        _generatedCount++;

        _eventQueue.Enqueue(new ArrivalEvent(arrivalTime, p));

        // Chain schedule: next arrival based on this arrival
        // (keeps arrivals spaced realistically)
        if (_generatedCount < _config.ProcessCountTarget)
        {
            // schedule future arrival relative to this one
            // we do it by enqueueing a "meta" arrival by calling again inside the arrival event handler
            // (kept simple; see ArrivalEvent.Execute)
        }
    }

    // Called by events
    internal void OnArrival(SimProcess p)
    {
        p.State = ProcessState.Ready;
        _scheduler.Enqueue(p);

        // Schedule next arrival after this one, until target count
        ScheduleNextArrival(startTime: p.ArrivalTime);

        // Trigger a dispatch decision at the same time if CPU is idle
        _eventQueue.Enqueue(new DispatchEvent(_time));
    }

    internal void OnDispatch()
    {
        if (_running is not null) return; // already running
        if (!_scheduler.HasReadyProcess) return;

        // Context switch overhead
        if (_cpuBusy)
        {
            _metrics.RecordContextSwitch();
        }

        int dispatchTime = _time + _config.ContextSwitchCost;
        _eventQueue.Enqueue(new StartRunEvent(dispatchTime));
    }

    internal void OnStartRun()
    {
        if (_running is not null) return;
        var p = _scheduler.Dequeue();
        if (p is null) return;

        _running = p;
        _cpuBusy = true;
        p.State = ProcessState.Running;

        if (p.FirstStartTime is null)
            p.FirstStartTime = _time;

        int slice = _scheduler.GetTimeSlice();
        p.QuantumRemaining = slice;

        // Decide how many CPU time units we execute until:
        // - quantum expires (RR), or
        // - process completes (FCFS acts like "infinite quantum")
        int plannedRun = Math.Min(p.RemainingTime, slice);

        // Add cache penalties based on probabilistic access checks during this run
        int cacheDelay = ComputeCacheDelayForRun(p, plannedRun);

        // Book CPU busy time excluding cache delay? In reality, cache miss still stalls CPU
        // but for this model we count it as "busy" time since CPU is occupied by the process.
        _metrics.AddCpuBusy(plannedRun + cacheDelay);

        int endTime = _time + plannedRun + cacheDelay;

        // Update remaining immediately (so event uses correct state)
        p.RemainingTime -= plannedRun;
        p.QuantumRemaining -= plannedRun;

        if (p.RemainingTime <= 0)
        {
            _eventQueue.Enqueue(new CompleteEvent(endTime, p));
        }
        else
        {
            _eventQueue.Enqueue(new SliceCompleteEvent(endTime, p));
        }
    }

    private int ComputeCacheDelayForRun(SimProcess p, int cpuUnits)
    {
        int delay = 0;

        for (int i = 0; i < cpuUnits; i++)
        {
            // chance of a memory access check this time unit
            if (_rng.NextDouble() < p.MemAccessRate)
            {
                bool hit = _cache.IsHit(_rng);
                _metrics.RecordCacheAccess(hit);

                if (!hit)
                    delay += _cache.MissPenalty;
            }
        }

        return delay;
    }

    internal void OnSliceComplete(SimProcess p)
    {
        // RR: preempt and re-enqueue
        p.State = ProcessState.Ready;
        _running = null;

        _scheduler.Enqueue(p);
        _eventQueue.Enqueue(new DispatchEvent(_time));
    }

    internal void OnComplete(SimProcess p)
    {
        p.State = ProcessState.Terminated;
        p.FinishTime = _time;
        _metrics.RecordCompletion(p);

        _running = null;
        _eventQueue.Enqueue(new DispatchEvent(_time));
    }

    // Event types
    private sealed class ArrivalEvent : SimEvent
    {
        private readonly SimProcess _p;
        public ArrivalEvent(int time, SimProcess p) : base(time, EventType.Arrival) => _p = p;
        public override void Execute(SimulationEngine engine) => engine.OnArrival(_p);
    }

    private sealed class DispatchEvent : SimEvent
    {
        public DispatchEvent(int time) : base(time, EventType.Dispatch) { }
        public override void Execute(SimulationEngine engine) => engine.OnDispatch();
    }

    private sealed class StartRunEvent : SimEvent
    {
        public StartRunEvent(int time) : base(time, EventType.Dispatch) { }
        public override void Execute(SimulationEngine engine) => engine.OnStartRun();
    }

    private sealed class SliceCompleteEvent : SimEvent
    {
        private readonly SimProcess _p;
        public SliceCompleteEvent(int time, SimProcess p) : base(time, EventType.SliceComplete) => _p = p;
        public override void Execute(SimulationEngine engine) => engine.OnSliceComplete(_p);
    }

    private sealed class CompleteEvent : SimEvent
    {
        private readonly SimProcess _p;
        public CompleteEvent(int time, SimProcess p) : base(time, EventType.ProcessComplete) => _p = p;
        public override void Execute(SimulationEngine engine) => engine.OnComplete(_p);
    }
}
