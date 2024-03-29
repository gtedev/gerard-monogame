﻿namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open Microsoft.Xna.Framework.Graphics

[<RequireQualifiedAccess>]
module BonhommeUpdate =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Input
    open GerardMonogame.Constants.BonhommeConstants



    let private newJumpStraight dir =
        Jumping(Up dir, CurrentJumpVelocity JUMP_VELOCITY_SPEED)


    let private newJump dir =
        Jumping(Toward dir, CurrentJumpVelocity JUMP_VELOCITY_SPEED)


    let private extractDirection movState =
        match movState with
        | Running dir -> dir
        | Idle dir -> dir
        | Jumping (Toward dir, _) -> dir
        | Jumping (Up dir, _) -> dir
        | Duck dir -> dir



    let withXLimitPositionCheck currentXPosition (sp: BonhommeMovemementState * Vector2 * float32) =

        let (state, entityPosition, virtualXPos) = sp

        let nextXPosition =
            if entityPosition.X > POSITION_X_LIMIT_RUNNING_RIGHT then
                currentXPosition
            else if entityPosition.X < POSITION_X_LIMIT_RUNNING_LEFT
                    && virtualXPos > POSITION_X_LIMIT_RUNNING_LEFT then
                currentXPosition
            else
                entityPosition.X

        (state, new Vector2(nextXPosition, entityPosition.Y), virtualXPos)


    let private withLeftBoarderWindowCheck (sp: BonhommeMovemementState * Vector2 * float32) =

        let (state, entityPosition, virtualPosX) = sp

        let nextXPosition =
            if entityPosition.X < 0f then 0f else entityPosition.X

        let nextVirtualPosX =
            if virtualPosX < 0f then 0f else virtualPosX

        (state, new Vector2(nextXPosition, entityPosition.Y), nextVirtualPosX)



    let private withFloorCheck (sp: BonhommeMovemementState * Vector2 * float32) =

        let (state, entityPosition, virtualXPos) = sp
        let dir = extractDirection state

        match entityPosition.Y with
        | _ when entityPosition.Y > FLOOR_HEIGHT ->

            (Idle dir, new Vector2(entityPosition.X, FLOOR_HEIGHT), virtualXPos)

        | _ -> sp



    let private updateYPosition (gt: GameTime) (sp: BonhommeMovemementState * Vector2 * float32) =

        let (state, entityPosition, virtualXPos) = sp

        let nextYPos =
            match state with
            | Jumping (prevDir, (CurrentJumpVelocity vl)) ->

                entityPosition.Y
                + vl * (float32 gt.ElapsedGameTime.TotalSeconds)

            | _ -> entityPosition.Y

        (state, new Vector2(entityPosition.X, nextYPos), virtualXPos)



    let private updateMovementState (vectorMov: Vector2) (sp: BonhommeMovemementState * Vector2 * float32) =

        let (state, entityPosition, virtualPosX) = sp

        let nextMovState =
            match state with
            | Jumping (jmpDir, (CurrentJumpVelocity velocity)) ->

                Jumping(jmpDir, CurrentJumpVelocity(velocity + JUMP_VELOCITY_INCREASE_STEP))

            | _ when vectorMov.Y < 0f && vectorMov.X < 0f ->

                newJump Left

            | _ when vectorMov.Y < 0f && vectorMov.X > 0f ->

                newJump Right

            | Idle dir when vectorMov.Y < 0f ->

                newJumpStraight dir

            | _ when vectorMov.Y > 0f && vectorMov.X < 0f ->

                Duck Left

            | _ when vectorMov.Y > 0f && vectorMov.X > 0f ->

                Duck Right

            | _ when vectorMov.X < 0f ->

                Running Left

            | _ when vectorMov.X > 0f ->

                Running Right

            | _ when vectorMov.X = 0f && vectorMov.Y = 0f ->

                extractDirection state
                |> GameHelper.matchDirection (Idle Left) (Idle Right)

            | _ when vectorMov.Y > 0f ->

                extractDirection state
                |> GameHelper.matchDirection (Duck Left) (Duck Right)

            | _ -> state


        (nextMovState, entityPosition, virtualPosX)



    let private updateXPosition (currentState: BonhommeMovemementState)
                                (vectorMov: Vector2)
                                (sp: BonhommeMovemementState * Vector2 * float32)
                                =

        let (nextState, entityPosition, virtualPosX) = sp

        let (xPos, nextVirtualPosX) =
            match (currentState, nextState) with
            | _, Duck _ ->

                (entityPosition.X, virtualPosX)

            | Jumping (Up _, _), Jumping (_, _) ->

                (entityPosition.X, virtualPosX)

            | Jumping (Toward currDir, _), Jumping (_, _) ->

                let directionVectorMov =
                    GameHelper.matchDirection (-1f) (1f) currDir

                let distToward =
                    directionVectorMov * SPEED_RUNNING_BONHOMME

                (entityPosition.X + distToward, virtualPosX + distToward)


            | _ ->

                let distToward = vectorMov.X * SPEED_RUNNING_BONHOMME

                // if idle (vector.X = 0)
                (entityPosition.X + distToward, virtualPosX + distToward)

        (nextState, new Vector2(xPos, entityPosition.Y), nextVirtualPosX)



    let private updateBonhommeStateAndPosition (gt: GameTime) (bonhommeProps: BonhommeProperties) (vectorMov: Vector2) =

        let currentMovState = bonhommeProps.movementStatus

        (currentMovState, bonhommeProps.position, bonhommeProps.virtualPosX)
        |> updateMovementState vectorMov
        |> updateXPosition currentMovState vectorMov
        |> updateYPosition gt
        |> withFloorCheck
        |> withLeftBoarderWindowCheck
        |> withXLimitPositionCheck bonhommeProps.position.X


    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity) (bonhommeProps: BonhommeProperties) =

        let vectorMovement =
            KeyboardState.getMovementVector (Keyboard.GetState())

        let currentMovState = bonhommeProps.movementStatus

        let (nextMovState, nextPosition, nextVirtualXPos) =
            updateBonhommeStateAndPosition gt bonhommeProps vectorMovement

        let nextSprite =
            BonhommeSprite.updateSprite gt currentEntity bonhommeProps currentMovState nextMovState nextPosition


        let nextBonhommeProps =
            BonhommeProperties
                { bonhommeProps with
                      virtualPosX = nextVirtualXPos
                      movementStatus = nextMovState
                      position = nextPosition }

        ////currentEntity
        ////|> GameEntity.updateEntity nextSprite nextBonhommeProps

        let decoratorDraw (sv: SpriteService) e =

            Sprites.drawEntity sv e

            sv.spriteBatch.DrawString(
                sv.spriteFont,
                "bonhomme Position X:" + nextPosition.X.ToString(),
                new Vector2(100f, 100f),
                Color.Yellow
            )

            sv.spriteBatch.DrawString(
                sv.spriteFont,
                "bonhomme Virtual Pos X:"
                + nextVirtualXPos.ToString(),
                new Vector2(100f, 130f),
                Color.GreenYellow
            )

        { currentEntity with
              extendProperties = nextBonhommeProps
              sprite = nextSprite
              drawEntity = decoratorDraw }
