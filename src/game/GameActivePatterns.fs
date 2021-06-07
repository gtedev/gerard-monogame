namespace GerardMonogame.Game

[<AutoOpen>]
module GameActivePatterns =

    open GerardMonogame.Game.Types



    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> entity properties within a tuple.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>Bonhomme properties.</returns>
    let (|Bonhomme|_|) (entity: GameEntity) =
        match entity.extendProperties with
        | BonhommeProperties bonhommeProps -> Some(bonhommeProps)
        | _ -> None



    /// <summary>
    /// Active pattern that returns <see cref="Bonhomme"/> entity properties within a tuple.
    /// </summary>
    /// <param name="someEntity">An optional game entity.</param>
    /// <returns>A tuple of game properties and bonhomme properties.</returns>
    let (|SomeBonhomme|_|) (entity: GameEntity option) =
        match entity with
        | Some (Bonhomme allProps) -> Some(allProps)
        | _ -> None



    /// <summary>
    /// Active pattern that returns <see cref="Level1"/> entity properties within a tuple.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>A tuple of game properties and Level1 properties.</returns>
    let (|Level1|_|) (entity: GameEntity) =
        match entity.extendProperties with
        | Level1Properties lvlProps -> Some(lvlProps)
        | _ -> None



    /// <summary>
    /// Active pattern that returns <see cref="Mechant"/> entity properties within a tuple.
    /// </summary>
    /// <param name="entity">game entity.</param>
    /// <returns>A tuple of game properties and Mechant properties.</returns>
    let (|Mechant|_|) (entity: GameEntity) =
        match entity.extendProperties with
        | MechantProperties mechantProps -> Some(mechantProps)
        | _ -> None
