using System.Globalization;
using System.Text;
using CpuSchedulingSim.Metrics;

namespace CpuSchedulingSim.Output;

public static class CsvExporter
{
    public static void ExportRunSummary(string outputDirectory, SimulationReport report)
    {
        Directory.CreateDirectory(outputDirectory);

        string path = Path.Combine(outputDirectory, $"{report.RunId}_summary.csv");

        var sb = new StringBuilder();
        sb.AppendLine("RunId,Scheduler,EndTime,CompletedProcesses,AverageWaitTime,AverageTurnaroundTime,CpuUtilization,ContextSwitches,CacheHits,CacheMisses,CacheHitRate");
        sb.AppendLine(string.Join(",",
            report.RunId,
            Escape(report.SchedulerName),
            report.EndTime,
            report.CompletedProcesses,
            report.AverageWaitTime.ToString("F4", CultureInfo.InvariantCulture),
            report.AverageTurnaroundTime.ToString("F4", CultureInfo.InvariantCulture),
            report.CpuUtilization.ToString("F4", CultureInfo.InvariantCulture),
            report.ContextSwitches,
            report.CacheHits,
            report.CacheMisses,
            report.CacheHitRate.ToString("F4", CultureInfo.InvariantCulture)
        ));

        File.WriteAllText(path, sb.ToString());
    }

    public static void ExportPerProcessResults(string outputDirectory, string runId, IEnumerable<ProcessResult> processResults)
    {
        Directory.CreateDirectory(outputDirectory);

        string path = Path.Combine(outputDirectory, $"{runId}_processes.csv");

        var sb = new StringBuilder();
        sb.AppendLine("Pid,ArrivalTime,BurstTime,Priority,FirstStartTime,FinishTime,WaitingTime,TurnaroundTime");

        foreach (var p in processResults)
        {
            sb.AppendLine(string.Join(",",
                p.Pid,
                p.ArrivalTime,
                p.BurstTime,
                p.Priority,
                p.FirstStartTime?.ToString() ?? "",
                p.FinishTime?.ToString() ?? "",
                p.WaitingTime,
                p.TurnaroundTime
            ));
        }

        File.WriteAllText(path, sb.ToString());
    }

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}
