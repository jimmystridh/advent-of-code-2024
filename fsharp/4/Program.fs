open System

[<EntryPoint>]
let main argv =
    // Read the grid from stdin or a file
    let grid = Seq.initInfinite (fun _ -> Console.ReadLine())
                |> Seq.takeWhile (fun line -> line <> null)
                |> Seq.map (fun line -> line.ToCharArray())
                |> Seq.toArray

    let countXMAS (grid: char[][]) : int =
        let numRows = grid.Length
        let numCols = grid.[0].Length
        let directions = [| (-1, 0); (-1, 1); (0, 1); (1, 1); (1, 0); (1, -1); (0, -1); (-1, -1) |]
        let wordToFind = [| 'X'; 'M'; 'A'; 'S' |]
        let length = wordToFind.Length

        let isValidPosition x y =
            x >= 0 && x < numRows && y >= 0 && y < numCols

        let mutable count = 0
        for x in 0 .. numRows - 1 do
            for y in 0 .. numCols - 1 do
                for (dx, dy) in directions do
                    let mutable matchFound = true
                    let mutable i = 0
                    let mutable xi = x
                    let mutable yi = y
                    while matchFound && i < length do
                        if isValidPosition xi yi && grid.[xi].[yi] = wordToFind.[i] then
                            xi <- xi + dx
                            yi <- yi + dy
                            i <- i + 1
                        else
                            matchFound <- false
                    if matchFound then
                        count <- count + 1
        count

    let countXMAS_X (grid: char[][]) : int =
        let numRows = grid.Length
        let numCols = grid.[0].Length
        let patterns = Set.ofList [ "MAS"; "SAM" ]
        let length = 3 // Length of "MAS" or "SAM"

        let isValidPosition x y =
            x >= 0 && x < numRows && y >= 0 && y < numCols

        let mutable count = 0
        for x in 0 .. numRows - 3 do
            for y in 0 .. numCols - 3 do
                // Positions for the diagonals
                let x0, y0 = x, y
                let x1, y1 = x + 1, y + 1
                let x2, y2 = x + 2, y + 2

                let x3, y3 = x, y + 2
                let x4, y4 = x + 2, y

                if isValidPosition x0 y0 && isValidPosition x1 y1 && isValidPosition x2 y2 &&
                   isValidPosition x3 y3 && isValidPosition x4 y4 then

                    let diag1 = [| grid.[x0].[y0]; grid.[x1].[y1]; grid.[x2].[y2] |]
                    let diag2 = [| grid.[x3].[y3]; grid.[x1].[y1]; grid.[x4].[y4] |]

                    let word1 = System.String.Concat(diag1)
                    let word2 = System.String.Concat(diag2)
                    let word1Rev = System.String.Concat(Array.rev diag1)
                    let word2Rev = System.String.Concat(Array.rev diag2)

                    if (patterns.Contains word1 || patterns.Contains word1Rev) &&
                       (patterns.Contains word2 || patterns.Contains word2Rev) &&
                       diag1.[1] = 'A' && diag2.[1] = 'A' then
                        count <- count + 1
        count

    let countPart1 = countXMAS grid
    let countPart2 = countXMAS_X grid

    printfn "Part One: %d" countPart1
    printfn "Part Two: %d" countPart2

    0 // return an integer exit code