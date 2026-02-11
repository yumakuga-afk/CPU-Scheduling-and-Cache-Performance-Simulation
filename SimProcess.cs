namespace CpuSchedulingSim.Models;

public enum ProcessState
{
    New,
    Ready,
    Running,
    Waiting,
    Terminated
}

public sealed class SimProcess
{
    public int Pid { get; }
    public int ArrivalTime { get; }
    public int TotalBurstTime { get; }
    public int RemainingTime { get; set; }
    public int Priority { get; }

    // Probability that a "memory access check" happens in a time unit
    public double MemAccessRate { get; }

    public ProcessState State { get; set; } = ProcessState.New;

    // Metrics timestamps
    public int? FirstStartTime { get; set; }
    public int? FinishTime { get; set; }

    // For RR: track how much time left in this quantum
    public int QuantumRemaining { get; set; }

    public SimProcess(int pid, int arrivalTime, int totalBurstTime, int priority, double memAccessRate)
    {
        Pid = pid;
        ArrivalTime = arrivalTime;
        TotalBurstTime = totalBurstTime;
        RemainingTime = totalBurstTime;
        Priority = priority;
        MemAccessRate = memAccessRate;
    }
}
