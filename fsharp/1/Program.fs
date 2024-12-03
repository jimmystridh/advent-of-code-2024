open System

[<EntryPoint>]
let main argv =
    let exitCode =
        match argv with
        | [|"1"|] -> 
            Part1.run ()
            0
        | [|"2"|] -> 
            Part2.run ()
            0
        | _ -> 
            printfn "Please specify which part to run (1 or 2)"
            printfn "Usage: dotnet run -- 1"
            printfn "       dotnet run -- 2"
            1
    exitCode