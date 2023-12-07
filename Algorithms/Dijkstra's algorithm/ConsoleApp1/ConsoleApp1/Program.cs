using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class DijkstraAlgorithm
{
    private int V;
    int threads;
    ParallelOptions parallelOptions = new ParallelOptions();

    public DijkstraAlgorithm()
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

    public void Dijkstra(int[,] graph, int startVertex)
    {
        V = graph.GetLength(0);
        int[] distance = new int[V];
        bool[] shortestPath = new bool[V];

        for (int i = 0; i < V; i++)
        {
            distance[i] = int.MaxValue;
            shortestPath[i] = false;
        }

        distance[startVertex - 1] = 0;

        for (int count = 0; count < V - 1; count++)
        {
            int minIndex = MinDistance(distance, shortestPath);

            for (int v = 0; v < V; v++)
            {
                if (!shortestPath[v] && graph[minIndex, v] != 0 && distance[minIndex] != int.MaxValue &&
                    distance[minIndex] + graph[minIndex, v] < distance[v])
                {
                    distance[v] = distance[minIndex] + graph[minIndex, v];
                }
            }
        }
        //PrintSolution(distance);
    }

    public void DijkstraParallel(int[,] graph, int startVertex)
    {
        V = graph.GetLength(0);
        int[] distance = new int[V];
        bool[] shortestPath = new bool[V];

        Parallel.For(0, V, parallelOptions, i =>
        {
            distance[i] = int.MaxValue;
            shortestPath[i] = false;
        });

        distance[startVertex - 1] = 0;

        Parallel.For(0, V - 1, parallelOptions, count =>
        {
            int minIndex = MinDistance(distance, shortestPath);

            for (int v = 0; v < V; v++)
            {
                if (!shortestPath[v] && graph[minIndex, v] != 0 &&
                    distance[minIndex] != int.MaxValue &&
                    distance[minIndex] + graph[minIndex, v] < distance[v])
                {
                    distance[v] = distance[minIndex] + graph[minIndex, v];
                }
            }
        });
        //PrintSolution(distance);
    }

    private void PrintSolution(int[] distance)
    {
        Console.WriteLine("Vertex \t\t Distance");
        for (int i = 0; i < V; i++)
        {
            Console.WriteLine(i + 1 + " \t\t " + distance[i]);
        }
    }

    public int[,] FillGraph()
    {
        int[,] graph = new int[,]
        {
            {0, 2, 1, 0, 0},
            {2, 0, 4, 0, 0},
            {1, 4, 0, 2, 1},
            {0, 0, 2, 0, 5},
            {0, 0, 1, 5, 0}
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
        var startVertex = 2;
        var threads = 12;
        var vertices = 10000;
        var dijkstra = new DijkstraAlgorithm();
        var default_watch = new Stopwatch();
        var parallel_watch = new Stopwatch();

        dijkstra.Threads(threads);
        //int[,] graph = dijkstra.FillGraph();
        int[,] graph = dijkstra.FillRandom(vertices);

        default_watch.Start();
        dijkstra.Dijkstra(graph, startVertex);
        default_watch.Stop();
        Console.WriteLine($"Dijkstra execution Time: {default_watch.ElapsedMilliseconds} ms\n");

        parallel_watch.Start();
        dijkstra.DijkstraParallel(graph, startVertex);
        parallel_watch.Stop();
        Console.WriteLine($"Dijkstra parallel execution time with {threads} threads: {parallel_watch.ElapsedMilliseconds} ms\n");

        double acceleration = dijkstra.Acceleration(default_watch, parallel_watch);
        Console.WriteLine($"\nAcceleration = {acceleration.ToString("f2")}");
        double dijkstra_efficiency = dijkstra.Efficiency(acceleration, threads);
        Console.WriteLine($"Efficiency = {dijkstra_efficiency.ToString("f2")}%");
    }
}
