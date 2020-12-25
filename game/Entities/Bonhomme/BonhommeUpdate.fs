[<RequireQualifiedAccess>]
module BonhommeUpdate

open Microsoft.Xna.Framework
open Types
open BonhommeConstants
open BonhommeTypes

let private extractDirection movState =
    match movState with
    | Running direction -> direction
    | Inactive direction -> direction
    | Jumping direction -> direction
    | Duck direction -> direction


let private withFloorCheck positionY (nextMovement: NextBonhommeMovement) =

    let (nextMovState, nextMovPosition, _) = nextMovement

    let direction = extractDirection nextMovState

    if positionY + nextMovPosition.Y > FLOOR_HEIGHT
    then (Inactive direction, new Vector2(nextMovPosition.X, 0f), None)
    else nextMovement



let private updateYPosition (gameTime: GameTime) (nextMovement: NextBonhommeMovement) =

    let (nextMovState, nextMovPosition, nextJumpVelocity) = nextMovement

    let nextYPosition =
        match (nextMovState, nextJumpVelocity) with
        | Jumping dir, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let newVectorPosition =
        new Vector2(nextMovPosition.X, nextYPosition)

    updateSnd3 newVectorPosition nextMovement



let private updateMovementState prevMovState (vectorMovement: Vector2) (nextMovement: NextBonhommeMovement) =

    let state =
        match (prevMovState, vectorMovement) with
        | (Jumping dir, _) -> Jumping dir
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X < 0f -> Jumping Left
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X > 0f -> Jumping Right
        | (Inactive dir, vectorMovement) when vectorMovement.Y < 0f ->

            match dir with
            | Left -> Jumping Left
            | Right -> Jumping Right
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

    updateFst3 state nextMovement



let private updateJumpVelocityOrDefault bonhommeProperties nextMovement =

    let (nextMovState, _, _) = nextMovement

    let velocity =
        match (nextMovState, bonhommeProperties.jumpVelocityState) with
        | Jumping _, Some velocity -> Some(velocity + JUMP_VELOCITY_INCREASE_STEP)
        | Jumping _, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    updateThrd3 velocity nextMovement



let private updateXPosition (vectorMovement: Vector2) (nextMovement: NextBonhommeMovement) =

    let (nextMovState, nextMovPosition, _) = nextMovement

    let xPosition =
        match nextMovState with
        | Duck dir -> 0f
        | Running _ -> vectorMovement.X * SPEED_RUNNING_BONHOMME
        | _ -> vectorMovement.X

    let newVector =
        new Vector2(xPosition, nextMovPosition.Y)

    updateSnd3 newVector nextMovement



let updateBonhommeStateAndPosition (gameTime: GameTime)
                                   (properties: GameEntityProperties)
                                   (bonhommeProperties: BonhommeProperties)
                                   (vectorMovement: Vector2)
                                   =

    let previousState = bonhommeProperties.movementStatus

    (previousState, properties.position, None)
    |> updateMovementState previousState vectorMovement
    |> updateXPosition vectorMovement
    |> updateJumpVelocityOrDefault bonhommeProperties
    |> updateYPosition gameTime
    |> withFloorCheck properties.position.Y
