namespace CpuSchedulingSim.Metrics;

public sealed class SimulationReport
{
    public string SchedulerName { get; init; } = "Unknown";
    public int EndTime { get; init; }

    public int CompletedProcesses { get; init; }

    public double AverageWaitTime { get; init; }
    public double AverageTurnaroundTime { get; init; }

    public double CpuUtilization { get; init; }
    public int ContextSwitches { get; init; }

    public int CacheHits { get; init; }
    public int CacheMisses { get; init; }
    public double CacheHitRate { get; init; }
}
