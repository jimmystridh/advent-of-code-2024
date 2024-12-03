using System.Text.RegularExpressions;

// Read the entire input from standard input
string input = Console.In.ReadToEnd();

// Define the regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
string pattern = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)";

// Find all matches in the input string
var matches = Regex.Matches(input, pattern)
    .Cast<Match>()
    .ToList();

// Part One: Sum of all valid 'mul' instructions
var partOneTotal = matches
    .Where(m => m.Value.StartsWith("mul"))
    .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
    .Sum();

// Part Two: Sum of enabled 'mul' instructions considering 'do()' and 'don't()'
var partTwoTotal = matches
    .Aggregate(new { MulEnabled = true, Total = 0 }, (acc, m) =>
    {
        var value = m.Value;
        if (value == "do()")
        {
            return new { MulEnabled = true, acc.Total };
        }
        else if (value == "don't()")
        {
            return new { MulEnabled = false, acc.Total };
        }
        else if (value.StartsWith("mul") && acc.MulEnabled)
        {
            int x = int.Parse(m.Groups[1].Value);
            int y = int.Parse(m.Groups[2].Value);
            return new { acc.MulEnabled, Total = acc.Total + x * y };
        }
        else
        {
            return acc;
        }
    }).Total;

// Output the results
Console.WriteLine(partOneTotal);
Console.WriteLine(partTwoTotal);
