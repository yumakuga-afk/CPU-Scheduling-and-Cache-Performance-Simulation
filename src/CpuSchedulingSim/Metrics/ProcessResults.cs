namespace CpuSchedulingSim.Metrics;

public sealed class ProcessResult
{
    public int Pid { get; init; }
    public int ArrivalTime { get; init; }
    public int BurstTime { get; init; }
    public int Priority { get; init; }

    public int? FirstStartTime { get; init; }
    public int? FinishTime { get; init; }

    public int WaitingTime { get; init; }
    public int TurnaroundTime { get; init; }
}
