using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

class Matrix
{
    int Rows { get; set; }
    int Columns { get; set; }

    int threads;
    ParallelOptions parallelOptions = new ParallelOptions();
    Random random = new Random();

    public Matrix(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;

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

    public double[][] RandomMatrix()
    {
        double[][] matrix = new double[Rows][];
        Random random = new Random();

        for (int i = 0; i < Rows; i++)
        {
            matrix[i] = new double[Columns];
            for (int j = 0; j < Columns; j++)
            {
                matrix[i][j] = random.Next(-9, 11);
            }
        }
        return matrix;
    }

    public bool isMatrixValid(double[][] matrix)
    {
        if (matrix == null)
        {
            return false;
        }
        else if (Rows != Columns - 1)
        {
            return false;
        }
        return true;
    }

    public double[] Gauss(double[][] matrix)
    {
        if (isMatrixValid(matrix))
        {
            int n = matrix.Length;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                int maxRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[maxRow][i]))
                    {
                        maxRow = j;
                    }
                }

                double[] temp = matrix[i];
                matrix[i] = matrix[maxRow];
                matrix[maxRow] = temp;

                double pivot = matrix[i][i];
                if (pivot == 0)
                {
                    throw new Exception("The SLAR has many solutions or no solutions.");
                }

                for (int j = i; j < n + 1; j++)
                {
                    matrix[i][j] /= pivot;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = matrix[j][i];   
                        for (int k = i; k < n + 1; k++)
                        {
                            matrix[j][k] -= factor * matrix[i][k];
                        }
                    }
                }
                result[i] = matrix[i][n];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += matrix[i][j] * result[j];
                }
                result[i] = (matrix[i][n] - sum) / matrix[i][i];
            }

            return result;
        }
        else
        {
            throw new Exception("Matrix is invlalid");
        }
    }

    public double[] GaussParallel(double[][] matrix)
    {
        if (isMatrixValid(matrix))
        {
            int n = matrix.Length;
            double[] result = new double[n];

            Parallel.For(0, n, parallelOptions, i =>
            {
                int maxRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[maxRow][i]))
                    {
                        maxRow = j;
                    }
                }

                double[] temp = matrix[i];
                matrix[i] = matrix[maxRow];
                matrix[maxRow] = temp;

                double pivot = matrix[i][i];
                if (pivot == 0)
                {
                    throw new Exception("The SLAR has many solutions or no solutions.");
                }
                for (int j = i; j < n + 1; j++)
                {
                    matrix[i][j] /= pivot;
                }
                Parallel.For(0, n, parallelOptions, j =>
                {
                    if (j != i)
                    {
                        double factor = matrix[j][i];
                        for (int k = i; k < n + 1; k++)
                        {
                            matrix[j][k] -= factor * matrix[i][k];
                        }
                    }
                });
                result[i] = matrix[i][n];
            });

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += matrix[i][j] * result[j];
                }
                result[i] = (matrix[i][n] - sum) / matrix[i][i];
            }

            return result;
        }
        else
        {
            throw new Exception("Matrix is invlalid");
        }
    }

    public bool Converged(double[] X, double[] prevX, double tolerance)
    {
        for (int i = 0; i < X.Length; i++)
        {
            if (Math.Abs(X[i] - prevX[i]) >= tolerance)
            {
                return false;
            }
        }
        return true;
    }

    public double[] Jacobi(double[][] matrix, double tolerance = 1e-6, int maxIterations = 100)
    {
        if (isMatrixValid(matrix))
        {
            int n = matrix.GetLength(0);
            double[] X = new double[n];
            double[] prevX = new double[n];

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (int i = 0; i < n; i++)
                {
                    double sigma = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            sigma += matrix[i][j] * prevX[j];
                        }
                    }
                    X[i] = (matrix[i][n] - sigma) / matrix[i][i];
                }

                if (Converged(X, prevX, tolerance))
                {
                    return X;
                }
                for (int i = 0; i < n; i++)
                {
                    prevX[i] = X[i];
                }
            }

            throw new Exception("Jacobi method did not converge within the specified number of iterations.");
        }
        else
        {
            throw new Exception("Matrix is invalid");
        }
    }


    public double[] JacobiParallel(double[][] matrix, double tolerance = 1e-6, int maxIterations = 100)
    {
        if (isMatrixValid(matrix))
        {
            int n = matrix.Length;
            double[] X = new double[n];
            double[] prevX = new double[n];

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                Parallel.For(0, n, parallelOptions, i =>
                {
                    double sigma = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            sigma += matrix[i][j] * prevX[j];
                        }
                    }
                    X[i] = (matrix[i][n] - sigma) / matrix[i][i];
                });

                if (Converged(X, prevX, tolerance))
                {
                    return X;
                }
                for (int i = 0; i < n; i++)
                {
                    prevX[i] = X[i];
                }
            }

            throw new Exception("Jacobi method did not converge within the specified number of iterations.");
        }
        else
        {
            throw new Exception("Matrix is invlalid");
        }
    }

    public void PrintSolutions(double[] solution)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            Console.WriteLine($"x{i + 1} = {solution[i].ToString("F2")}");
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
    static void Main(string[] args)
    {
        int rows = 1000;
        int columns = 1001;
        int threads = 6;

        Matrix matrix = new Matrix(rows, columns);
        matrix.Threads(threads);

        double[][] randomMatrix = matrix.RandomMatrix();

        Console.WriteLine($"--------------------------\n");
        Console.WriteLine($"Gauss algorithm for {rows}x{columns} matrix:");

        Stopwatch gaussStop = new Stopwatch();
        gaussStop.Start();
        double[] solution = matrix.Gauss(randomMatrix);
        gaussStop.Stop();
        Console.WriteLine("Time: " + gaussStop.ElapsedMilliseconds + " ms");

        //Console.WriteLine("\nThe SLAR solutions:");
        //matrix.PrintSolutions(solution);

        Console.WriteLine($"\n");
        Console.WriteLine($"Parallel Gauss with {threads} threads:");

        Stopwatch gaussParallelStop = new Stopwatch();
        gaussParallelStop.Start();
        double[] solution1 = matrix.GaussParallel(randomMatrix);
        gaussParallelStop.Stop();
        Console.WriteLine("Time: " + gaussParallelStop.ElapsedMilliseconds + " ms");

        //Console.WriteLine("\nThe SLAR solutions:");
        //matrix.PrintSolutions(solution1);

        double gauss_boost = matrix.Acceleration(gaussStop, gaussParallelStop);
        Console.WriteLine($"\nAcceleration = {gauss_boost.ToString("f2")}");
        double gauss_efficiency = matrix.Efficiency(gauss_boost, threads);
        Console.WriteLine($"Efficiency = {gauss_efficiency.ToString("f2")}%");        

        var random = new Random();
        columns -= 1;
        var A = DenseMatrix.Build.Dense(rows, columns, (i, j) => random.Next(-9, 11));
        var b = DenseVector.Build.Dense(columns, i => random.Next(-9, 11));

        Console.WriteLine($"\n--------------------------\n");
        Console.WriteLine($"Lib algorithm:");

        Stopwatch libStop = new Stopwatch();
        libStop.Start();
        var x = A.Solve(b);
        libStop.Stop();
        Console.WriteLine("Time: " + libStop.ElapsedMilliseconds + " ms");

        Console.WriteLine($"\n--------------------------\n");
        int jacobiRows = 15000;
        int jacobiColumns = 15001;
        Matrix matrix_ = new Matrix(jacobiRows, jacobiColumns);
        double[][] randomMatrix_ = matrix_.RandomMatrix();
        Console.WriteLine($"Jacobi algorithm for {jacobiRows}x{jacobiColumns} matrix:");

        Stopwatch jacobiStop = new Stopwatch();
        jacobiStop.Start();
        double[] solution_ = matrix.Jacobi(randomMatrix_);
        jacobiStop.Stop();
        Console.WriteLine("Time: " + jacobiStop.ElapsedMilliseconds + " ms");

        //Console.WriteLine("\nThe SLAR solutions:");
        //matrix.PrintSolutions(solution_);

        Console.WriteLine($"\n");
        Console.WriteLine($"Jacobi Parallel algorithm with {threads} threads:");

        Stopwatch jacobiParallelStop = new Stopwatch();
        jacobiParallelStop.Start();
        double[] solution__ = matrix_.JacobiParallel(randomMatrix_);
        jacobiParallelStop.Stop();
        Console.WriteLine("Time: " + jacobiParallelStop.ElapsedMilliseconds + " ms");
        double jacobi_boost = matrix.Acceleration(jacobiStop, jacobiParallelStop);
        Console.WriteLine($"\nAcceleration = {jacobi_boost.ToString("f2")}");
        double jacobi_efficiency = matrix.Efficiency(jacobi_boost, threads);
        Console.WriteLine($"Efficiency = {jacobi_efficiency.ToString("f2")}%");

        //Console.WriteLine("\nThe SLAR solutions:");
        //matrix.PrintSolutions(solution__);
        Console.WriteLine($"\n--------------------------\n");
    }
}
