module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE1 = "bonhomme32-piskel"
let ASSET_BONHOMME_SPRITE2 = "bonhomme6-piskel"
let SPEED_BONHOMME_SPRITE = 2f
let ANIMATION_FRAME_TIME = 1f / 6f


let computeNextSpriteIndex sprites currentSpriteIndex =
    let lastIndex = List.length sprites - 1
    if currentSpriteIndex = lastIndex then 0 else (currentSpriteIndex + 1)


let updateAnimatedSprite (gameTime: GameTime) animatedSpriteState =

    let nextElapsedTime =
        (float32) gameTime.ElapsedGameTime.TotalSeconds
        + animatedSpriteState.elapsedTimeSinceLastFrame

    let currentSpriteIndex = animatedSpriteState.currentSpriteIndex
    let sprites = animatedSpriteState.sprites

    let nextIndexAndElapsedTime =
        if (nextElapsedTime > ANIMATION_FRAME_TIME)
        then (computeNextSpriteIndex sprites currentSpriteIndex, 0f)
        else (currentSpriteIndex, nextElapsedTime)

    AnimatedSprite
        { sprites = animatedSpriteState.sprites
          currentSpriteIndex = fst nextIndexAndElapsedTime
          elapsedTimeSinceLastFrame = snd nextIndexAndElapsedTime }


let updateSpriteState gameTime sprite =
    match sprite with
    | SingleSprite spriteTexture -> sprite
    | AnimatedSprite animatedSpriteState -> updateAnimatedSprite gameTime animatedSpriteState


let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    let nextSprite =
        updateSpriteState gameTime (currentGameEntity.Sprite)

    let vectorMovement =
        KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE

    let newVector =
        Vector2.Add(currentGameEntity.Position, vectorMovement)

    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = nextSprite }

    EntityHelper.createGameEntity properties currentGameEntity.UpdateEntity


let initializeEntity (game: Game) =

    let spriteTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) } ]

    let properties =
        { position = new Vector2(0f, 100f)
          sprite =
              AnimatedSprite
                  { sprites = spriteTextures
                    currentSpriteIndex = 0
                    elapsedTimeSinceLastFrame = 0f }
          isEnabled = true }

    EntityHelper.createGameEntity properties update
