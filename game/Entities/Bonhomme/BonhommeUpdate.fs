module BonhommeUpdate

open Microsoft.Xna.Framework
open Types
open BonhommeConstants

let currentVelocityWithFloorCheck positionY (nextMovementState: (BonhommeMovemementState * Vector2 * float32 option)) =

    let currentVelocity =
        if positionY + (snd3 nextMovementState).Y > FLOOR_HEIGHT
        then None
        else thrd3 nextMovementState

    (fst3 nextMovementState, snd3 nextMovementState, currentVelocity)


let currentMovementStateWithFloorCheck positionY (nextMovementState: (BonhommeMovemementState * Vector2 * float32 option)) =
    let currentMovementState =
        if fst3 nextMovementState = Jumping
           && (positionY + (snd3 nextMovementState).Y > FLOOR_HEIGHT) then
            Inactive
        else
            fst3 nextMovementState

    (currentMovementState, snd3 nextMovementState, thrd3 nextMovementState)


let withFloorCheck positionY (nextMovementState: (BonhommeMovemementState * Vector2 * float32 option)) =

    if positionY + (snd3 nextMovementState).Y > FLOOR_HEIGHT
    then (Inactive, new Vector2((snd3 nextMovementState).X, 0f), thrd3 nextMovementState)
    else nextMovementState


let updateYPosition (gameTime: GameTime) (nextMovementState: (BonhommeMovemementState * Vector2 * float32 option)) =

    let jumpPosition =
        match (fst3 nextMovementState, thrd3 nextMovementState) with
        | Jumping, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let newVectorPosition =
        new Vector2((snd3 nextMovementState).X, jumpPosition)

    (fst3 nextMovementState, newVectorPosition, thrd3 nextMovementState)


let updateMovementState previousState (vectorMovement: Vector2) nextMovementState =

    let state =
        if previousState = Jumping || vectorMovement.Y < 0f
        then Jumping
        elif vectorMovement.X = 0f && vectorMovement.Y = 0f
        then Inactive
        else Running

    (state, snd3 nextMovementState, thrd3 nextMovementState)


let updateJumpVelocityOrDefault (bonhommeProperties: BonhommeProperties) nextMovementState =

    let velocity =
        match (fst3 nextMovementState, bonhommeProperties.jumpState) with
        | Jumping, Some velocity -> Some(velocity + 25f)
        | Jumping, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    (fst3 nextMovementState, snd3 nextMovementState, velocity)


let updateXPosition (vectorMovement: Vector2) (nextMovementState: (BonhommeMovemementState * Vector2 * float32 option)) =
    let newVector =
        new Vector2(vectorMovement.X, (snd3 nextMovementState).Y)

    (fst3 nextMovementState, newVector, thrd3 nextMovementState)


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
