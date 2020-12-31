[<RequireQualifiedAccess>]
module Level1Update

open Microsoft.Xna.Framework
open GameTypes
open Level1Constants
open Microsoft.Xna.Framework.Input

let private withBoarderScreenLeftCheck positionX (vectorMovement: Vector2) =

    let nextXPosition =
        if positionX + vectorMovement.X > 0f then 0f else vectorMovement.X

    new Vector2(nextXPosition, vectorMovement.Y)



let updateLevel1XPosition (vectorMovement: Vector2) =
    new Vector2(vectorMovement.X * (-1f) * SPEED_MOVING_FLOOR, vectorMovement.Y)



let updateLevel1YPosition (vectorMovement: Vector2) =
    // maintain level background to same Y position !
    new Vector2(vectorMovement.X, 0f)



let updateEntity gameTime (gameState: GameState) (currentGameEntity: IGameEntity) (properties: Level1Properties) =

    let vectorMovement =
        KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

    let gameProperties = currentGameEntity.Properties

    let nextVectorPosition =
        vectorMovement
        |> updateLevel1XPosition
        |> updateLevel1YPosition
        |> withBoarderScreenLeftCheck gameProperties.position.X

    let nextLevel1Properties = Level1Properties(properties) |> Some

    let nextGameEntityProperties =
        { currentGameEntity.Properties with
              position = Vector2.Add(currentGameEntity.Position, nextVectorPosition) }

    GameEntity.createGameEntity nextGameEntityProperties nextLevel1Properties currentGameEntity.UpdateEntity
