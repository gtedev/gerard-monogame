namespace GerardMonogame.Game

open GerardMonogame.Game.Entities

[<RequireQualifiedAccess>]
module GameState =

    open Types
    open Microsoft.Xna.Framework
    open FSharp.Core.Extensions

    let updateEntities (gt: GameTime) (gs: GameState) =

        let newEntities =
            gs.entities
            |> ReadOnlyDict.map (fun (key, entity) -> (key, entity.updateEntity gt gs entity))

        { entities = newEntities }



    let initEntities (g: Game) (gs: GameState) =

        let bh = BonhommeEntity.initEntity g gs

        let lvl1 = Level1Entity.initEntity g gs

        let mechant = MechantEntity.initEntity g gs

        let entities =
            [ lvl1; bh; mechant ]
            |> List.map (fun entity -> (entity.id, entity))
            |> List.toReadOnlyDict

        { gs with entities = entities }
