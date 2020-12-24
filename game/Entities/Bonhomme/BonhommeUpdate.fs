[<RequireQualifiedAccess>]
module BonhommeUpdate

open Microsoft.Xna.Framework
open Types
open BonhommeConstants

let private extractDirection movState =
    match movState with
    | Running direction -> direction
    | Inactive direction -> direction
    | Jumping direction -> direction


let private withFloorCheck positionY (nextMovState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option)) =

    let direction = extractDirection (fst3 nextMovState)

    if positionY + (snd3 nextMovState).Y > FLOOR_HEIGHT
    then (Inactive direction, new Vector2((snd3 nextMovState).X, 0f), None)
    else nextMovState



let private updateYPosition (gameTime: GameTime)
                            (nextMovState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                            =
    let nextYPosition =
        match (fst3 nextMovState, thrd3 nextMovState) with
        | Jumping dir, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let newVectorPosition =
        new Vector2((snd3 nextMovState).X, nextYPosition)

    updateSnd3 newVectorPosition nextMovState



let private updateMovementState prevMovState
                                (vectorMovement: Vector2)
                                (nextMovState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                                =
    let state =
        match (prevMovState, vectorMovement) with
        | (Jumping dir, _) -> Jumping dir
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X < 0f -> Jumping Left
        | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X > 0f -> Jumping Right
        | (Inactive dir, vectorMovement) when vectorMovement.Y < 0f ->

            match dir with
            | Left -> Jumping Left
            | Right -> Jumping Right

        | (_, vectorMovement) when vectorMovement.X < 0f -> Running Left
        | (_, vectorMovement) when vectorMovement.X > 0f -> Running Right
        | (Jumping dir, vectorMovement) when vectorMovement.X = 0f && vectorMovement.Y = 0f ->

            match dir with
            | Left -> Inactive Left
            | Right -> Inactive Right

        | (Running dir, vectorMovement) when vectorMovement.X = 0f && vectorMovement.Y = 0f ->

            match dir with
            | Left -> Inactive Left
            | Right -> Inactive Right

        | (_, _) -> prevMovState

    updateFst3 state nextMovState



let private updateJumpVelocityOrDefault (bonhommeProperties: BonhommeProperties)
                                        (nextMovState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                                        =
    let velocity =
        match (fst3 nextMovState, bonhommeProperties.jumpVelocityState) with
        | Jumping _, Some velocity -> Some(velocity + 25f)
        | Jumping _, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    updateThrd3 velocity nextMovState



let private updateXPosition (vectorMovement: Vector2)
                            (nextMoveState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                            =
    let newVector =
        new Vector2(vectorMovement.X, (snd3 nextMoveState).Y)

    updateSnd3 newVector nextMoveState



let updateBonhommeStateAndPosition (gameTime: GameTime)
                                   (properties: GameEntityProperties)
                                   (bonhommeProperties: BonhommeProperties)
                                   (vectorMovement: Vector2)
                                   =

    let previousState = bonhommeProperties.movementStatus

    (previousState, properties.position, None)
    |> updateXPosition vectorMovement
    |> updateMovementState previousState vectorMovement
    |> updateJumpVelocityOrDefault bonhommeProperties
    |> updateYPosition gameTime
    |> withFloorCheck properties.position.Y
