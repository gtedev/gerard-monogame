[<RequireQualifiedAccess>]
module GameState

open Types
open Microsoft.Xna.Framework
open FSharp.Core.Extensions

let updateEntities gameTime (gameState: GameState) =
    let newEntities =
        gameState.entities
        |> ReadOnlyDict.map (fun (key, entity) -> (key, entity.UpdateEntity gameTime gameState entity))

    { entities = newEntities }



let initializeEntities<'T when 'T :> Game> (game: 'T) (gameState: GameState) =

    let bonhommeGameEntity =
        BonhommeEntity.initializeEntity game gameState

    let level1GameEntity =
        Level1Entity.initializeEntity game gameState

    let kvpEntities =
        [ level1GameEntity; bonhommeGameEntity ]
        |> List.map (fun entity -> (entity.Properties.id, entity))
        |> List.toReadOnlyDict

    { gameState with
          entities = kvpEntities }
