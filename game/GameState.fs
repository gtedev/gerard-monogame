[<RequireQualifiedAccess>]
module GameState

open GameTypes
open Microsoft.Xna.Framework


let updateEntities gameTime (gameState: GameState) =
    let newEntities =
        gameState.entities
        |> Dict.map (fun (key, entity) -> (key, entity.UpdateEntity gameTime gameState entity))

    { entities = newEntities }



let initializeEntities<'T when 'T :> Game> (game: 'T) (gameState: GameState) =

    let bonhommeGameEntity = BonhommeEntity.initializeEntity game gameState
    let level1GameEntity = Level1Entity.initializeEntity game gameState

    let kvpEntities =
        [ level1GameEntity; bonhommeGameEntity ]
        |> List.map (fun entity -> (entity.Properties.id, entity))
        |> List.toDict

    { gameState with
          entities = kvpEntities }
