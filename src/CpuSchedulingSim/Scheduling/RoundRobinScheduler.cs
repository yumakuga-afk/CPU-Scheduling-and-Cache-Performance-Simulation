using System.Collections.Generic;
using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Scheduling;

public sealed class RoundRobinScheduler : IScheduler
{
    private readonly Queue<SimProcess> _ready = new();
    private readonly int _quantum;

    public RoundRobinScheduler(int timeQuantum)
    {
        _quantum = timeQuantum < 1 ? 1 : timeQuantum;
    }

    public string Name => $"Round Robin (q={_quantum})";

    public void Enqueue(SimProcess p) => _ready.Enqueue(p);

    public SimProcess? Dequeue() => _ready.Count > 0 ? _ready.Dequeue() : null;

    public bool HasReadyProcess => _ready.Count > 0;

    public int GetTimeSlice() => _quantum;
}
