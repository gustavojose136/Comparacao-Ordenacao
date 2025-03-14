using System.Diagnostics;

namespace Ordenacao.Services
{
    public class SortLogger
    {
        private static readonly ActivitySource ActivitySource = new("Ordenacao");

        public static void LogSortDetails(string algorithmName, int dataSize, long timeElapsed, int comparisons, int swaps)
        {
            using var activity = ActivitySource.StartActivity($"{algorithmName} Execution");

            activity?.SetTag("algorithm.name", algorithmName);
            activity?.SetTag("dataset.size", dataSize);
            activity?.SetTag("execution.time_ms", timeElapsed);
            activity?.SetTag("operation.comparisons", comparisons);
            activity?.SetTag("operation.swaps", swaps);

            Console.WriteLine($"[LOG] {algorithmName}: Size={dataSize}, Time={timeElapsed}ms, Comparisons={comparisons}, Swaps={swaps}");
        }
    }
}