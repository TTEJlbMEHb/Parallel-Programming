
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>

cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size);

__global__
void vectorAdd(float* a, float* b, float* result, int size) {
    int i = blockIdx.x * blockDim.x + threadIdx.x;
    if (i < size) {
        result[i] = a[i] + b[i];
    }
}

int main() {
    const int size = 5;

    // Ініціалізація векторів на CPU
    float h_a[size] = { 1.0, 2.0, 3.0, 4.0, 5.0 };
    float h_b[size] = { 5.0, 4.0, 3.0, 2.0, 1.0 };
    float h_result[size];

    // Оголошення та алокація пам'яті на GPU
    float* d_a, * d_b, * d_result;
    cudaMalloc((void**)&d_a, size * sizeof(float));
    cudaMalloc((void**)&d_b, size * sizeof(float));
    cudaMalloc((void**)&d_result, size * sizeof(float));

    // Копіювання даних з CPU на GPU
    cudaMemcpy(d_a, h_a, size * sizeof(float), cudaMemcpyHostToDevice);
    cudaMemcpy(d_b, h_b, size * sizeof(float), cudaMemcpyHostToDevice);

    // Визначення конфігурації блоків та ниток
    int blockSize = 256;
    int numBlocks = (size + blockSize - 1) / blockSize;

    // Виклик ядра на GPU
    vectorAdd << <numBlocks, blockSize >> > (d_a, d_b, d_result, size);

    // Копіювання результату з GPU на CPU
    cudaMemcpy(h_result, d_result, size * sizeof(float), cudaMemcpyDeviceToHost);

    // Виведення результату
    for (int i = 0; i < size; ++i) {
        std::cout << h_result[i] << " ";
    }
    std::cout << std::endl;

    // Звільнення пам'яті на GPU
    cudaFree(d_a);
    cudaFree(d_b);
    cudaFree(d_result);

    return 0;
}
