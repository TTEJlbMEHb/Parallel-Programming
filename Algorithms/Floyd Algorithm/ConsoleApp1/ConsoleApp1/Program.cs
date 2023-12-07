using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

class V
{
    public int A0 { get; set; }
    public int An { get; set; }
    public int Value { get; set; }

    public V(int a0, int an, int value)
    {
        A0 = a0;
        An = an;
        Value = value;
    }
}

class Floyd
{
    int threads;
    ParallelOptions parallelOptions = new ParallelOptions();

    public Floyd()
    {
        threads = Environment.ProcessorCount;
        parallelOptions.MaxDegreeOfParallelism = threads;
    }

    public ParallelOptions Threads(int threads)
    {
        try
        {
            if (threads > 0)
            {
                parallelOptions.MaxDegreeOfParallelism = threads;
            }
            else
            {
                throw new Exception("Threads must be more than 0");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            threads = Environment.ProcessorCount;
            parallelOptions.MaxDegreeOfParallelism = threads;
        }
        return parallelOptions;
    }

    public void InitGraph(int[,] graph, int n, List<V> V)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    graph[i, j] = 0;
                }                  
                else
                {
                    graph[i, j] = int.MaxValue;
                }            
            }
        }
        foreach (var v in V)
        {
            graph[v.A0 - 1, v.An - 1] = v.Value;
        }
    }

    public void PrintMatrix(int[,] graph, int n)
    {
        Console.WriteLine("\nOriginal Matrix:\n");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (graph[i, j] == int.MaxValue)
                {
                    Console.Write("INF\t");
                }
                else
                {
                    Console.Write(graph[i, j] + "\t");
                }
            }
            Console.WriteLine();
        }
    }

    public void FloydAlgorithm(int[,] graph, int n)
    {
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (graph[i, k] != int.MaxValue && graph[k, j] != int.MaxValue &&
                        graph[i, k] + graph[k, j] < graph[i, j])
                    {
                        graph[i, j] = graph[i, k] + graph[k, j];
                    }
                }
            }
        }
    }

    public void FloydAlgorithm_Parallel(int[,] graph, int n)
    {

        Parallel.For(0, n, parallelOptions, k =>
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (graph[i, k] != int.MaxValue && graph[k, j] != int.MaxValue &&
                        graph[i, k] + graph[k, j] < graph[i, j])
                    {
                        graph[i, j] = graph[i, k] + graph[k, j];
                    }
                }
            }
        });
    }

    public void Fill(List<V> list)
    {
        list.Add(new V(1, 2, 1));
        list.Add(new V(2, 3, 4));
        list.Add(new V(2, 4, 1));
        list.Add(new V(4, 3, 1));
        list.Add(new V(1, 3, 6));

        //list.Add(new V(1, 2, 8));
        //list.Add(new V(2, 3, 5));
        //list.Add(new V(2, 4, 7));
        //list.Add(new V(3, 4, 6));
        //list.Add(new V(4, 6, 3));
        //list.Add(new V(4, 5, 4));
        //list.Add(new V(5, 6, 1));
    }

    public void RandomV(int numOfV, Random random, List<V> random_V)
    {
        for (int i = 0; i < numOfV; i++)
        {
            int a0 = random.Next(1, numOfV + 1);
            int an = random.Next(1, numOfV + 1);
            int value = random.Next(1, 10);

            var v = new V(a0, an, value);
            random_V.Add(v);
        }
    }

    public void Solution(int[,] graph, int n)
    {
        Console.WriteLine("\nResult:\n");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (graph[i, j] == int.MaxValue)
                {
                    Console.Write("INF\t");
                }
                else
                {
                    Console.Write(graph[i, j] + "\t");
                }
            }
            Console.WriteLine();
        }
    }

    public double Acceleration(Stopwatch stopwatch1, Stopwatch stopwatch2)
    {
        double time1 = (double)stopwatch1.ElapsedMilliseconds;
        double time2 = (double)stopwatch2.ElapsedMilliseconds;
        double boost = time1 / time2;
        return boost;
    }

    public double Efficiency(double acceleration, int threads)
    {
        double efficiency = acceleration / threads * 100;
        return efficiency;
    }
}

class Program
{
    public static void Main()
    {
        var numOfV = 500;
        int[,] random_graph = new int[numOfV, numOfV];
        int threads = 6;
        var floyd = new Floyd();
        var custom_V = new List<V>();
        var random_V = new List<V>();
        var random = new Random();
        var default_watch = new Stopwatch();
        var parallel_watch = new Stopwatch();
        floyd.Threads(threads);

        floyd.Fill(custom_V);

        int n = custom_V.Max(v => v.An);
        int[,] custom_graph = new int[n, n];
        
        floyd.InitGraph(custom_graph, n, custom_V);
        floyd.PrintMatrix(custom_graph, n);
        floyd.FloydAlgorithm(custom_graph, n);
        floyd.Solution(custom_graph, n);

        floyd.RandomV(numOfV, random, random_V);
        default_watch = Stopwatch.StartNew();
                
        floyd.InitGraph(random_graph, numOfV, random_V);
        floyd.FloydAlgorithm(random_graph, numOfV);
        default_watch.Stop();

        Console.WriteLine($"\nFloyd execution Time: {default_watch.ElapsedMilliseconds} ms");

        parallel_watch = Stopwatch.StartNew();

        floyd.InitGraph(random_graph, numOfV, random_V);
        floyd.FloydAlgorithm_Parallel(random_graph, numOfV);
        parallel_watch.Stop();

        Console.WriteLine($"\nParallel Floyd execution time: {parallel_watch.ElapsedMilliseconds} ms");

        double acceleration = floyd.Acceleration(default_watch, parallel_watch);
        Console.WriteLine($"\nAcceleration = {acceleration.ToString("f2")}");
        double floyd_efficiency = floyd.Efficiency(acceleration, threads);
        Console.WriteLine($"Efficiency = {floyd_efficiency.ToString("f2")}%");
    }
}
