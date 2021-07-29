namespace FSharp.Core.Extensions

[<AutoOpen>]
module Types =

    open System.Collections.Generic


    type readonlydict<'a, 'b> = IReadOnlyDictionary<'a, 'b>
