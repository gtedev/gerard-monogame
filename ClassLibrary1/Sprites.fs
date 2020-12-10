module Sprites

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Graphics

let computeNextSpriteIndex sprites currentSpriteIndex =
    let lastIndex = List.length sprites - 1
    if currentSpriteIndex = lastIndex then 0 else (currentSpriteIndex + 1)


let updateAnimatedSprite (gameTime: GameTime) animatedSpriteState animationFrameTime =

    let nextElapsedTime =
        (float32) gameTime.ElapsedGameTime.TotalSeconds
        + animatedSpriteState.elapsedTimeSinceLastFrame

    let currentSpriteIndex = animatedSpriteState.currentSpriteIndex
    let sprites = animatedSpriteState.sprites

    let nextIndexAndElapsedTime =
        if (nextElapsedTime > animationFrameTime)
        then (computeNextSpriteIndex sprites currentSpriteIndex, 0f)
        else (currentSpriteIndex, nextElapsedTime)

    AnimatedSprite
        { sprites = animatedSpriteState.sprites
          currentSpriteIndex = fst nextIndexAndElapsedTime
          elapsedTimeSinceLastFrame = snd nextIndexAndElapsedTime }


let updateSpriteState gameTime sprite animationFrameTime =
    match sprite with
    | SingleSprite _ -> sprite
    | AnimatedSprite animatedSpriteState -> updateAnimatedSprite gameTime animatedSpriteState animationFrameTime


let getTextureToDraw sprite =
    match sprite with
    | SingleSprite sprite -> sprite.texture
    | AnimatedSprite animatedSpriteState -> animatedSpriteState.sprites.[animatedSpriteState.currentSpriteIndex].texture


let drawSprite (spriteBatch: SpriteBatch) (gameEntity: IGameEntity) =

    let textureToDraw = getTextureToDraw gameEntity.Sprite

    spriteBatch.Draw(textureToDraw, gameEntity.Position, Color.White)