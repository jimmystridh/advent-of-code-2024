var reports = new List<int[]>();

// Read input reports
string line;
while ((line = Console.ReadLine()!) != null)
{
    if (!string.IsNullOrWhiteSpace(line))
    {
        var levels = line.Trim()
                         .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(int.Parse)
                         .ToArray();
        reports.Add(levels);
    }
}

int safeCountPart1 = 0;
int safeCountPart2 = 0;

foreach (var levels in reports)
{
    if (IsSafeReport(levels))
    {
        // Report is safe under original rules (Part One)
        safeCountPart1++;
        safeCountPart2++;
    }
    else
    {
        // For Part Two, try removing one level at a time
        bool foundSafe = false;
        for (int i = 0; i < levels.Length; i++)
        {
            var newLevels = levels.Where((_, index) => index != i).ToArray();
            if (IsSafeReport(newLevels))
            {
                foundSafe = true;
                break;
            }
        }
        if (foundSafe)
        {
            safeCountPart2++;
        }
    }
}

Console.WriteLine($"Part One: {safeCountPart1}");
Console.WriteLine($"Part Two: {safeCountPart2}");


/// <summary>
/// Determines if a report is safe based on the levels.
/// A report is safe if:
/// - The levels are either all increasing or all decreasing.
/// - Any two adjacent levels differ by at least one and at most three.
/// </summary>
static bool IsSafeReport(int[] levels)
{
    if (levels.Length < 2)
        return false; // Not enough levels to compare

    var diffs = new int[levels.Length - 1];
    for (int i = 0; i < levels.Length - 1; i++)
    {
        diffs[i] = levels[i + 1] - levels[i];
    }

    bool allPositive = diffs.All(d => d > 0);
    bool allNegative = diffs.All(d => d < 0);
    bool diffsValid = diffs.All(d => Math.Abs(d) >= 1 && Math.Abs(d) <= 3);

    return diffsValid && (allPositive || allNegative);
}
