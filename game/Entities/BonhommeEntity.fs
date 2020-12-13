module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE1 = "bonhomme32-2-piskel"
let ASSET_BONHOMME_SPRITE2 = "bonhomme62-piskel"
let SPEED_BONHOMME_SPRITE = 2f
let ANIMATION_FRAME_TIME = 1f / 6f


let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    let nextSprite =
        Sprites.updateSpriteState gameTime (currentGameEntity.Sprite) ANIMATION_FRAME_TIME

    let vectorMovement =
        KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE

    let newVector =
        Vector2.Add(currentGameEntity.Position, vectorMovement)

    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = nextSprite }

    GameEntity.createGameEntity properties currentGameEntity.UpdateEntity


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

    GameEntity.createGameEntity properties update
