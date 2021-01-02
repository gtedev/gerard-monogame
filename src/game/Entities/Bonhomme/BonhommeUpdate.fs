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



    let private withLeftBoarderWindowCheck posX (sp: (BonhommeMovemementState * Vector2)) =

        let (state, position) = sp

        let nextXPosition =
            if posX + position.X < 0f then 0f else position.X

        (state, new Vector2(nextXPosition, position.Y))



    let private withFloorCheck posY (sp: (BonhommeMovemementState * Vector2)) =

        let (state, position) = sp

        let dir = extractDirection state
        let nextYPos = posY + position.Y

        match nextYPos with
        | _ when nextYPos > FLOOR_HEIGHT ->

            (Idle dir, new Vector2(position.X, 0f))

        | _ -> sp



    let private updateYPosition (gt: GameTime) (sp: (BonhommeMovemementState * Vector2)) =

        let (state, position) = sp

        let nextYPos =
            match state with
            | Jumping (prevDir, vl: CurrentJumpVelocity) ->

                vl * (float32 gt.ElapsedGameTime.TotalSeconds)

            | _ -> 0f

        (state, new Vector2(position.X, nextYPos))



    let private updateMovementState (vectorMov: Vector2) (sp: (BonhommeMovemementState * Vector2)) =

        let (state, position) = sp

        let nextMovState =
            match state with
            | Jumping (dir, velocity) ->

                Jumping(dir, velocity + JUMP_VELOCITY_INCREASE_STEP)

            | _ when vectorMov.Y < 0f && vectorMov.X < 0f ->

                Jumping(Left, JUMP_VELOCITY_SPEED)

            | _ when vectorMov.Y < 0f && vectorMov.X > 0f ->

                Jumping(Right, JUMP_VELOCITY_SPEED)

            | Idle dir when vectorMov.Y < 0f ->

                GameHelper.matchDirection dir newJumpLeft newJumpRight

            | _ when vectorMov.Y > 0f && vectorMov.X < 0f ->

                Duck Left

            | _ when vectorMov.Y > 0f && vectorMov.X > 0f ->

                Duck Right

            | _ when vectorMov.X < 0f ->

                Running Left

            | _ when vectorMov.X > 0f ->

                Running Right

            | _ when vectorMov.X = 0f && vectorMov.Y = 0f ->

                let dir = extractDirection state
                GameHelper.matchDirection dir (Idle Left) (Idle Right)

            | _ when vectorMov.Y > 0f ->

                let dir = extractDirection state
                GameHelper.matchDirection dir (Duck Left) (Duck Right)

            | _ -> state


        (nextMovState, position)



    let private updateXPosition (vectorMov: Vector2) (sp: (BonhommeMovemementState * Vector2)) =

        let (state, position) = sp

        let xPos =
            match state with
            | Duck _ -> 0f
            | Running _ -> vectorMov.X * SPEED_RUNNING_BONHOMME
            | _ -> vectorMov.X

        (state, new Vector2(xPos, position.Y))



    let private updateBonhommeStateAndPosition (gt: GameTime)
                                               (entityProps: GameEntityProperties)
                                               (bonhommeProps: BonhommeProperties)
                                               (vectorMov: Vector2)
                                               =

        let currentMovState = bonhommeProps.movementStatus

        (currentMovState, entityProps.position)
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

        currentEntity
        |> GameEntity.updateEntity nextEntityProps nextBonhommeProps
