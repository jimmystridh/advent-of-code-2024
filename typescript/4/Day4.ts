// Import necessary modules
import * as readline from 'readline';

// Define types for clarity
type Grid = string[][];
type Direction = [number, number];

// Create an interface for pattern matching functions
interface PatternMatcher {
  (grid: Grid): number;
}

// Read the grid from standard input
function readGrid(): Promise<Grid> {
  return new Promise((resolve) => {
    const grid: Grid = [];
    const rl = readline.createInterface({
      input: process.stdin,
      output: process.stdout,
      terminal: false,
    });

    rl.on('line', (line: string) => {
      if (line.trim() !== '') {
        grid.push(line.trim().split(''));
      }
    });

    rl.on('close', () => {
      resolve(grid);
    });
  });
}

// Part One: Count occurrences of "XMAS" in any direction
const countXMAS: PatternMatcher = (grid) => {
  const numRows = grid.length;
  const numCols = grid[0].length;
  const word = 'XMAS';
  const directions: Direction[] = [
    [-1, 0],   // North
    [-1, 1],   // Northeast
    [0, 1],    // East
    [1, 1],    // Southeast
    [1, 0],    // South
    [1, -1],   // Southwest
    [0, -1],   // West
    [-1, -1],  // Northwest
  ];

  const isValid = (x: number, y: number): boolean =>
    x >= 0 && x < numRows && y >= 0 && y < numCols;

  const checkWord = (x: number, y: number, dx: number, dy: number): boolean => {
    const positions = Array.from({ length: word.length }, (_, i) => [
      x + i * dx,
      y + i * dy,
    ]);
    if (positions.every(([xi, yi]) => isValid(xi, yi))) {
      const letters = positions.map(([xi, yi]) => grid[xi][yi]);
      return letters.join('') === word;
    }
    return false;
  };

  const count = Array.from({ length: numRows }, (_, x) => x)
    .flatMap((x) =>
      Array.from({ length: numCols }, (_, y) =>
        directions.filter(([dx, dy]) => checkWord(x, y, dx, dy)).length
      )
    )
    .reduce((a, b) => a + b, 0);

  return count;
};

// Part Two: Count occurrences of "X-MAS" patterns
const countXMAS_X: PatternMatcher = (grid) => {
  const numRows = grid.length;
  const numCols = grid[0].length;
  const patterns = new Set(['MAS', 'SAM']);
  let count = 0;

  const isValid = (x: number, y: number): boolean =>
    x >= 0 && x < numRows && y >= 0 && y < numCols;

  for (let x = 0; x <= numRows - 3; x++) {
    for (let y = 0; y <= numCols - 3; y++) {
      const diag1 = [
        [x, y],
        [x + 1, y + 1],
        [x + 2, y + 2],
      ];
      const diag2 = [
        [x, y + 2],
        [x + 1, y + 1],
        [x + 2, y],
      ];
      if (
        diag1.concat(diag2).every(([xi, yi]) => isValid(xi, yi)) &&
        grid[x + 1][y + 1] === 'A'
      ) {
        const letters1 = diag1.map(([xi, yi]) => grid[xi][yi]);
        const letters2 = diag2.map(([xi, yi]) => grid[xi][yi]);
        const word1 = letters1.join('');
        const word2 = letters2.join('');
        const word1Rev = letters1.slice().reverse().join('');
        const word2Rev = letters2.slice().reverse().join('');
        if (
          (patterns.has(word1) || patterns.has(word1Rev)) &&
          (patterns.has(word2) || patterns.has(word2Rev))
        ) {
          count += 1;
        }
      }
    }
  }
  return count;
};

// Main function to execute the program
async function main() {
  const grid = await readGrid();
  const partOneCount = countXMAS(grid);
  const partTwoCount = countXMAS_X(grid);
  console.log(`Part One: ${partOneCount}`);
  console.log(`Part Two: ${partTwoCount}`);
}

main();