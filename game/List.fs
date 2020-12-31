[<RequireQualifiedAccess>]
module List

open System.Collections.Generic

let toDict (list: ('k * 'v) list) =

    let results =
        list
        |> List.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))
        |> dict

    results
