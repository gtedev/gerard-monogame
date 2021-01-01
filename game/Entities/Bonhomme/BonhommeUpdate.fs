namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module BonhommeUpdate =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Input
    open GerardMonogame.Constants

    let private newJumpLeft =
        Jumping(Left, BonhommeConstants.JUMP_VELOCITY_SPEED)

    let private newJumpRight =
        Jumping(Right, BonhommeConstants.JUMP_VELOCITY_SPEED)

    let private extractDirection movState =
        match movState with
        | Running dir -> dir
        | Idle dir -> dir
        | Jumping (dir, _) -> dir
        | Duck dir -> dir



    let private withBoarderScreenLeftCheck positionX (nextMovement: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPosition) = nextMovement

        let nextXPosition =
            if positionX + nextMovPosition.X < 0f then 0f else nextMovPosition.X

        (nextMovState, new Vector2(nextXPosition, nextMovPosition.Y))


    let private withFloorCheck positionY (nextMovement: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPosition) = nextMovement

        let direction = extractDirection nextMovState

        if positionY + nextMovPosition.Y > BonhommeConstants.FLOOR_HEIGHT
        then (Idle direction, new Vector2(nextMovPosition.X, 0f))
        else nextMovement



    let private updateYPosition (gameTime: GameTime) (nextMovement: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPosition) = nextMovement

        let nextYPosition =
            match nextMovState with
            | Jumping (prevDir, velocity) ->
                velocity
                * (float32) gameTime.ElapsedGameTime.TotalSeconds
            | _ -> 0f

        (nextMovState, new Vector2(nextMovPosition.X, nextYPosition))



    let private updateMovementState (vectorMovement: Vector2) (nextMovement: (BonhommeMovemementState * Vector2)) =

        let (prevMovState, nextMovPosition) = nextMovement

        let state =
            match (prevMovState, vectorMovement) with
            | (Jumping (prevDir, velocity), _) ->
                Jumping(
                    prevDir,
                    velocity
                    + BonhommeConstants.JUMP_VELOCITY_INCREASE_STEP
                )
            | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X < 0f -> newJumpLeft
            | (_, vectorMovement) when vectorMovement.Y < 0f && vectorMovement.X > 0f -> newJumpRight
            | (Idle prevDir, vectorMovement) when vectorMovement.Y < 0f ->

                GameHelper.matchDirection prevDir newJumpLeft newJumpRight

            | (_, vectorMovement) when vectorMovement.Y > 0f && vectorMovement.X < 0f -> Duck Left
            | (_, vectorMovement) when vectorMovement.Y > 0f && vectorMovement.X > 0f -> Duck Right
            | (_, vectorMovement) when vectorMovement.X < 0f -> Running Left
            | (_, vectorMovement) when vectorMovement.X > 0f -> Running Right
            | (prevMovState, vectorMovement) when vectorMovement.X = 0f && vectorMovement.Y = 0f ->

                let prevDir = extractDirection prevMovState
                GameHelper.matchDirection prevDir (Idle Left) (Idle Right)

            | (prevMovState, vectorMovement) when vectorMovement.Y > 0f ->

                let prevDir = extractDirection prevMovState
                GameHelper.matchDirection prevDir (Duck Left) (Duck Right)

            | (_, _) -> prevMovState

        (state, nextMovPosition)



    let private updateXPosition (vectorMovement: Vector2) (nextMovement: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPosition) = nextMovement

        let xPosition =
            match nextMovState with
            | Duck _ -> 0f
            | Running _ ->
                vectorMovement.X
                * BonhommeConstants.SPEED_RUNNING_BONHOMME
            | _ -> vectorMovement.X

        (nextMovState, new Vector2(xPosition, nextMovPosition.Y))



    let private updateBonhommeStateAndPosition (gameTime: GameTime)
                                               (properties: GameEntityProperties)
                                               (bonhommeProperties: BonhommeProperties)
                                               (vectorMovement: Vector2)
                                               =

        let previousState = bonhommeProperties.movementStatus

        (previousState, properties.position)
        |> updateMovementState vectorMovement
        |> updateXPosition vectorMovement
        |> updateYPosition gameTime
        |> withFloorCheck properties.position.Y
        |> withBoarderScreenLeftCheck properties.position.X


    let updateEntity gameTime (gameState: GameState) (currentGameEntity: IGameEntity) (properties: BonhommeProperties) =

        let vectorMovement =
            KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

        let prevMovementState = properties.movementStatus

        let (nextMovementState, nextPositionMovement) =
            updateBonhommeStateAndPosition gameTime currentGameEntity.Properties properties vectorMovement

        let newSprite =
            BonhommeSprite.updateSprite gameTime currentGameEntity properties prevMovementState nextMovementState


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
