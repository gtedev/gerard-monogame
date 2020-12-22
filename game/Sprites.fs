module Sprites

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Graphics

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
        if (nextElapsedTime > animatedSpriteState.animatedFrameTime)
        then (computeNextSpriteIndex sprites currentSpriteIndex, 0f)
        else (currentSpriteIndex, nextElapsedTime)

    AnimatedSprite
        { animatedSpriteState with
              sprites = animatedSpriteState.sprites
              currentSpriteIndex = fst nextIndexAndElapsedTime
              elapsedTimeSinceLastFrame = snd nextIndexAndElapsedTime }


let updateSpriteState gameTime sprite =
    match sprite with
    | SingleSprite _ -> sprite
    | AnimatedSprite animatedSpriteState -> updateAnimatedSprite gameTime animatedSpriteState


let getTextureToDraw sprite =
    match sprite with
    | SingleSprite sprite -> sprite.texture
    | AnimatedSprite animatedSpriteState ->
        animatedSpriteState.sprites.[animatedSpriteState.currentSpriteIndex]
            .texture

let createAnimatedSprite animatedFrameTime sprite  =
    AnimatedSprite
        { sprites = sprite
          currentSpriteIndex = 0
          elapsedTimeSinceLastFrame = 0f
          animatedFrameTime = animatedFrameTime  }

let drawSprite (spriteBatch: SpriteBatch) (gameEntity: IGameEntity) =

    let textureToDraw = getTextureToDraw gameEntity.Sprite

    spriteBatch.Draw(textureToDraw, gameEntity.Position, Color.White)
