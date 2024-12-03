// mull_it_over.cu

#include <iostream>
#include <string>
#include <regex>
#include <vector>
#include <numeric>
#include <cuda_runtime.h>

// Kernel to compute products of X and Y arrays
__global__ void compute_products(int* x, int* y, int* products, int n) {
    int idx = blockIdx.x * blockDim.x + threadIdx.x;
    if (idx < n) {
        products[idx] = x[idx] * y[idx];
    }
}

int main() {
    // Read input from standard input
    std::string input((std::istreambuf_iterator<char>(std::cin)), std::istreambuf_iterator<char>());

    // Define regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
    std::regex pattern(R"(mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\))");

    // Find all matches
    std::vector<std::smatch> matches;
    auto words_begin = std::sregex_iterator(input.begin(), input.end(), pattern);
    auto words_end = std::sregex_iterator();

    for (std::sregex_iterator i = words_begin; i != words_end; ++i) {
        matches.push_back(*i);
    }

    // Vectors to store X and Y values for Part One
    std::vector<int> x_values;
    std::vector<int> y_values;

    // Part Two variables
    bool mulEnabled = true;
    std::vector<int> x_enabled;
    std::vector<int> y_enabled;

    // Process matches
    for (const auto& match : matches) {
        std::string value = match.str();
        if (value == "do()") {
            mulEnabled = true;
        } else if (value == "don't()") {
            mulEnabled = false;
        } else if (value.substr(0, 3) == "mul") {
            int x = std::stoi(match[1]);
            int y = std::stoi(match[2]);
            x_values.push_back(x);
            y_values.push_back(y);
            if (mulEnabled) {
                x_enabled.push_back(x);
                y_enabled.push_back(y);
            }
        }
    }

    // Part One: Compute total sum of all products
    int n = x_values.size();
    int* d_x;
    int* d_y;
    int* d_products;
    int* products = new int[n];

    cudaMalloc((void**)&d_x, n * sizeof(int));
    cudaMalloc((void**)&d_y, n * sizeof(int));
    cudaMalloc((void**)&d_products, n * sizeof(int));

    cudaMemcpy(d_x, x_values.data(), n * sizeof(int), cudaMemcpyHostToDevice);
    cudaMemcpy(d_y, y_values.data(), n * sizeof(int), cudaMemcpyHostToDevice);

    int blockSize = 256;
    int gridSize = (n + blockSize - 1) / blockSize;
    compute_products<<<gridSize, blockSize>>>(d_x, d_y, d_products, n);

    cudaMemcpy(products, d_products, n * sizeof(int), cudaMemcpyDeviceToHost);

    int partOneTotal = std::accumulate(products, products + n, 0);

    // Part Two: Compute total sum of enabled products
    int m = x_enabled.size();
    int* d_x_enabled;
    int* d_y_enabled;
    int* d_products_enabled;
    int* products_enabled = new int[m];

    if (m > 0) {
        cudaMalloc((void**)&d_x_enabled, m * sizeof(int));
        cudaMalloc((void**)&d_y_enabled, m * sizeof(int));
        cudaMalloc((void**)&d_products_enabled, m * sizeof(int));

        cudaMemcpy(d_x_enabled, x_enabled.data(), m * sizeof(int), cudaMemcpyHostToDevice);
        cudaMemcpy(d_y_enabled, y_enabled.data(), m * sizeof(int), cudaMemcpyHostToDevice);

        int gridSizeEnabled = (m + blockSize - 1) / blockSize;
        compute_products<<<gridSizeEnabled, blockSize>>>(d_x_enabled, d_y_enabled, d_products_enabled, m);

        cudaMemcpy(products_enabled, d_products_enabled, m * sizeof(int), cudaMemcpyDeviceToHost);
    }

    int partTwoTotal = std::accumulate(products_enabled, products_enabled + m, 0);

    // Free device memory
    cudaFree(d_x);
    cudaFree(d_y);
    cudaFree(d_products);
    if (m > 0) {
        cudaFree(d_x_enabled);
        cudaFree(d_y_enabled);
        cudaFree(d_products_enabled);
    }

    // Free host memory
    delete[] products;
    delete[] products_enabled;

    // Output the results
    std::cout << partOneTotal << std::endl;
    std::cout << partTwoTotal << std::endl;

    return 0;
}