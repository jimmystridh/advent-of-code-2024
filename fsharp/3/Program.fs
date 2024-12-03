open System
open System.Text.RegularExpressions

// Read the input from standard input
let input = Console.In.ReadToEnd()

// Define the regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
let pattern = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)"

// Find all matches in the input string
let matches = Regex.Matches(input, pattern)

// Part One: Sum of all valid 'mul' instructions
let partOneTotal =
    matches
    |> Seq.cast<Match>
    |> Seq.filter (fun m -> m.Value.StartsWith("mul"))
    |> Seq.map (fun m ->
        let x = int m.Groups.[1].Value
        let y = int m.Groups.[2].Value
        x * y
    )
    |> Seq.sum

// Part Two: Sum of enabled 'mul' instructions considering 'do()' and 'don't()'
let mutable mulEnabled = true
let mutable partTwoTotal = 0

for m in matches do
    let value = m.Value
    if value = "do()" then
        mulEnabled <- true
    elif value = "don't()" then
        mulEnabled <- false
    elif value.StartsWith("mul") then
        let x = int m.Groups.[1].Value
        let y = int m.Groups.[2].Value
        if mulEnabled then
            partTwoTotal <- partTwoTotal + (x * y)

// Output the results
printfn "%d" partOneTotal    // Output for Part One
printfn "%d" partTwoTotal    // Output for Part Two