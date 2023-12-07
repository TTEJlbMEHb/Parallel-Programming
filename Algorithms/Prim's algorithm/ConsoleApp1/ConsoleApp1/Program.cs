using System;
using System.Collections.Generic;
using System.Diagnostics;

class PrimAlgorithm
{
    private int V;
    private int startVertex;
    private int threads;
    ParallelOptions parallelOptions = new ParallelOptions();    

    public PrimAlgorithm()
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

    private int MinDistance(int[] distance, bool[] shortestPath)
    {
        int minDistance = int.MaxValue;
        int minIndex = int.MaxValue;

        for (int v = 0; v < V; v++)
        {
            if (!shortestPath[v] && distance[v] <= minDistance)
            {
                minDistance = distance[v];
                minIndex = v;
            }
        }
        if (minIndex != int.MaxValue)
        {
            shortestPath[minIndex] = true;
        }

        return minIndex;
    }

    public void Prim(int[,] graph, int startVertex)
    {
        V = graph.GetLength(0);
        this.startVertex = startVertex;

        int[] parent = new int[V];
        int[] distance = new int[V];
        bool[] shortestPath = new bool[V];

        for (int i = 0; i < V; i++)
        {
            distance[i] = int.MaxValue;
            shortestPath[i] = false;
        }

        distance[startVertex] = 0;
        parent[startVertex] = -1;

        for (int count = 0; count < V - 1; count++)
        {
            int minIndex = MinDistance(distance, shortestPath);

            for (int v = 0; v < V; v++)
            {
                if (graph[minIndex, v] != 0 && !shortestPath[v] && graph[minIndex, v] < distance[v])
                {
                    parent[v] = minIndex;
                    distance[v] = graph[minIndex, v];
                }
            }
        }

        Print(parent, graph);
    }

    public void PrimParallel(int[,] graph, int startVertex)
    {
        V = graph.GetLength(0);
        this.startVertex = startVertex;

        int[] parent = new int[V];
        int[] distance = new int[V];
        bool[] shortestPath = new bool[V];

        Parallel.For(0, V, parallelOptions, i =>
        {
            distance[i] = int.MaxValue;
            shortestPath[i] = false;
        });

        distance[startVertex] = 0;
        parent[startVertex] = -int.MaxValue;

        Parallel.For(0, V - 1, parallelOptions, count =>
        {
            int minIndex = MinDistance(distance, shortestPath);

            for (int v = 0; v < V; v++)
            {
                if (graph[minIndex, v] != 0 && !shortestPath[v] && graph[minIndex, v] < distance[v])
                {
                    parent[v] = minIndex;
                    distance[v] = graph[minIndex, v];
                }
            }
        });

        Print(parent, graph);
    }

    private char GetLabel(int index)
    {
        return (char)('A' + index);
    }

    private void Print(int[] parent, int[,] graph)
    {
        var edges = new List<(char, char, int)>();
        var sum = 0;

        for (int i = 0; i < V; i++)
        {
            if (i != startVertex)
            {
                sum += graph[i, parent[i]];
                char from = GetLabel(parent[i]);
                char to = GetLabel(i);
                edges.Add((from, to, graph[i, parent[i]]));
            }
        }

        edges.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        Console.WriteLine("Edge \tWeight");
        foreach (var (from, to, weight) in edges)
        {
            Console.WriteLine($"{from} - {to} \t {weight}");
        }

        Console.WriteLine($"Total weight = {sum}");
    }

    public int[,] FillGraph()
    {
        int[,] graph = new int[,]
        {  //A  B  C  D  E  F  G
            {0, 7, 0, 5, 0, 0, 0},//A
            {7, 0, 8, 9, 7, 0, 0},//B
            {0, 8, 0, 0, 5, 0, 0},//C
            {5, 9, 0, 0, 15, 5, 0},//D
            {0, 7, 5, 15, 0, 8, 9},//E
            {0, 0, 0, 6, 8, 0, 11},//F
            {0, 0, 0, 0, 9, 11, 0},//G
        };

        return graph;
    }

    public int[,] FillRandom(int vertices)
    {
        Random random = new Random();
        int[,] graph = new int[vertices, vertices];

        for (int i = 0; i < vertices; i++)
        {
            for (int j = 0; j < vertices; j++)
            {
                if (i == j)
                {
                    graph[i, j] = 0;
                }
                else
                {
                    int value = random.Next(1, 100);
                    graph[i, j] = value;
                    graph[j, i] = value;
                }
            }
        }

        return graph;
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
    static void Main(string[] args)
    {
        var prim = new PrimAlgorithm();
        var default_watch = new Stopwatch();
        var parallel_watch = new Stopwatch();
        var threads = 6;
        var vertices = 10000;
        int[,] graph;
        int startVertex = 0;

        prim.Threads(threads);
        graph = prim.FillGraph();
        //graph = prim.FillRandom(vertices);

        default_watch.Start();
        prim.Prim(graph, startVertex);
        default_watch.Stop();
        Console.WriteLine($"Prim's algorithm time: {default_watch.ElapsedMilliseconds} ms\n");

        parallel_watch.Start();
        prim.PrimParallel(graph, startVertex);
        parallel_watch.Stop();
        Console.WriteLine($"Prim's algorithm time with {threads} threads: {parallel_watch.ElapsedMilliseconds} ms\n");

        double acceleration = prim.Acceleration(default_watch, parallel_watch);
        Console.WriteLine($"\nAcceleration = {acceleration.ToString("f2")}");
        double prim_efficiency = prim.Efficiency(acceleration, threads);
        Console.WriteLine($"Efficiency = {prim_efficiency.ToString("f2")}%");
    }
}
