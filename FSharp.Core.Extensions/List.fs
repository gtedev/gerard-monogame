namespace FSharp.Core.Extensions

[<RequireQualifiedAccess>]
module List =

    open System.Collections.Generic

    /// <summary>Views the given List as a <see cref="ReadOnlyDictionary"/>.</summary>
    /// <param name="list">The list to transform to a <see cref="ReadOnlyDictionary"/>.</param>
    /// <returns>A read only dictionary.</returns>
    let toReadOnlyDict (list: ('k * 'v) list) =

        let kvps =
            list
            |> List.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>
