namespace GerardMonogame.Game

[<AutoOpen>]
module GameActivePatterns =

    open GerardMonogame.Game.Types

    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> properties.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>Bonhomme properties.</returns>
    let (|Bonhomme|_|) (e: IGameEntity) =
        match e.CustomEntityProperties with
        | Some (BonhommeProperties p) -> Some((e.Properties, p))
        | _ -> None



    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> entity properties within a tuple.
    /// </summary>
    /// <param name="someEntity">An optional game entity.</param>
    /// <returns>A tuple of game properties and bonhomme properties.</returns>
    let (|SomeBonhomme|_|) (e: IGameEntity option) =
        match e with
        | Some (Bonhomme allProps) -> Some(allProps)
        | _ -> None



    /// <summary>
    /// Active pattern that returns <see cref="Level1"/> entity properties within a tuple.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>A tuple of game properties and Level1 properties.</returns>
    let (|Level1|_|) (e: IGameEntity) =
        match e.CustomEntityProperties with
        | Some (Level1Properties p) -> Some((e.Properties, p))
        | _ -> None
