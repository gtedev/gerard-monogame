namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameHelper =

    open Types



    /// <summary>Match direction then return appropriate left / right params.</summary>
    let matchDirection left right dir =
        match dir with
        | Left -> left
        | Right -> right
