module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE1 = "bonhomme3-piskel"
let ASSET_BONHOMME_SPRITE2 = "bonhomme5-piskel"
let SPEED_BONHOMME_SPRITE = 2f

let updateAnimatedSpriteState sprites currentIndex =

    let lastIndex = List.length sprites - 1

    let nexIndex =
        if currentIndex = lastIndex then 0 else (currentIndex + 1)

    AnimatedSprite(sprites, nexIndex)

let updateSpriteState sprite =
    match sprite with
    | SingleSprite spriteTexture -> sprite
    | AnimatedSprite (sprites, currentIndex) -> updateAnimatedSpriteState sprites currentIndex

let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    let sprite =
        updateSpriteState (currentGameEntity.Sprite)

    let vectorMovement =
        KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE

    let newVector =
        Vector2.Add(currentGameEntity.Position, vectorMovement)

    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = sprite }

    EntityHelper.createGameEntity properties currentGameEntity.UpdateEntity


let initializeEntity (game: Game) =

    let spriteTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) } ]

    let properties =
        { position = new Vector2(0f, 100f)
          sprite = AnimatedSprite(spriteTextures, 0)
          isEnabled = true }

    EntityHelper.createGameEntity properties update
