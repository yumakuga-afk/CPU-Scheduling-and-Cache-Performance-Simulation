namespace CpuSchedulingSim.Simulation;

public sealed class SimConfig
{
    public int Seed { get; set; } = 1;
    public int EndTime { get; set; } = 500;

    public int ContextSwitchCost { get; set; } = 1;

    public double CacheHitProbability { get; set; } = 0.9;
    public int CacheMissPenalty { get; set; } = 3;

    public int MeanInterarrivalTime { get; set; } = 6;
    public int ProcessCountTarget { get; set; } = 50;

    public string SchedulerName { get; set; } = "Unknown";
}
