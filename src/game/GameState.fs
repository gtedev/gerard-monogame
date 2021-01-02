namespace GerardMonogame.Game

open GerardMonogame.Game.Entities

[<RequireQualifiedAccess>]
module GameState =

    open Types
    open Microsoft.Xna.Framework
    open FSharp.Core.Extensions

    let updateEntities gameTime (gs: GameState) =

        let newEntities =
            gs.entities
            |> ReadOnlyDict.map (fun (key, entity) -> (key, entity.UpdateEntity gameTime gs entity))

        { entities = newEntities }



    let initEntities (game: Game) (gs: GameState) =

        let bh = BonhommeEntity.initEntity game gs

        let lvl1 = Level1Entity.initEntity game gs

        let entities =
            [ lvl1; bh ]
            |> List.map (fun e -> (e.Properties.id, e))
            |> List.toReadOnlyDict

        { gs with entities = entities }
