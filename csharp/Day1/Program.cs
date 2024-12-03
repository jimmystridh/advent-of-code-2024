// Read input
var (leftList, rightList) = ReadInput();

// Part One: Calculate Total Distance
int totalDistance = CalculateTotalDistance(leftList, rightList);
Console.WriteLine($"Total distance: {totalDistance}");

// Part Two: Calculate Similarity Score
int similarityScore = CalculateSimilarityScore(leftList, rightList);
Console.WriteLine($"Similarity score: {similarityScore}");

static (List<int> leftList, List<int> rightList) ReadInput()
{
    var leftList = new List<int>();
    var rightList = new List<int>();

    string line;
    while ((line = Console.ReadLine()!) != null)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        var tokens = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length != 2)
            throw new ArgumentException($"Invalid input line: {line}");

        if (!int.TryParse(tokens[0], out int leftNum) || !int.TryParse(tokens[1], out int rightNum))
            throw new ArgumentException($"Invalid numbers in line: {line}");

        leftList.Add(leftNum);
        rightList.Add(rightNum);
    }

    return (leftList, rightList);
}

static int CalculateTotalDistance(List<int> leftList, List<int> rightList)
{
    var sortedLeft = leftList.OrderBy(x => x).ToList();
    var sortedRight = rightList.OrderBy(x => x).ToList();

    int totalDistance = sortedLeft
        .Zip(sortedRight, (a, b) => Math.Abs(a - b))
        .Sum();

    return totalDistance;
}

static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
{
    var rightCounts = rightList
        .GroupBy(x => x)
        .ToDictionary(g => g.Key, g => g.Count());

    int similarityScore = leftList
        .Select(num => num * rightCounts.GetValueOrDefault(num, 0))
        .Sum();

    return similarityScore;
}


// Extension method to simplify dictionary access
public static class DictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue = default!)
    {
        return dictionary.TryGetValue(key, out TValue? value) ? value : defaultValue;
    }
}
