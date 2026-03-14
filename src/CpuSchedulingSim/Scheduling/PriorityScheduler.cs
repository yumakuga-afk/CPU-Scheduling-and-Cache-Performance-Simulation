using System.Collections.Generic;
using System.Linq;
using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Scheduling;

public sealed class PriorityScheduler : IScheduler
{
    private readonly List<SimProcess> _ready = new();

    /// <summary>
    /// If true, this scheduler is intended to support preemptive priority behavior.
    /// The simulation engine must check this flag and decide whether to preempt.
    /// </summary>
    public bool IsPreemptive { get; }

    public PriorityScheduler(bool isPreemptive = false)
    {
        IsPreemptive = isPreemptive;
    }

    public string Name => IsPreemptive
        ? "Priority Scheduling (Preemptive)"
        : "Priority Scheduling (Non-Preemptive)";

    public void Enqueue(SimProcess p)
    {
        _ready.Add(p);
    }

    public SimProcess? Dequeue()
    {
        if (_ready.Count == 0)
            return null;

        SimProcess next = _ready
            .OrderBy(p => p.Priority)      // lower value = higher priority
            .ThenBy(p => p.ArrivalTime)    // earlier arrival first
            .ThenBy(p => p.Pid)            // stable deterministic tie-breaker
            .First();

        _ready.Remove(next);
        return next;
    }

    public bool HasReadyProcess => _ready.Count > 0;

    public int GetTimeSlice()
    {
        // Priority scheduling typically runs until completion unless preempted by a higher-priority arrival.
        return int.MaxValue;
    }

    /// <summary>
    /// Returns true if an incoming process should preempt the currently running process.
    /// Only meaningful when IsPreemptive is true.
    /// </summary>
    public bool ShouldPreempt(SimProcess current, SimProcess incoming)
    {
        if (!IsPreemptive)
            return false;

        if (incoming.Priority < current.Priority)
            return true;

        if (incoming.Priority == current.Priority)
        {
            // Optional tie-breaking policy:
            // earlier arrival wins, then lower PID.
            if (incoming.ArrivalTime < current.ArrivalTime)
                return true;

            if (incoming.ArrivalTime == current.ArrivalTime && incoming.Pid < current.Pid)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Useful for debugging or metrics snapshots.
    /// </summary>
    public IReadOnlyList<SimProcess> GetReadyQueueSnapshot()
    {
        return _ready
            .OrderBy(p => p.Priority)
            .ThenBy(p => p.ArrivalTime)
            .ThenBy(p => p.Pid)
            .ToList();
    }
}
