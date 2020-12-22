module BonhommeUpdate

open Microsoft.Xna.Framework
open Types
open BonhommeConstants


let withFloorCheck positionY (nextMovementState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option)) =

    if positionY + (snd3 nextMovementState).Y > FLOOR_HEIGHT
    then (Inactive, new Vector2((snd3 nextMovementState).X, 0f), None)
    else nextMovementState



let updateYPosition (gameTime: GameTime)
                    (nextMovementState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                    =

    let jumpPosition =
        match (fst3 nextMovementState, thrd3 nextMovementState) with
        | Jumping, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let newVectorPosition =
        new Vector2((snd3 nextMovementState).X, jumpPosition)

    updateSnd3 newVectorPosition nextMovementState



let updateMovementState previousState
                        (vectorMovement: Vector2)
                        (nextMovementState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                        =

    let state =
        if previousState = Jumping || vectorMovement.Y < 0f
        then Jumping
        elif vectorMovement.X > 0f
        then Running Right
        elif vectorMovement.X < 0f
        then Running Left
        elif vectorMovement.X = 0f && vectorMovement.Y = 0f
        then Inactive
        else previousState


    updateFst3 state nextMovementState



let updateJumpVelocityOrDefault (bonhommeProperties: BonhommeProperties)
                                (nextMovementState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                                =

    let velocity =
        match (fst3 nextMovementState, bonhommeProperties.jumpVelocityState) with
        | Jumping, Some velocity -> Some(velocity + 25f)
        | Jumping, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    updateThrd3 velocity nextMovementState



let updateXPosition (vectorMovement: Vector2)
                    (nextMovementState: (BonhommeMovemementState * Vector2 * CurrentJumpVelocity option))
                    =
    let newVector =
        new Vector2(vectorMovement.X, (snd3 nextMovementState).Y)

    updateSnd3 newVector nextMovementState



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
