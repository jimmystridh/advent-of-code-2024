module Part2

open System
open System.IO

let readInput () =
    let rec loop accLeft accRight =
        match Console.ReadLine() with
        | null -> (List.rev accLeft, List.rev accRight)
        | line ->
            let tokens = line.Trim().Split([|' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
            if tokens.Length <> 2 then
                failwithf "Invalid input line: %s" line
            else
                let leftNum = int tokens.[0]
                let rightNum = int tokens.[1]
                loop (leftNum::accLeft) (rightNum::accRight)
    loop [] []

let run () =
    // Read the input lists
    let (leftList, rightList) = readInput ()

    // Create a frequency map for the right list
    let rightCounts = 
        rightList
        |> Seq.countBy id
        |> Map.ofSeq

    // Calculate the similarity score
    let similarityScore =
        leftList
        |> List.map (fun num ->
            let count = Map.tryFind num rightCounts |> Option.defaultValue 0
            num * count)
        |> List.sum

    // Output the similarity score
    printfn "%d" similarityScore