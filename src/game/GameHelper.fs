namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameHelper =

    open Types



    /// <summary>Match direction then return appropriate left / right params.</summary>
    let matchDirection dir left right =
        match dir with
        | Left -> left
        | Right -> right
