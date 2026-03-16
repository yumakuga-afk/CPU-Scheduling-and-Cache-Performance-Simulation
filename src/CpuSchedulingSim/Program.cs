using CpuSchedulingSim.Metrics;
using CpuSchedulingSim.Scheduling;
using CpuSchedulingSim.Simulation;
using CpuSchedulingSim.Workload;
using CpuSchedulingSim.Output;


namespace CpuSchedulingSim;

internal static class Program
{
    private enum RunMode
    {
        Single,
        All
    }

    private enum SchedulerType
    {
        FCFS,
        RR,
        PRIORITY
    }

    private static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            RunFromArguments(args);
        }
        else
        {
            RunInteractiveMenu();
        }
    }

    private static void RunInteractiveMenu()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.Clear();
            Console.WriteLine("=== CPU Scheduling Simulation Runner ===");
            Console.WriteLine("1. Run a single scheduler");
            Console.WriteLine("2. Run all schedulers");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            string? mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    RunSingleSchedulerInteractive();
                    break;

                case "2":
                    RunAllSchedulersInteractive();
                    break;

                case "3":
                    keepRunning = false;
                    continue;

                default:
                    Console.WriteLine("Invalid selection. Press any key to try again...");
                    Console.ReadKey();
                    continue;
            }

            Console.WriteLine();
            Console.Write("Would you like to run another simulation? (y/n): ");
            string? again = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (again != "y" && again != "yes")
            {
                keepRunning = false;
            }
        }
    }

    private static void RunSingleSchedulerInteractive()
    {
        Console.Clear();
        Console.WriteLine("=== Single Scheduler Mode ===");
        Console.WriteLine("1. FCFS");
        Console.WriteLine("2. Round Robin");
        Console.WriteLine("3. Priority");
        Console.Write("Select a scheduler: ");

        string? schedulerChoice = Console.ReadLine();

        SchedulerType schedulerType = schedulerChoice switch
        {
            "1" => SchedulerType.FCFS,
            "2" => SchedulerType.RR,
            "3" => SchedulerType.PRIORITY,
            _ => SchedulerType.FCFS
        };

        int runs = PromptForInt("Enter number of runs: ", minValue: 1);
        int quantum = 4;

        if (schedulerType == SchedulerType.RR)
        {
            quantum = PromptForInt("Enter Round Robin quantum: ", minValue: 1);
        }

        int seed = PromptForInt("Enter random seed (default 42): ", minValue: 0, defaultValue: 42);

        double lambda = PromptForDouble("Enter Poisson arrival rate lambda (default 0.2): ", minValue: 0.0001, defaultValue: 0.2);

        var runPlan = Enumerable.Repeat(schedulerType, runs).ToList();
        ExecuteRunPlan(runPlan, quantum, seed, lambda);

    }

    private static void RunAllSchedulersInteractive()
    {
        Console.Clear();
        Console.WriteLine("=== All Schedulers Mode ===");

        int runs = PromptForInt("Enter total number of runs (minimum 3): ", minValue: 3);
        int quantum = PromptForInt("Enter Round Robin quantum: ", minValue: 1);
        int seed = PromptForInt("Enter random seed (default 42): ", minValue: 0, defaultValue: 42);
        double lambda = PromptForDouble("Enter Poisson arrival rate lambda (default 0.2): ", minValue: 0.0001, defaultValue: 0.2);

        List<SchedulerType> runPlan = BuildAllSchedulerRunPlan(runs, seed);
        ExecuteRunPlan(runPlan, quantum, seed, lambda);
    }

    private static void RunFromArguments(string[] args)
    {
        RunMode mode = ParseMode(args);
        int runs = ParseIntArg(args, "--runs", 1);
        int quantum = ParseIntArg(args, "--quantum", 4);
        int seed = ParseIntArg(args, "--seed", 42);
        double lambda = ParseDoubleArg(args, "--lambda", 0.2);


        if (runs < 1)
        {
            Console.WriteLine("Error: --runs must be at least 1.");
            return;
        }

        List<SchedulerType> runPlan;

        if (mode == RunMode.Single)
        {
            SchedulerType schedulerType = ParseScheduler(args);
            runPlan = Enumerable.Repeat(schedulerType, runs).ToList();
        }
        else
        {
            if (runs < 3)
            {
                Console.WriteLine("Error: --mode all requires --runs >= 3 so each scheduler can be used at least once.");
                return;
            }

            runPlan = BuildAllSchedulerRunPlan(runs, seed);
        }

        ExecuteRunPlan(runPlan, quantum, seed, lambda);
    }

    private static double ParseDoubleArg(string[] args, string key, double defaultValue)
    {
        string raw = ParseStringArg(args, key, defaultValue.ToString());
        return double.TryParse(raw, out double value) ? value : defaultValue;
    }

    private static double PromptForDouble(string prompt, double minValue, double? defaultValue = null)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
            {
                return defaultValue.Value;
            }

            if (double.TryParse(input, out double value) && value >= minValue)
            {
                return value;
            }

            Console.WriteLine($"Please enter a number greater than or equal to {minValue}.");
        }
    }

    private static void ExecuteRunPlan(List<SchedulerType> runPlan, int quantum, int seed, double lambda)
    {
        Console.WriteLine();
        Console.WriteLine("=== Running Simulation ===");
        Console.WriteLine($"Total Runs: {runPlan.Count}");
        Console.WriteLine();

        var rng = new Random(seed);

        for (int i = 0; i < runPlan.Count; i++)
        {
            SchedulerType schedulerType = runPlan[i];
            IScheduler scheduler = CreateScheduler(schedulerType, quantum, rng);

            var config = new SimConfig
            {
                Seed = seed + i,
                EndTime = 500,
                ContextSwitchCost = 1,
                CacheHitProbability = 0.85,
                CacheMissPenalty = 3,
                Lambda = lambda,
                ProcessCountTarget = 50,
                SchedulerName = scheduler.Name
            };

            var generator = new WorkloadGenerator(
                seed: config.Seed,
                lambda: config.Lambda
            );

            var engine = new SimulationEngine(config, scheduler, generator);
            SimulationReport report = engine.Run();

            string runId = $"run_{i + 1:D3}";
            report.RunId = runId;
            report = new SimulationReport
            {
                RunId = runId,
                SchedulerName = report.SchedulerName,
                EndTime = report.EndTime,
                CompletedProcesses = report.CompletedProcesses,
                AverageWaitTime = report.AverageWaitTime,
                AverageTurnaroundTime = report.AverageTurnaroundTime,
                CpuUtilization = report.CpuUtilization,
                ContextSwitches = report.ContextSwitches,
                CacheHits = report.CacheHits,
                CacheMisses = report.CacheMisses,
                CacheHitRate = report.CacheHitRate
            };

            string outputDirectory = Path.Combine(AppContext.BaseDirectory, "Output");
            CsvExporter.ExportRunSummary(outputDirectory, report);
            CsvExporter.ExportPerProcessResults(outputDirectory, report.RunId, engine.Metrics.ProcessResults);


            Console.WriteLine($"--- Run {i + 1} / {runPlan.Count} ---");
            Console.WriteLine($"Scheduler: {report.SchedulerName}");
            Console.WriteLine($"Completed Processes: {report.CompletedProcesses}");
            Console.WriteLine($"Avg Wait Time: {report.AverageWaitTime:F2}");
            Console.WriteLine($"Avg Turnaround Time: {report.AverageTurnaroundTime:F2}");
            Console.WriteLine($"CPU Utilization: {report.CpuUtilization:P2}");
            Console.WriteLine($"Context Switches: {report.ContextSwitches}");
            Console.WriteLine($"Cache Hit Rate: {report.CacheHitRate:P2}");
            Console.WriteLine($"Cache Misses: {report.CacheMisses}");
            Console.WriteLine();
        }

        Console.WriteLine("Simulation run(s) complete.");
    }

    private static int PromptForInt(string prompt, int minValue, int? defaultValue = null)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
            {
                return defaultValue.Value;
            }

            if (int.TryParse(input, out int value) && value >= minValue)
            {
                return value;
            }

            Console.WriteLine($"Please enter an integer greater than or equal to {minValue}.");
        }
    }

    private static RunMode ParseMode(string[] args)
    {
        string value = ParseStringArg(args, "--mode", "single").ToLowerInvariant();

        return value switch
        {
            "single" => RunMode.Single,
            "all" => RunMode.All,
            _ => RunMode.Single
        };
    }

    private static SchedulerType ParseScheduler(string[] args)
    {
        string value = ParseStringArg(args, "--scheduler", "fcfs").ToLowerInvariant();

        return value switch
        {
            "fcfs" => SchedulerType.FCFS,
            "rr" => SchedulerType.RR,
            "roundrobin" => SchedulerType.RR,
            "priority" => SchedulerType.PRIORITY,
            _ => SchedulerType.FCFS
        };
    }

    private static IScheduler CreateScheduler(SchedulerType type, int quantum, Random rng)
    {
        return type switch
        {
            SchedulerType.FCFS => new FCFSScheduler(),
            SchedulerType.RR => new RoundRobinScheduler(quantum),
            SchedulerType.PRIORITY => new PriorityScheduler(isPreemptive: rng.Next(2) == 0),
            _ => new FCFSScheduler()
        };
    }


    private static List<SchedulerType> BuildAllSchedulerRunPlan(int totalRuns, int seed)
    {
        var rng = new Random(seed);

        var schedulers = new List<SchedulerType>
        {
            SchedulerType.FCFS,
            SchedulerType.RR,
            SchedulerType.PRIORITY
        };

        var plan = new List<SchedulerType>();
        plan.AddRange(schedulers);

        while (plan.Count < totalRuns)
        {
            plan.Add(schedulers[rng.Next(schedulers.Count)]);
        }

        return plan.OrderBy(_ => rng.Next()).ToList();
    }

    private static string ParseStringArg(string[] args, string key, string defaultValue)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i].Equals(key, StringComparison.OrdinalIgnoreCase))
                return args[i + 1];
        }

        return defaultValue;
    }

    private static int ParseIntArg(string[] args, string key, int defaultValue)
    {
        string raw = ParseStringArg(args, key, defaultValue.ToString());
        return int.TryParse(raw, out int value) ? value : defaultValue;
    }
}
