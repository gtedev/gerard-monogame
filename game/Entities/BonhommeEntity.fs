﻿module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE1 = "bonhomme32-2-piskel"
let ASSET_BONHOMME_SPRITE2 = "bonhomme32-2-piskel"
let ASSET_BONHOMME_SPRITE3 = "bonhomme62-piskel"
let ASSET_BONHOMME_JUMPING = "bonhomme-jumping-piskel"

let SPEED_BONHOMME_SPRITE = 2f
let ANIMATION_FRAME_TIME = 1f / 8f
let JUMP_VELOCITY_SPEED = -800f
let FLOOR_HEIGHT = 350f



let currentMovementState (gameTime: GameTime)
                         (properties: GameEntityProperties)
                         (bonhommeProperties: BonhommeProperties)
                         (vectorMovement: Vector2)
                         =

    let positionY = properties.position.Y
    let previousState = bonhommeProperties.movementStatus

    let currentMovementState =
        if previousState = Jumping || vectorMovement.Y < 0f
        then Jumping
        elif vectorMovement.X = 0f && vectorMovement.Y = 0f
        then Inactive
        else Running

    let currentVelocity =
        match (currentMovementState, bonhommeProperties.jumpState) with
        | Jumping, Some velocity -> Some(velocity + 25f)
        | Jumping, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    let nextPositionYMovement =
        match (currentMovementState, currentVelocity) with
        | Jumping, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let nextPositionYMovementWithFloor =
        if positionY + nextPositionYMovement > FLOOR_HEIGHT
        then 0f
        else nextPositionYMovement

    let currentMovementState2 =
        if currentMovementState = Jumping
           && (positionY + nextPositionYMovement > FLOOR_HEIGHT) then
            Inactive
        else
            currentMovementState

    let currentVelocity2 =
        if positionY + nextPositionYMovement > FLOOR_HEIGHT
        then None
        else currentVelocity

    (currentMovementState2, new Vector2(vectorMovement.X, nextPositionYMovementWithFloor), currentVelocity2)



let updateSprite gameTime
                 (currentGameEntity: IGameEntity)
                 (properties: BonhommeProperties)
                 previousMovement
                 currentMovement
                 =

    let spriteToPass =
        match (previousMovement, fst3 currentMovement) with
        | _, Jumping -> SingleSprite properties.jumpingSprite
        | Inactive, Running ->
            AnimatedSprite
                { sprites = properties.runningAnimatedSprite
                  currentSpriteIndex = 0
                  elapsedTimeSinceLastFrame = 0f
                  animatedFrameTime = ANIMATION_FRAME_TIME }
        | Running, Running -> currentGameEntity.Sprite
        | Running, Inactive -> SingleSprite properties.staticSprite
        | _ -> SingleSprite properties.staticSprite

    Sprites.updateSpriteState gameTime spriteToPass



let updateEntity gameTime (currentGameEntity: IGameEntity) (properties: BonhommeProperties) =

    let vectorMovement =
        KeyboardState.getMovementVector (Keyboard.GetState())

    let previousMovement = properties.movementStatus

    let currentMovement =
        currentMovementState gameTime currentGameEntity.Properties properties vectorMovement

    let currentVelocity = thrd3 currentMovement

    let newProperties =
        { properties with
              movementStatus = fst3 currentMovement
              jumpState = currentVelocity }

    let bonhommeProperties = Some(BonhommeProperties newProperties)

    let newSprite =
        updateSprite gameTime currentGameEntity properties previousMovement currentMovement

    let newVector =
        Vector2.Add(currentGameEntity.Position, snd3 currentMovement)


    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = newSprite }

    GameEntity.createGameEntity properties bonhommeProperties currentGameEntity.UpdateEntity



let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (BonhommeProperties properties) -> updateEntity gameTime currentGameEntity properties

    | _ -> currentGameEntity



let initializeEntity (game: Game) =

    let jumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_JUMPING) }

    let staticTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }

    let animatedRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE3) } ]

    let bonhommeProperties =
        BonhommeProperties
            { jumpState = None
              movementStatus = Inactive
              jumpingSprite = jumpingTexture
              staticSprite = staticTexture
              runningAnimatedSprite = animatedRunningTextures }
        |> Some

    let properties =
        { position = new Vector2(0f, 350f)
          sprite = SingleSprite staticTexture
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties update
