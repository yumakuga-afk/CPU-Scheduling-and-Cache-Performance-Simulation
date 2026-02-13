using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Workload;

public sealed class WorkloadGenerator
{
    private readonly Random _rng;
    private readonly int _meanInterarrival;

    public WorkloadGenerator(int seed, int meanInterarrivalTime)
    {
        _rng = new Random(seed);
        _meanInterarrival = Math.Max(1, meanInterarrivalTime);
    }

    // Simple interarrival sampling: geometric-like using uniform range around mean
    public int NextInterarrival()
    {
        // Keeps it simple for M2; you can replace with Poisson/exponential later.
        // Range [1, 2*mean]
        return _rng.Next(1, 2 * _meanInterarrival + 1);
    }

    public SimProcess CreateProcess(int pid, int arrivalTime)
    {
        int burst = _rng.Next(2, 20);       // CPU burst length
        int priority = _rng.Next(1, 11);    // 1..10
        double memRate = _rng.NextDouble() * 0.4; // 0..0.4 chance per time unit

        return new SimProcess(pid, arrivalTime, burst, priority, memRate);
    }
}
