using System.Collections.Generic;

namespace CpuSchedulingSim.Simulation;

public sealed class EventQueue
{
    // SortedDictionary<time, Queue<event>> keeps chronological ordering
    private readonly SortedDictionary<int, Queue<SimEvent>> _events = new();

    public int Count { get; private set; }

    public void Enqueue(SimEvent e)
    {
        if (!_events.TryGetValue(e.Time, out var bucket))
        {
            bucket = new Queue<SimEvent>();
            _events[e.Time] = bucket;
        }

        bucket.Enqueue(e);
        Count++;
    }

    public SimEvent Dequeue()
    {
        // Get first key (smallest time)
        var first = _events.Keys.GetEnumerator();
        first.MoveNext();
        int time = first.Current;

        var bucket = _events[time];
        SimEvent e = bucket.Dequeue();
        Count--;

        if (bucket.Count == 0)
            _events.Remove(time);

        return e;
    }
}
