[<RequireQualifiedAccess>]
module Dict

open System.Collections.Generic

/// <summary>Builds a new dictionary by applying a map function to each element of the dictionary.</summary>
let map mapper (dictionary: dict<'k, 'v>) =

    let results =
        dictionary
        |> Seq.map (|KeyValue|)
        |> Seq.map mapper
        |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))
        |> dict

    results

/// <summary>Apply the given function to each element of the dictionary.</summary>
let iter action (dictionary: dict<'k, 'v>) =

    dictionary
    |> Seq.map (|KeyValue|)
    |> Seq.iter action

/// <summary>Builds a new dictionary by applying a filter function on the dictionary.</summary>
let filter predicate (dictionary: dict<'k, 'v>) =

    let results =
        dictionary
        |> Seq.map (|KeyValue|)
        |> Seq.filter predicate
        |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))
        |> dict

    results