module Part1

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
    let (leftList, rightList) = readInput ()
    let sortedLeft = List.sort leftList
    let sortedRight = List.sort rightList
    let pairs = List.zip sortedLeft sortedRight
    let distances = pairs |> List.map (fun (a,b) -> abs (a - b))
    let totalDistance = List.sum distances
    printfn "%d" totalDistance 