namespace FSharp.Core.Extensions

[<RequireQualifiedAccess>]
module List =

    open System.Collections.Generic

    let toReadOnlyDict (list: ('k * 'v) list) =

        let kvps =
            list
            |> List.map (fun kvp -> KeyValuePair(fst kvp, snd kvp))

        Dictionary(kvps) :> readonlydict<'k, 'v>
