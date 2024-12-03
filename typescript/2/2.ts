import * as readline from 'readline';

// Define types for clarity and type safety
type Level = number;
type Report = ReadonlyArray<Level>;

// Function to check if a report is safe according to the rules
const isSafeReport = (levels: Report): boolean => {
    if (levels.length < 2) return false; // Not enough levels to compare

    const diffs = levels.slice(1).map((level, index) => level - levels[index]);

    const allIncreasing = diffs.every((d) => d > 0);
    const allDecreasing = diffs.every((d) => d < 0);
    const diffsValid = diffs.every((d) => Math.abs(d) >= 1 && Math.abs(d) <= 3);

    return diffsValid && (allIncreasing || allDecreasing);
};

const main = async () => {
    const reports: Report[] = [];

    // Create a readline interface for asynchronous input
    const rl = readline.createInterface({
        input: process.stdin,
        output: process.stdout,
        terminal: false,
    });

    // Read input lines and parse them into reports
    for await (const line of rl) {
        const trimmedLine = line.trim();
        if (trimmedLine !== '') {
            const levels = trimmedLine.split(/\s+/).map(Number);
            reports.push(levels);
        }
    }

    // Use functional programming techniques to compute safe report counts
    const [safeCountPart1, safeCountPart2] = reports.reduce(
        ([count1, count2], levels) => {
            const isSafeOriginal = isSafeReport(levels);
            const isSafeWithRemoval = isSafeOriginal
                ? true
                : levels.some((_, index) =>
                      isSafeReport(levels.filter((_, i) => i !== index))
                  );

            return [
                count1 + (isSafeOriginal ? 1 : 0),
                count2 + (isSafeWithRemoval ? 1 : 0),
            ];
        },
        [0, 0]
    );

    // Output the results
    console.log(`Part One: ${safeCountPart1}`);
    console.log(`Part Two: ${safeCountPart2}`);
};

main();