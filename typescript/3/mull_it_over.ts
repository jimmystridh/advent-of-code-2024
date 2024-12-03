import * as fs from 'fs';

// Read the input from standard input
const input = fs.readFileSync(0, 'utf-8');

// Define types for instructions
type MulInstruction = { type: 'mul'; x: number; y: number };
type DoInstruction = { type: 'do' };
type DontInstruction = { type: "don't" };
type Instruction = MulInstruction | DoInstruction | DontInstruction;

// Define a regular expression to match 'mul(X,Y)', 'do()', and 'don't()'
const pattern = /mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)/g;

// Parse the input into instructions
const instructions: Instruction[] = Array.from(input.matchAll(pattern)).map((match) => {
  const [fullMatch, xStr, yStr] = match;

  if (fullMatch === 'do()') {
    return { type: 'do' } as DoInstruction;
  } else if (fullMatch === "don't()") {
    return { type: "don't" } as DontInstruction;
  } else if (fullMatch.startsWith('mul')) {
    return {
      type: 'mul',
      x: parseInt(xStr, 10),
      y: parseInt(yStr, 10),
    } as MulInstruction;
  } else {
    // This case should not occur due to the regex pattern
    throw new Error('Unrecognized instruction');
  }
});

// Part One: Sum of all valid 'mul' instructions
const partOneTotal = instructions
  .filter((inst): inst is MulInstruction => inst.type === 'mul')
  .map((inst) => inst.x * inst.y)
  .reduce((sum, value) => sum + value, 0);

// Part Two: Sum of enabled 'mul' instructions considering 'do()' and 'don't()'
interface Accumulator {
  mulEnabled: boolean;
  total: number;
}

const initialAcc: Accumulator = { mulEnabled: true, total: 0 };

const finalAcc = instructions.reduce((acc, inst) => {
  switch (inst.type) {
    case 'do':
      return { ...acc, mulEnabled: true };
    case "don't":
      return { ...acc, mulEnabled: false };
    case 'mul':
      if (acc.mulEnabled) {
        return { ...acc, total: acc.total + inst.x * inst.y };
      } else {
        return acc;
      }
    default:
      return acc;
  }
}, initialAcc);

// Output the results
console.log(partOneTotal);
console.log(finalAcc.total);