[<RequireQualifiedAccess>]
module GameState

open Types
open Microsoft.Xna.Framework


let updateEntities gameTime gameState: GameState =
    let newEntities =
        gameState.entities
        |> List.map (fun entity -> entity.UpdateEntity gameTime entity)

    { entities = newEntities }



let initializeEntities<'T when 'T :> Game> (game: 'T) (gameState: GameState) =

    let bonhommeGameEntity = BonhommeEntity.initializeEntity game

    { gameState with
          entities = [ bonhommeGameEntity ] }
