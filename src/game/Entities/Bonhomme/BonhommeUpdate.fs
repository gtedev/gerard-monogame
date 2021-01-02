namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module BonhommeUpdate =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Input
    open GerardMonogame.Constants.BonhommeConstants



    let private newJumpLeft = Jumping(Left, JUMP_VELOCITY_SPEED)



    let private newJumpRight = Jumping(Right, JUMP_VELOCITY_SPEED)



    let private extractDirection movState =
        match movState with
        | Running dir -> dir
        | Idle dir -> dir
        | Jumping (dir, _) -> dir
        | Duck dir -> dir



    let private withLeftBoarderWindowCheck posX (nextMov: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPos) = nextMov

        let nextXPosition =
            if posX + nextMovPos.X < 0f then 0f else nextMovPos.X

        (nextMovState, new Vector2(nextXPosition, nextMovPos.Y))



    let private withFloorCheck posY (nextMov: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPos) = nextMov

        let dir = extractDirection nextMovState

        if posY + nextMovPos.Y > FLOOR_HEIGHT
        then (Idle dir, new Vector2(nextMovPos.X, 0f))
        else nextMov



    let private updateYPosition (gt: GameTime) (nextMov: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPos) = nextMov

        let nextYPos =
            match nextMovState with
            | Jumping (prevDir, vl: CurrentJumpVelocity) ->

                vl * (float32 gt.ElapsedGameTime.TotalSeconds)

            | _ -> 0f

        (nextMovState, new Vector2(nextMovPos.X, nextYPos))



    let private updateMovementState (vectorMov: Vector2) (nextMov: (BonhommeMovemementState * Vector2)) =

        let (prevMovState, nextMovPos) = nextMov

        let nextMovState =
            match (prevMovState, vectorMov) with
            | (Jumping (prevDir, velocity), _) ->

                Jumping(prevDir, velocity + JUMP_VELOCITY_INCREASE_STEP)

            | (_, vectorMov) when vectorMov.Y < 0f && vectorMov.X < 0f -> newJumpLeft
            | (_, vectorMov) when vectorMov.Y < 0f && vectorMov.X > 0f -> newJumpRight
            | (Idle prevDir, vectorMov) when vectorMov.Y < 0f ->

                GameHelper.matchDirection prevDir newJumpLeft newJumpRight

            | (_, vectorMov) when vectorMov.Y > 0f && vectorMov.X < 0f -> Duck Left
            | (_, vectorMov) when vectorMov.Y > 0f && vectorMov.X > 0f -> Duck Right
            | (_, vectorMov) when vectorMov.X < 0f -> Running Left
            | (_, vectorMov) when vectorMov.X > 0f -> Running Right
            | (prevMovState, vectorMov) when vectorMov.X = 0f && vectorMov.Y = 0f ->

                let prevDir = extractDirection prevMovState
                GameHelper.matchDirection prevDir (Idle Left) (Idle Right)

            | (prevMovState, vectorMov) when vectorMov.Y > 0f ->

                let prevDir = extractDirection prevMovState
                GameHelper.matchDirection prevDir (Duck Left) (Duck Right)

            | (_, _) -> prevMovState

        (nextMovState, nextMovPos)



    let private updateXPosition (vectorMov: Vector2) (nextMov: (BonhommeMovemementState * Vector2)) =

        let (nextMovState, nextMovPos) = nextMov

        let xPos =
            match nextMovState with
            | Duck _ -> 0f
            | Running _ -> vectorMov.X * SPEED_RUNNING_BONHOMME
            | _ -> vectorMov.X

        (nextMovState, new Vector2(xPos, nextMovPos.Y))



    let private updateBonhommeStateAndPosition (gt: GameTime)
                                               (entityProps: GameEntityProperties)
                                               (bonhommeProps: BonhommeProperties)
                                               (vectorMov: Vector2)
                                               =

        let prevState = bonhommeProps.movementStatus

        (prevState, entityProps.position)
        |> updateMovementState vectorMov
        |> updateXPosition vectorMov
        |> updateYPosition gt
        |> withFloorCheck entityProps.position.Y
        |> withLeftBoarderWindowCheck entityProps.position.X



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: IGameEntity) (bonhommeProps: BonhommeProperties) =

        let vectorMovement =
            KeyboardState.getMovementVector (Keyboard.GetState())

        let prevMovState = bonhommeProps.movementStatus

        let (nextMovState, nextPosMov) =
            updateBonhommeStateAndPosition gt currentEntity.Properties bonhommeProps vectorMovement

        let newSprite =
            BonhommeSprite.updateSprite gt currentEntity bonhommeProps prevMovState nextMovState


        let nextBonhommeProps =
            BonhommeProperties
                { bonhommeProps with
                      movementStatus = nextMovState }
            |> Some

        let nextEntityProps =
            { currentEntity.Properties with
                  position = Vector2.Add(currentEntity.Position, nextPosMov)
                  sprite = newSprite }

        GameEntity.createEntity nextEntityProps nextBonhommeProps currentEntity.UpdateEntity
