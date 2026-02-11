using System.Collections.Generic;
using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Scheduling;

public sealed class FCFSScheduler : IScheduler
{
    private readonly Queue<SimProcess> _ready = new();

    public string Name => "FCFS";

    public void Enqueue(SimProcess p) => _ready.Enqueue(p);

    public SimProcess? Dequeue() => _ready.Count > 0 ? _ready.Dequeue() : null;

    public bool HasReadyProcess => _ready.Count > 0;

    public int GetTimeSlice()
    {
        // FCFS effectively runs until completion (we still model completion as an event).
        return int.MaxValue;
    }
}
