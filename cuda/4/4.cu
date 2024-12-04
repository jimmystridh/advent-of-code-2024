// Include necessary headers
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Define grid dimensions (maximum expected sizes)
#define MAX_ROWS 1024
#define MAX_COLS 1024

// Define the target word for Part One
#define WORD "XMAS"
#define WORD_LENGTH 4

// Device function to check if a position is within grid bounds
__device__ bool isValid(int x, int y, int numRows, int numCols) {
    return (x >= 0 && x < numRows && y >= 0 && y < numCols);
}

// Kernel for Part One
__global__ void countXMAS(char *grid, int numRows, int numCols, int *count) {
    int idx = blockIdx.x * blockDim.x + threadIdx.x;
    int totalCells = numRows * numCols;

    if (idx >= totalCells) return;

    int x = idx / numCols;
    int y = idx % numCols;

    // Define directions: N, NE, E, SE, S, SW, W, NW
    int directions[8][2] = { {-1, 0}, {-1, 1}, {0, 1}, {1, 1},
                             {1, 0}, {1, -1}, {0, -1}, {-1, -1} };

    for (int dir = 0; dir < 8; dir++) {
        bool match = true;
        int dx = directions[dir][0];
        int dy = directions[dir][1];
        int xi = x, yi = y;

        for (int i = 0; i < WORD_LENGTH; i++) {
            if (!isValid(xi, yi, numRows, numCols) || grid[xi * numCols + yi] != WORD[i]) {
                match = false;
                break;
            }
            xi += dx;
            yi += dy;
        }
        if (match) {
            atomicAdd(count, 1);
        }
    }
}

// Kernel for Part Two
__global__ void countXMAS_X(char *grid, int numRows, int numCols, int *count) {
    int idx = blockIdx.x * blockDim.x + threadIdx.x;
    int totalCells = (numRows - 2) * (numCols - 2); // Adjust for 3x3 pattern

    if (idx >= totalCells) return;

    int x = idx / (numCols - 2);
    int y = idx % (numCols - 2);

    // Positions in the X shape
    int positions[5][2] = {
        {x, y},         // Top-left or top-right
        {x + 1, y + 1}, // Center
        {x + 2, y + 2}, // Bottom-right or bottom-left
        {x + 2, y},     // Bottom-left or bottom-right
        {x, y + 2}      // Top-right or top-left
    };

    // Extract letters at the positions
    char diag1[3]; // Diagonal from top-left to bottom-right
    char diag2[3]; // Diagonal from top-right to bottom-left

    diag1[0] = grid[positions[0][0] * numCols + positions[0][1]];
    diag1[1] = grid[positions[1][0] * numCols + positions[1][1]];
    diag1[2] = grid[positions[2][0] * numCols + positions[2][1]];

    diag2[0] = grid[positions[4][0] * numCols + positions[4][1]];
    diag2[1] = grid[positions[1][0] * numCols + positions[1][1]];
    diag2[2] = grid[positions[3][0] * numCols + positions[3][1]];

    // Valid patterns (MAS or SAM)
    const char *patterns[] = { "MAS", "SAM" };

    bool matchDiag1 = false;
    bool matchDiag2 = false;

    // Check diag1
    for (int i = 0; i < 2; i++) {
        if ((diag1[0] == patterns[i][0] && diag1[1] == patterns[i][1] && diag1[2] == patterns[i][2]) ||
            (diag1[0] == patterns[i][2] && diag1[1] == patterns[i][1] && diag1[2] == patterns[i][0])) {
            matchDiag1 = true;
            break;
        }
    }

    // Check diag2
    for (int i = 0; i < 2; i++) {
        if ((diag2[0] == patterns[i][0] && diag2[1] == patterns[i][1] && diag2[2] == patterns[i][2]) ||
            (diag2[0] == patterns[i][2] && diag2[1] == patterns[i][1] && diag2[2] == patterns[i][0])) {
            matchDiag2 = true;
            break;
        }
    }

    if (matchDiag1 && matchDiag2 && diag1[1] == 'A' && diag2[1] == 'A') {
        atomicAdd(count, 1);
    }
}

// Host code
int main() {
    // Read the grid from input
    char hostGrid[MAX_ROWS * MAX_COLS];
    int numRows = 0, numCols = 0;

    char line[MAX_COLS + 2]; // +2 for newline and null terminator
    while (fgets(line, sizeof(line), stdin)) {
        size_t len = strlen(line);
        // Remove newline character
        if (line[len - 1] == '\n') {
            line[len - 1] = '\0';
            len--;
        }
        if (numCols == 0) {
            numCols = len;
        } else if (len != numCols) {
            fprintf(stderr, "Error: Inconsistent row lengths.\n");
            return 1;
        }
        memcpy(&hostGrid[numRows * numCols], line, numCols);
        numRows++;
    }

    // Allocate device memory
    char *deviceGrid;
    int *deviceCountPart1;
    int *deviceCountPart2;
    cudaMalloc((void **)&deviceGrid, numRows * numCols * sizeof(char));
    cudaMalloc((void **)&deviceCountPart1, sizeof(int));
    cudaMalloc((void **)&deviceCountPart2, sizeof(int));

    // Copy grid to device
    cudaMemcpy(deviceGrid, hostGrid, numRows * numCols * sizeof(char), cudaMemcpyHostToDevice);
    cudaMemset(deviceCountPart1, 0, sizeof(int));
    cudaMemset(deviceCountPart2, 0, sizeof(int));

    // Define block and grid sizes
    int totalCells = numRows * numCols;
    int blockSize = 256;
    int gridSize = (totalCells + blockSize - 1) / blockSize;

    // Launch kernel for Part One
    countXMAS<<<gridSize, blockSize>>>(deviceGrid, numRows, numCols, deviceCountPart1);

    // Adjust total cells for 3x3 grid in Part Two
    int totalCellsPart2 = (numRows - 2) * (numCols - 2);
    int gridSizePart2 = (totalCellsPart2 + blockSize - 1) / blockSize;

    // Launch kernel for Part Two
    countXMAS_X<<<gridSizePart2, blockSize>>>(deviceGrid, numRows, numCols, deviceCountPart2);

    // Copy results back to host
    int countPart1 = 0, countPart2 = 0;
    cudaMemcpy(&countPart1, deviceCountPart1, sizeof(int), cudaMemcpyDeviceToHost);
    cudaMemcpy(&countPart2, deviceCountPart2, sizeof(int), cudaMemcpyDeviceToHost);

    // Free device memory
    cudaFree(deviceGrid);
    cudaFree(deviceCountPart1);
    cudaFree(deviceCountPart2);

    // Output the results
    printf("Part One: %d\n", countPart1);
    printf("Part Two: %d\n", countPart2);

    return 0;
}