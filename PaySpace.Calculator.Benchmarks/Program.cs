using BenchmarkDotNet.Running;

namespace PaySpace.Calculator.Benchmarks;
class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CalculationBenchmark>();
        Console.WriteLine(summary);
    }
}