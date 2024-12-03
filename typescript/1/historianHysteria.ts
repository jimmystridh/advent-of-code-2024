import { readFileSync } from 'fs';

interface InputData {
  leftList: number[];
  rightList: number[];
}

function main(): void {
  // Read input data
  const { leftList, rightList } = readInput();

  // Part One: Calculate Total Distance
  const totalDistance = calculateTotalDistance(leftList, rightList);
  console.log(`Total distance: ${totalDistance}`);

  // Part Two: Calculate Similarity Score
  const similarityScore = calculateSimilarityScore(leftList, rightList);
  console.log(`Similarity score: ${similarityScore}`);
}

function readInput(): InputData {
  const leftList: number[] = [];
  const rightList: number[] = [];

  const input = readFileSync(0, 'utf-8'); // Read from stdin
  const lines = input.trim().split('\n');

  for (const line of lines) {
    if (line.trim() === '') continue; // Skip empty lines

    const tokens = line.trim().split(/\s+/);
    if (tokens.length !== 2) {
      throw new Error(`Invalid input line: ${line}`);
    }

    const leftNum = parseInt(tokens[0], 10);
    const rightNum = parseInt(tokens[1], 10);

    if (isNaN(leftNum) || isNaN(rightNum)) {
      throw new Error(`Invalid numbers in line: ${line}`);
    }

    leftList.push(leftNum);
    rightList.push(rightNum);
  }

  return { leftList, rightList };
}

function calculateTotalDistance(leftList: number[], rightList: number[]): number {
  const sortedLeft = [...leftList].sort((a, b) => a - b);
  const sortedRight = [...rightList].sort((a, b) => a - b);

  const distances = sortedLeft.map((value, index) => Math.abs(value - sortedRight[index]));
  const totalDistance = distances.reduce((acc, val) => acc + val, 0);

  return totalDistance;
}

function calculateSimilarityScore(leftList: number[], rightList: number[]): number {
  const rightCounts = countOccurrences(rightList);

  const similarityScore = leftList.reduce((acc, num) => {
    const count = rightCounts.get(num) ?? 0;
    return acc + num * count;
  }, 0);

  return similarityScore;
}

function countOccurrences(numbers: number[]): Map<number, number> {
  const counts = new Map<number, number>();

  for (const num of numbers) {
    counts.set(num, (counts.get(num) ?? 0) + 1);
  }

  return counts;
}

// Execute the main function
main();