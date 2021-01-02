namespace FSharp.Core.Extensions

[<RequireQualifiedAccess>]
module ReadOnlyDict =

    open System.Collections.Generic



    /// <summary>
    /// Builds a new read only dictionary by applying a map function to each element of the dictionary.
    /// </summary>
    /// <param name="mapper">mapper function applied to each element of the read only dictionary.</param>
    /// <returns>A read only dictionary with mapped elements.</returns>
    let map mapper (dict: readonlydict<'k, 'v>) =

        let kvps =
            dict
            |> Seq.map (|KeyValue|)
            |> Seq.map mapper
            |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>



    /// <summary>Apply the given function to each element of the read only dictionary.</summary>
    /// <param name="action">action function applied to each element of the read only dictionary.</param>
    /// <returns>unit.</returns>
    let iter action (dict: readonlydict<'k, 'v>) =

        dict 
        |> Seq.map (|KeyValue|) 
        |> Seq.iter action



    /// <summary>Builds a new read only dictionary by applying a filter function on the read only dictionary.</summary>
    /// <param name="predicate">predicate function applied to filter the read only dictionary.</param>
    /// <returns>A read only dictionary filtered.</returns>
    let filter predicate (dict: readonlydict<'k, 'v>) =

        let kvps =
            dict
            |> Seq.map (|KeyValue|)
            |> Seq.filter predicate
            |> Seq.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>



    /// <summary>Try to get value in read only dictionary by its key. Returns an Option<value>.</summary>
    /// <param name="key">key to look for.</param>
    /// <returns>Option value element retrieved.</returns>
    let tryGetValue key (dict: readonlydict<'k, 'v>) =

        match dict.TryGetValue key with
        | true, value -> Some(value)
        | _ -> None
