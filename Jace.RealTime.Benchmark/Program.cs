using System;
using System.Diagnostics;
using System.Linq;

namespace Jace.RealTime
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const int nbOfExecutions = 1_000_000;
            const int nbOfRuns = 20;

            string formula = "cos(tan(x))^4.23*sin(x+y+t)";

            Console.WriteLine("Jace.RealTime");
            Console.WriteLine($"Number of runs: {nbOfRuns}");
            Console.WriteLine($"Executions per run: {nbOfExecutions}");
            Console.WriteLine();

            Console.WriteLine($"Formula: {formula}");
            Console.WriteLine();

            CalculationEngine engine = new CalculationEngine();
            Func<float, float, float, float> func = engine
                .Formula(formula)
                .Parameter("x")
                .Parameter("y")
                .Parameter("t")
                .Build();

            float warmup = func(0.0f, 0.0f, 0.0f);

            long[] results = new long[nbOfRuns];
            for (int run = 0; run < nbOfRuns; run++)
            {
                Stopwatch watch = Stopwatch.StartNew();
                for (int i = 0; i < nbOfExecutions; i++)
                {
                    func(1.0f, 2.0f, 3.0f);
                }

                long result = watch.ElapsedMilliseconds;
                results[run] = result;

                Console.WriteLine($"Run {run + 1}: {result}ms");
            }

            Console.WriteLine();
            Console.WriteLine($"Mean: {(float)results.Sum() / nbOfRuns}ms");
            Console.ReadKey();
        }
    }
}