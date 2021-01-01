namespace FSharp.Core.Extensions

[<RequireQualifiedAccess>]
module ReadOnlyDict =

    open System.Collections.Generic

    /// <summary>Builds a new dictionary by applying a map function to each element of the dictionary.</summary>
    let map mapper (dictionary: readonlydict<'k, 'v>) =

        let kvps =
            dictionary
            |> Seq.map (|KeyValue|)
            |> Seq.map mapper
            |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>

    /// <summary>Apply the given function to each element of the dictionary.</summary>
    let iter action (dictionary: readonlydict<'k, 'v>) =

        dictionary
        |> Seq.map (|KeyValue|)
        |> Seq.iter action

    /// <summary>Builds a new dictionary by applying a filter function on the dictionary.</summary>
    let filter predicate (dictionary: readonlydict<'k, 'v>) =

        let kvps =
            dictionary
            |> Seq.map (|KeyValue|)
            |> Seq.filter predicate
            |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>

    /// <summary>Try to get value in Dictionary by its key. Returns an Option<value>.</summary>
    let tryGetValue key (dictionary: readonlydict<'k, 'v>) =

        match dictionary.TryGetValue key with
        | true, value -> Some(value)
        | _ -> None
