namespace GerardMonogame.Game

[<AutoOpen>]
module GameActivePatterns =

    open GerardMonogame.Game.Types

    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> properties.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>Bonhomme properties.</returns>
    let (|Bonhomme|_|) (entity: IGameEntity) =
        match entity.CustomEntityProperties with
        | Some (BonhommeProperties properties) -> Some((entity.Properties, properties))
        | _ -> None

    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> entity properties within a tuple.
    /// </summary>
    /// <param name="someEntity">An optional game entity.</param>
    /// <returns>A tuple of game properties and bonhomme properties.</returns>
    let (|SomeBonhomme|_|) (someEntity: IGameEntity option) =
        match someEntity with
        | Some entity ->
            match entity with
            | Bonhomme allEntityProperties -> Some(allEntityProperties)
            | _ -> None
        | _ -> None

    /// <summary>
    /// Active pattern that returns <see cref="Level1"/> entity properties within a tuple.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>A tuple of game properties and Level1 properties.</returns>
    let (|Level1|_|) (entity: IGameEntity) =
        match entity.CustomEntityProperties with
        | Some (Level1Properties properties) -> Some((entity.Properties, properties))
        | _ -> None
