using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Scheduling;

public interface IScheduler
{
    string Name { get; }
    void Enqueue(SimProcess p);
    SimProcess? Dequeue();
    bool HasReadyProcess { get; }
    int GetTimeSlice();

    bool ShouldPreempt(SimProcess current, SimProcess incoming);
}
