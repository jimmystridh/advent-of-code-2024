#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include <algorithm>
#include <cuda_runtime.h>

__device__ bool isSafeReport(const int* levels, int length) {
    if (length < 2) return false;

    bool allIncreasing = true;
    bool allDecreasing = true;

    for (int i = 0; i < length - 1; ++i) {
        int diff = levels[i + 1] - levels[i];
        if (diff <= 0) allIncreasing = false;
        if (diff >= 0) allDecreasing = false;
        if (abs(diff) < 1 || abs(diff) > 3) return false;
    }

    return allIncreasing || allDecreasing;
}

__global__ void analyzeReports(const int* d_levels, const int* d_indices, const int* d_lengths, int numReports, int* d_safePart1, int* d_safePart2) {
    int idx = blockIdx.x * blockDim.x + threadIdx.x;
    if (idx >= numReports) return;

    const int* levels = d_levels + d_indices[idx];
    int length = d_lengths[idx];

    bool safePart1 = isSafeReport(levels, length);
    bool safePart2 = safePart1;

    if (!safePart1) {
        // Try removing one level at a time
        for (int i = 0; i < length; ++i) {
            // Create a new levels array without the i-th element
            int tempLength = length - 1;
            int tempLevels[100]; // Assuming max report length is less than 100
            int k = 0;
            for (int j = 0; j < length; ++j) {
                if (j != i) {
                    tempLevels[k++] = levels[j];
                }
            }
            if (isSafeReport(tempLevels, tempLength)) {
                safePart2 = true;
                break;
            }
        }
    }

    if (safePart1) atomicAdd(d_safePart1, 1);
    if (safePart2) atomicAdd(d_safePart2, 1);
}

int main() {
    std::vector<std::vector<int>> reports;
    std::string line;

    // Read input reports
    while (std::getline(std::cin, line)) {
        if (line.empty()) continue;

        std::istringstream iss(line);
        std::vector<int> levels;
        int num;
        while (iss >> num) {
            levels.push_back(num);
        }
        reports.push_back(levels);
    }

    int totalLevels = 0;
    for (const auto& report : reports) {
        totalLevels += report.size();
    }

    // Flatten levels and prepare indices
    std::vector<int> h_levels;
    std::vector<int> h_indices;
    std::vector<int> h_lengths;

    int index = 0;
    for (const auto& report : reports) {
        h_indices.push_back(index);
        h_lengths.push_back(report.size());
        h_levels.insert(h_levels.end(), report.begin(), report.end());
        index += report.size();
    }

    int numReports = reports.size();

    // Allocate device memory
    int* d_levels;
    int* d_indices;
    int* d_lengths;
    int* d_safePart1;
    int* d_safePart2;

    cudaMalloc(&d_levels, h_levels.size() * sizeof(int));
    cudaMalloc(&d_indices, h_indices.size() * sizeof(int));
    cudaMalloc(&d_lengths, h_lengths.size() * sizeof(int));
    cudaMalloc(&d_safePart1, sizeof(int));
    cudaMalloc(&d_safePart2, sizeof(int));

    // Copy data to device
    cudaMemcpy(d_levels, h_levels.data(), h_levels.size() * sizeof(int), cudaMemcpyHostToDevice);
    cudaMemcpy(d_indices, h_indices.data(), h_indices.size() * sizeof(int), cudaMemcpyHostToDevice);
    cudaMemcpy(d_lengths, h_lengths.data(), h_lengths.size() * sizeof(int), cudaMemcpyHostToDevice);
    cudaMemset(d_safePart1, 0, sizeof(int));
    cudaMemset(d_safePart2, 0, sizeof(int));

    // Launch kernel
    int threadsPerBlock = 256;
    int blocksPerGrid = (numReports + threadsPerBlock - 1) / threadsPerBlock;

    analyzeReports<<<blocksPerGrid, threadsPerBlock>>>(d_levels, d_indices, d_lengths, numReports, d_safePart1, d_safePart2);
    cudaDeviceSynchronize();

    // Copy results back to host
    int h_safePart1 = 0;
    int h_safePart2 = 0;

    cudaMemcpy(&h_safePart1, d_safePart1, sizeof(int), cudaMemcpyDeviceToHost);
    cudaMemcpy(&h_safePart2, d_safePart2, sizeof(int), cudaMemcpyDeviceToHost);

    // Free device memory
    cudaFree(d_levels);
    cudaFree(d_indices);
    cudaFree(d_lengths);
    cudaFree(d_safePart1);
    cudaFree(d_safePart2);

    // Output results
    std::cout << "Part One: " << h_safePart1 << std::endl;
    std::cout << "Part Two: " << h_safePart2 << std::endl;

    return 0;
}
