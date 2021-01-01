[<RequireQualifiedAccess>]
module Level1Update

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open GerardMonogame.Constants

let private withBoarderScreenLeftCheck positionX (vectorMovement: Vector2) =

    let nextXPosition =
        if positionX + vectorMovement.X > 0f then 0f else vectorMovement.X

    new Vector2(nextXPosition, vectorMovement.Y)



let updateLevel1XPosition (bonhommeEntity: IGameEntity) (vectorMovement: Vector2) =
    if bonhommeEntity.Position.X > Level1Constants.LEVEL1_BONHOMME_X_POSITION_MOVE_TRIGGER then
        new Vector2(
            vectorMovement.X
            * (-1f)
            * Level1Constants.SPEED_MOVING_FLOOR,
            vectorMovement.Y
        )
    else
        new Vector2(0f, 0f)


let updateLevel1YPosition (vectorMovement: Vector2) =
    // maintain level background to same Y position !
    new Vector2(vectorMovement.X, 0f)


let updateLevel1Entity (bonhommeEntity: IGameEntity) (currentGameEntity: IGameEntity) (properties: Level1Properties) =

    let vectorMovement =
        KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

    let gameProperties = currentGameEntity.Properties

    let nextVectorPosition =
        vectorMovement
        |> updateLevel1XPosition bonhommeEntity
        |> updateLevel1YPosition
        |> withBoarderScreenLeftCheck gameProperties.position.X

    let nextLevel1Properties = Level1Properties(properties) |> Some

    let nextGameEntityProperties =
        { currentGameEntity.Properties with
              position = Vector2.Add(currentGameEntity.Position, nextVectorPosition) }

    GameEntity.createGameEntity nextGameEntityProperties nextLevel1Properties currentGameEntity.UpdateEntity



let updateEntity gameTime (gameState: GameState) (currentGameEntity: IGameEntity) (properties: Level1Properties) =

    let someEntity = GameEntity.getBonhommeEntity gameState

    match someEntity with
    | Some bonhommeEntity -> updateLevel1Entity bonhommeEntity currentGameEntity properties
    | None -> currentGameEntity
