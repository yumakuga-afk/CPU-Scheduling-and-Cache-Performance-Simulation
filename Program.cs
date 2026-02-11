using CpuSchedulingSim.Metrics;
using CpuSchedulingSim.Scheduling;
using CpuSchedulingSim.Simulation;
using CpuSchedulingSim.Workload;

namespace CpuSchedulingSim;

internal static class Program
{
    private static void Main(string[] args)
    {
        // Choose scheduler model (M2 requirement: at least 2 models implemented)
        // Toggle between FCFS and Round Robin to prove both are coded + working.
        IScheduler scheduler = new RoundRobinScheduler(timeQuantum: 4);
        // IScheduler scheduler = new FCFSScheduler();

        var config = new SimConfig
        {
            Seed = 42,
            EndTime = 500,
            ContextSwitchCost = 1,
            CacheHitProbability = 0.85,
            CacheMissPenalty = 3,
            MeanInterarrivalTime = 6,
            ProcessCountTarget = 50,
            SchedulerName = scheduler.Name
        };

        var generator = new WorkloadGenerator(
            seed: config.Seed,
            meanInterarrivalTime: config.MeanInterarrivalTime
        );

        var engine = new SimulationEngine(config, scheduler, generator);
        SimulationReport report = engine.Run();

        Console.WriteLine("=== CPU Scheduling + Cache Simulation (M2 Initial Implementation) ===");
        Console.WriteLine($"Scheduler: {report.SchedulerName}");
        Console.WriteLine($"Simulated Time: 0..{report.EndTime}");
        Console.WriteLine($"Completed Processes: {report.CompletedProcesses}");
        Console.WriteLine();

        Console.WriteLine($"Avg Wait Time: {report.AverageWaitTime:F2}");
        Console.WriteLine($"Avg Turnaround Time: {report.AverageTurnaroundTime:F2}");
        Console.WriteLine($"CPU Utilization: {report.CpuUtilization:P2}");
        Console.WriteLine($"Context Switches: {report.ContextSwitches}");
        Console.WriteLine($"Cache Hit Rate: {report.CacheHitRate:P2}");
        Console.WriteLine($"Cache Misses: {report.CacheMisses}");
        Console.WriteLine();

        Console.WriteLine("Tip: Switch scheduler in Program.cs to FCFS and re-run to compare.");
    }
}
