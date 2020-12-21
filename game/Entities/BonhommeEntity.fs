module BonhommeEntity

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

let currentVelocityWithFloorCheck positionY (result: (BonhommeMovemementState * Vector2 * float32 option)) =

    let currentVelocity =
        if positionY + (snd3 result).Y > FLOOR_HEIGHT
        then None
        else thrd3 result

    (fst3 result, snd3 result, currentVelocity)

let currentMovementStateWithFloorCheck positionY (result: (BonhommeMovemementState * Vector2 * float32 option)) =
    let currentMovementState =
        if fst3 result = Jumping
           && (positionY + (snd3 result).Y > FLOOR_HEIGHT) then
            Inactive
        else
            fst3 result

    (currentMovementState, snd3 result, thrd3 result)

let withFloorCheck positionY (result: (BonhommeMovemementState * Vector2 * float32 option)) =

    if positionY + (snd3 result).Y > FLOOR_HEIGHT
    then (Inactive, new Vector2((snd3 result).X, 0f), thrd3 result)
    else result

let nextJumpYPosition (gameTime: GameTime) (result: (BonhommeMovemementState * Vector2 * float32 option)) =

    let jumpPosition =
        match (fst3 result, thrd3 result) with
        | Jumping, Some velocity ->
            ((velocity)
             * (float32) gameTime.ElapsedGameTime.TotalSeconds)
        | _ -> 0f

    let newVectorPosition =
        new Vector2((snd3 result).X, jumpPosition)

    (fst3 result, newVectorPosition, thrd3 result)

let currentMovementState previousState (vectorMovement: Vector2) result =

    let state =
        if previousState = Jumping || vectorMovement.Y < 0f
        then Jumping
        elif vectorMovement.X = 0f && vectorMovement.Y = 0f
        then Inactive
        else Running

    (state, snd3 result, thrd3 result)

let currentVelocity (bonhommeProperties: BonhommeProperties) result =

    let velocity =
        match (fst3 result, bonhommeProperties.jumpState) with
        | Jumping, Some velocity -> Some(velocity + 25f)
        | Jumping, None -> Some(JUMP_VELOCITY_SPEED)
        | _, _ -> None

    (fst3 result, snd3 result, velocity)


let currentXPosition (vectorMovement:Vector2) (result: (BonhommeMovemementState * Vector2 * float32 option)) =
    let newVector =  new Vector2(vectorMovement.X, (snd3 result).Y)
    (fst3 result, newVector, thrd3 result)

let computeCurrentMovementState (gameTime: GameTime)
                                (properties: GameEntityProperties)
                                (bonhommeProperties: BonhommeProperties)
                                (vectorMovement: Vector2)
                                =

    let previousState = bonhommeProperties.movementStatus

    (previousState, properties.position, None)
    |> currentXPosition vectorMovement
    |> currentMovementState previousState vectorMovement
    |> currentVelocity bonhommeProperties
    |> nextJumpYPosition gameTime
    |> withFloorCheck properties.position.Y



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

    let currentMovementState =
        computeCurrentMovementState gameTime currentGameEntity.Properties properties vectorMovement

    let newProperties =
        { properties with
              movementStatus = fst3 currentMovementState
              jumpState = thrd3 currentMovementState }

    let bonhommeProperties = Some(BonhommeProperties newProperties)

    let newSprite =
        updateSprite gameTime currentGameEntity properties previousMovement currentMovementState

    let newVector =
        Vector2.Add(currentGameEntity.Position, snd3 currentMovementState)


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
