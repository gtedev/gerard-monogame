namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module Sprites =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Graphics


    let private nextSpriteIndex sprites currentIndex =
        let lastIndex = List.length sprites - 1

        if currentIndex = lastIndex then 0 else (currentIndex + 1)



    let private updateAnimatedSprite (g: GameTime) (animState: AnimatedSpriteState) =

        let currentIndex = animState.currentSpriteIndex
        let sprites = animState.sprites

        let nextElapsedTime =
            (float32) g.ElapsedGameTime.TotalSeconds
            + animState.elapsedTimeSinceLastFrame

        let (nextIndex, nextElapsedTime) =
            if (nextElapsedTime > animState.frameTime)
            then (nextSpriteIndex sprites currentIndex, 0f)
            else (currentIndex, nextElapsedTime)

        AnimatedSprite
            { animState with
                  sprites = animState.sprites
                  currentSpriteIndex = nextIndex
                  elapsedTimeSinceLastFrame = nextElapsedTime }



    let private getTextureToDraw (s: Sprite) =
        match s with
        | SingleSprite s -> s.texture
        | AnimatedSprite animState ->
            animState.sprites.[animState.currentSpriteIndex]
                .texture



    let updateSpriteState (gt: GameTime) (s: Sprite) =
        match s with
        | SingleSprite _ -> s
        | AnimatedSprite animState -> updateAnimatedSprite gt animState



    let createAnimatedSprite frameTime sprite =

        AnimatedSprite
            { sprites = sprite
              currentSpriteIndex = 0
              elapsedTimeSinceLastFrame = 0f
              frameTime = frameTime }



    let drawSprite (spriteBatch: SpriteBatch) (ge: IGameEntity) =

        let t = getTextureToDraw ge.Sprite

        spriteBatch.Draw(t, ge.Position, Color.White)
