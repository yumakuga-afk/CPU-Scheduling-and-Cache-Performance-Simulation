using CpuSchedulingSim.Models;

namespace CpuSchedulingSim.Workload;

public sealed class WorkloadGenerator
{
    private readonly Random _rng;
    private readonly double _lambda;

    public WorkloadGenerator(int seed, double lambda)
    {
        _rng = new Random(seed);

        if (lambda <= 0)
            throw new ArgumentOutOfRangeException(nameof(lambda), "Lambda must be greater than 0.");

        _lambda = lambda;
    }

    /// <summary>
    /// Generates the next interarrival time using an exponential distribution.
    /// For a Poisson arrival process with rate lambda, interarrival times are exponential.
    /// </summary>
    public int NextInterarrival()
    {
        double u = _rng.NextDouble();

        // Prevent log(0)
        if (u >= 1.0)
            u = 0.999999999;

        double exponentialSample = -Math.Log(1.0 - u) / _lambda;

        // Convert to discrete simulation time units
        int discreteTime = (int)Math.Ceiling(exponentialSample);

        return Math.Max(1, discreteTime);
    }

    public SimProcess CreateProcess(int pid, int arrivalTime)
    {
        int burst = _rng.Next(2, 20);           // CPU burst length
        int priority = _rng.Next(1, 11);        // 1..10, lower = higher priority
        double memRate = _rng.NextDouble() * 0.4;

        return new SimProcess(pid, arrivalTime, burst, priority, memRate);
    }
}
