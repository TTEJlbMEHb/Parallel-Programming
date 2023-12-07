#include <iostream>
#include <cstdlib>
#include <ctime>
#include <mpi.h>

using namespace std;

const int MATRIX_SIZE = 800;

void generateRandomMatrix(int matrix[MATRIX_SIZE][MATRIX_SIZE]) 
{
    for (int i = 0; i < MATRIX_SIZE; ++i) 
    {
        for (int j = 0; j < MATRIX_SIZE; ++j) 
        {
            matrix[i][j] = rand() % 10;
        }
    }
}

void printMatrix(int matrix[MATRIX_SIZE][MATRIX_SIZE]) 
{
    for (int i = 0; i < MATRIX_SIZE; ++i) 
    {
        for (int j = 0; j < MATRIX_SIZE; ++j) 
        {
            cout << matrix[i][j] << " ";
        }
        cout << endl;
    }
}

void multiplyMatrices(int A[MATRIX_SIZE][MATRIX_SIZE], int B[MATRIX_SIZE][MATRIX_SIZE], 
    int C[MATRIX_SIZE][MATRIX_SIZE], int start_row, int end_row) 
{
    for (int i = start_row; i < end_row; ++i) 
    {
        for (int j = 0; j < MATRIX_SIZE; ++j) 
        {
            C[i][j] = 0;
            for (int k = 0; k < MATRIX_SIZE; ++k) 
            {
                C[i][j] += A[i][k] * B[k][j];
            }
        }
    }
}

int main(int argc, char** argv) 
{
    srand(time(NULL));

    MPI_Init(&argc, &argv);

    int world_rank, world_size;

    MPI_Comm_rank(MPI_COMM_WORLD, &world_rank);
    MPI_Comm_size(MPI_COMM_WORLD, &world_size);

    int A[MATRIX_SIZE][MATRIX_SIZE];
    int B[MATRIX_SIZE][MATRIX_SIZE];
    int C[MATRIX_SIZE][MATRIX_SIZE];

    if (world_rank == 0) 
    {
        generateRandomMatrix(A);
        generateRandomMatrix(B);
    }

    MPI_Bcast(A, MATRIX_SIZE * MATRIX_SIZE, MPI_INT, 0, MPI_COMM_WORLD);
    MPI_Bcast(B, MATRIX_SIZE * MATRIX_SIZE, MPI_INT, 0, MPI_COMM_WORLD);

    int rows_per_process = MATRIX_SIZE / world_size;
    int extra_rows = MATRIX_SIZE % world_size;

    int start_row, end_row;

    if (world_rank == 0) 
    {
        start_row = 0;
        end_row = rows_per_process + extra_rows;
    } 
    else 
    {
        start_row = world_rank * rows_per_process + extra_rows;
        end_row = start_row + rows_per_process;
    }

    double start_time = MPI_Wtime();

    multiplyMatrices(A, B, C, start_row, end_row);
    MPI_Gather(C + start_row, rows_per_process * MATRIX_SIZE, MPI_INT, C, rows_per_process * MATRIX_SIZE, MPI_INT, 0, MPI_COMM_WORLD);

    double end_time = MPI_Wtime();

    if (world_rank == 0) 
    {
        cout << "MPI multiplication " << MATRIX_SIZE << "x" << MATRIX_SIZE << " matrices:" << endl;
        cout << "Execution time: " << (end_time - start_time) * 1000 << " ms" << endl;
        cout << "Processes: " << world_size << endl;
    }

    MPI_Finalize();

    return 0;
}
