using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Read the grid from standard input
        var grid = ReadGrid();

        // Part One: Count occurrences of "XMAS"
        int partOneCount = CountXMAS(grid);
        Console.WriteLine($"Part One: {partOneCount}");

        // Part Two: Count occurrences of "X-MAS" patterns
        int partTwoCount = CountXMAS_X(grid);
        Console.WriteLine($"Part Two: {partTwoCount}");
    }

    // Function to read the grid from input
    static char[][] ReadGrid()
    {
        var lines = new List<string>();
        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            lines.Add(line);
        }
        return lines.Select(l => l.ToCharArray()).ToArray();
    }

    // Part One: Count all occurrences of "XMAS" in any direction
    static int CountXMAS(char[][] grid)
    {
        int numRows = grid.Length;
        int numCols = grid[0].Length;
        string word = "XMAS";
        var directions = new (int dx, int dy)[]
        {
            (-1, 0),    // North
            (-1, 1),    // Northeast
            (0, 1),     // East
            (1, 1),     // Southeast
            (1, 0),     // South
            (1, -1),    // Southwest
            (0, -1),    // West
            (-1, -1)    // Northwest
        };

        var count = (
            from x in Enumerable.Range(0, numRows)
            from y in Enumerable.Range(0, numCols)
            from dir in directions
            where IsMatch(grid, x, y, dir.dx, dir.dy, word)
            select 1
        ).Sum();

        return count;
    }

    // Helper function to check if "XMAS" matches starting at (x, y) in direction (dx, dy)
    static bool IsMatch(char[][] grid, int x, int y, int dx, int dy, string word)
    {
        int numRows = grid.Length;
        int numCols = grid[0].Length;

        for (int i = 0; i < word.Length; i++)
        {
            int xi = x + i * dx;
            int yi = y + i * dy;

            if (xi < 0 || xi >= numRows || yi < 0 || yi >= numCols)
                return false;

            if (grid[xi][yi] != word[i])
                return false;
        }
        return true;
    }

    // Part Two: Count all "X-MAS" patterns
    static int CountXMAS_X(char[][] grid)
    {
        int numRows = grid.Length;
        int numCols = grid[0].Length;
        var patterns = new HashSet<string> { "MAS", "SAM" };

        var count = (
            from x in Enumerable.Range(0, numRows - 2)
            from y in Enumerable.Range(0, numCols - 2)
            let diag1 = new[]
            {
                grid[x][y],
                grid[x+1][y+1],
                grid[x+2][y+2]
            }
            let diag2 = new[]
            {
                grid[x][y+2],
                grid[x+1][y+1],
                grid[x+2][y]
            }
            let word1 = new string(diag1)
            let word2 = new string(diag2)
            let word1Rev = new string(diag1.Reverse().ToArray())
            let word2Rev = new string(diag2.Reverse().ToArray())
            where (patterns.Contains(word1) || patterns.Contains(word1Rev)) &&
                  (patterns.Contains(word2) || patterns.Contains(word2Rev)) &&
                  diag1[1] == 'A' && diag2[1] == 'A'
            select 1
        ).Sum();

        return count;
    }
}
