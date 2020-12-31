[<RequireQualifiedAccess>]
module Level1Update

open Microsoft.Xna.Framework
open Types
open Level1Constants
open Microsoft.Xna.Framework.Input

let updateLevel1XPosition (vectorMovement: Vector2) =
    new Vector2(vectorMovement.X * (-1f) * SPEED_MOVING_FLOOR, vectorMovement.Y)


let updateLevel1YPosition (vectorMovement: Vector2) =
    // maintain level background to same Y position !
    new Vector2(vectorMovement.X, 0f)


let updateEntity gameTime (currentGameEntity: IGameEntity) (properties: Level1Properties) =

    let vectorMovement =
        KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

    let nextVectorPosition =
        vectorMovement
        |> updateLevel1XPosition
        |> updateLevel1YPosition

    let nextLevel1Properties = Level1Properties(properties) |> Some

    let nextGameEntityProperties =
        { currentGameEntity.Properties with
              position = Vector2.Add(currentGameEntity.Position, nextVectorPosition) }

    GameEntity.createGameEntity nextGameEntityProperties nextLevel1Properties currentGameEntity.UpdateEntity
