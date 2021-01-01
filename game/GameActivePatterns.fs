namespace GerardMonogame.Game

[<AutoOpen>]
module GameActivePatterns =

    open GerardMonogame.Game.Types

    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> entity properties within a tuple.
    /// </summary>
    /// <param name="someEntity">An optional game entity.</param>
    /// <returns>A tuple of game properties and bonhomme properties.</returns>
    let (|SomeBonhommeEntity|_|) (someEntity: IGameEntity option) =
        match someEntity with
        | Some entity ->
            match entity.CustomEntityProperties with
            | Some (BonhommeProperties properties) -> Some((entity.Properties, properties))
            | _ -> None
        | _ -> None
