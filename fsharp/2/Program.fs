open System

// Read all input lines into a list
let readLines () =
    Seq.initInfinite (fun _ -> Console.ReadLine())
    |> Seq.takeWhile (fun line -> line <> null)
    |> Seq.toList

// Function to check if a report is safe according to the rules
let isSafeReport (levels: int array) =
    if levels.Length < 2 then
        false // Not enough levels to compare
    else
        let diffs = Array.pairwise levels |> Array.map (fun (a, b) -> b - a)
        let allPositive = diffs |> Array.forall (fun x -> x > 0)
        let allNegative = diffs |> Array.forall (fun x -> x < 0)
        let absDiffsValid = diffs |> Array.forall (fun x -> abs x >= 1 && abs x <= 3)
        absDiffsValid && (allPositive || allNegative)

[<EntryPoint>]
let main argv =
    let lines = readLines()

    let mutable safeCountPart1 = 0
    let mutable safeCountPart2 = 0

    for line in lines do
        if line.Trim() <> "" then
            let levels = line.Split([|' '|], StringSplitOptions.RemoveEmptyEntries) |> Array.map int
            if isSafeReport levels then
                // Safe under original rules
                safeCountPart1 <- safeCountPart1 + 1
                safeCountPart2 <- safeCountPart2 + 1
            else
                // Try removing one level at a time for Part Two
                let mutable foundSafe = false
                for i in 0 .. levels.Length - 1 do
                    let newLevels = [| for j in 0 .. levels.Length - 1 do if j <> i then yield levels.[j] |]
                    if isSafeReport newLevels then
                        foundSafe <- true
                        //break
                if foundSafe then
                    safeCountPart2 <- safeCountPart2 + 1

    printfn "Part One: %d" safeCountPart1
    printfn "Part Two: %d" safeCountPart2
    0 // Return an integer exit code