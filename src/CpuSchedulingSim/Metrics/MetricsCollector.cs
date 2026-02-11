using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Metrics;

public sealed class MetricsCollector
{
    private readonly List<int> _waitTimes = new();
    private readonly List<int> _turnaroundTimes = new();

    public int ContextSwitches { get; private set; }
    public int CpuBusyTime { get; private set; }

    public int CacheHits { get; private set; }
    public int CacheMisses { get; private set; }

    public void AddCpuBusy(int amount) => CpuBusyTime += Math.Max(0, amount);

    public void RecordContextSwitch() => ContextSwitches++;

    public void RecordCacheAccess(bool hit)
    {
        if (hit) CacheHits++;
        else CacheMisses++;
    }

    public void RecordCompletion(SimProcess p)
    {
        if (p.FinishTime is null) return;

        int turnaround = p.FinishTime.Value - p.ArrivalTime;
        int runTime = p.TotalBurstTime;
        int wait = turnaround - runTime;

        _turnaroundTimes.Add(turnaround);
        _waitTimes.Add(wait);
    }

    public SimulationReport BuildReport(string schedulerName, int endTime)
    {
        double avgWait = _waitTimes.Count > 0 ? _waitTimes.Average() : 0.0;
        double avgTurn = _turnaroundTimes.Count > 0 ? _turnaroundTimes.Average() : 0.0;

        int totalCache = CacheHits + CacheMisses;
        double hitRate = totalCache > 0 ? (double)CacheHits / totalCache : 0.0;

        return new SimulationReport
        {
            SchedulerName = schedulerName,
            EndTime = endTime,
            CompletedProcesses = _turnaroundTimes.Count,
            AverageWaitTime = avgWait,
            AverageTurnaroundTime = avgTurn,
            CpuUtilization = endTime > 0 ? (double)CpuBusyTime / endTime : 0.0,
            ContextSwitches = ContextSwitches,
            CacheHits = CacheHits,
            CacheMisses = CacheMisses,
            CacheHitRate = hitRate
        };
    }
}
