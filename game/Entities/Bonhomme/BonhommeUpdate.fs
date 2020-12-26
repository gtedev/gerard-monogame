[<RequireQualifiedAccess>]
module BonhommeUpdate

open Microsoft.Xna.Framework
open Types
open BonhommeConstants
open Microsoft.Xna.Framework.Input

let private extractDirection movState =
    match movState with
    | Running dir -> dir
    | Inactive dir -> dir
    | Jumping (dir, _) -> dir
    | Duck dir -> dir


let private withFloorCheck positionY (nextMovement: (BonhommeMovemementState * Vector2)) =

    let (nextMovState, nextMovPosition) = nextMovement

    let direction = extractDirection nextMovState

    if positionY + nextMovPosition.Y > FLOOR_HEIGHT
    then (Inactive direction, new Vector2(nextMovPosition.X, 0f))
    else nextMovement



let private updateYPosition (gameTime: GameTime) (nextMovement: (BonhommeMovemementState * Vector2)) =

    let (nextMovState, nextMovPosition) = nextMovement

    let nextYPosition =
        match nextMovState with
        | Jumping (prevDir, velocity) ->
            velocity
            * (float32) gameTime.ElapsedGameTime.TotalSeconds
        | _ -> 0f

    let newVectorPosition =
        new Vector2(nextMovPosition.X, nextYPosition)

    updateSnd2 newVectorPosition nextMovement



let private updateMovementState prevMovState
                                (vectorMovement: Vector2)
                                (nextMovement: (BonhommeMovemementState * Vector2))
                                =

    let (_, nextMovPosition) = nextMovement

    let state =
        match (prevMovState, vectorMovement) with
        | (Jumping (prevDir, velocity), _) -> Jumping(prevDir, velocity + JUMP_VELOCITY_INCREASE_STEP)
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X < 0f -> Jumping(Left, JUMP_VELOCITY_SPEED)
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X > 0f -> Jumping(Right, JUMP_VELOCITY_SPEED)
        | (Inactive prevDir, vectorMovement) when vectorMovement.Y < 0f ->

            match prevDir with
            | Left -> Jumping(Left, JUMP_VELOCITY_SPEED)
            | Right -> Jumping(Right, JUMP_VELOCITY_SPEED)

        | (_, vectorMovement) when vectorMovement.Y > 0f && vectorMovement.X < 0f -> Duck Left
        | (_, vectorMovement) when vectorMovement.Y > 0f && vectorMovement.X > 0f -> Duck Right
        | (_, vectorMovement) when vectorMovement.X < 0f -> Running Left
        | (_, vectorMovement) when vectorMovement.X > 0f -> Running Right
        | (prevMovState, vectorMovement) when vectorMovement.X = 0f && vectorMovement.Y = 0f ->

            let prevDir = extractDirection prevMovState

            match prevDir with
            | Left -> Inactive Left
            | Right -> Inactive Right

        | (prevMovState, vectorMovement) when vectorMovement.Y > 0f ->

            let prevDir = extractDirection prevMovState

            match prevDir with
            | Left -> Duck Left
            | Right -> Duck Right

        | (_, _) -> prevMovState

    (state, nextMovPosition)



let private updateXPosition (vectorMovement: Vector2) (nextMovement: (BonhommeMovemementState * Vector2)) =

    let (nextMovState, nextMovPosition) = nextMovement

    let xPosition =
        match nextMovState with
        | Duck _ -> 0f
        | Running _ -> vectorMovement.X * SPEED_RUNNING_BONHOMME
        | _ -> vectorMovement.X

    let newMovVector =
        new Vector2(xPosition, nextMovPosition.Y)

    (nextMovState, newMovVector)



let private updateBonhommeStateAndPosition (gameTime: GameTime)
                                           (properties: GameEntityProperties)
                                           (bonhommeProperties: BonhommeProperties)
                                           (vectorMovement: Vector2)
                                           =

    let previousState = bonhommeProperties.movementStatus

    (previousState, properties.position)
    |> updateMovementState previousState vectorMovement
    |> updateXPosition vectorMovement
    |> updateYPosition gameTime
    |> withFloorCheck properties.position.Y


let updateEntity gameTime (currentGameEntity: IGameEntity) (properties: BonhommeProperties) =

    let vectorMovement =
        KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

    let previousMovement = properties.movementStatus

    let (nextMovementState, nextPositionMovement) =
        updateBonhommeStateAndPosition gameTime currentGameEntity.Properties properties vectorMovement

    let newSprite =
        BonhommeSprite.updateSprite gameTime currentGameEntity properties previousMovement nextMovementState


    let nextBonhommeProperties =
        BonhommeProperties
            { properties with
                  movementStatus = nextMovementState }
        |> Some

    let nextGameEntityProperties =
        { currentGameEntity.Properties with
              position = Vector2.Add(currentGameEntity.Position, nextPositionMovement)
              sprite = newSprite }

    GameEntity.createGameEntity nextGameEntityProperties nextBonhommeProperties currentGameEntity.UpdateEntity
