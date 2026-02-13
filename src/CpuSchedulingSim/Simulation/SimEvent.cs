namespace CpuSchedulingSim.Simulation;

public enum EventType
{
    Arrival,
    Dispatch,
    SliceComplete,
    ProcessComplete
}

public abstract class SimEvent
{
    public int Time { get; }
    public EventType Type { get; }

    protected SimEvent(int time, EventType type)
    {
        Time = time;
        Type = type;
    }

    public abstract void Execute(SimulationEngine engine);
}
