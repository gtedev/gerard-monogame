namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameHelper =

    open Types

    /// <summary>Match direction then return appropriate left / right params.</summary>
    let matchDirection direction whenLeft whenRight =
        match direction with
        | Left -> whenLeft
        | Right -> whenRight
