namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module Sprites =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Graphics


    let private nextSpriteIndex sprites currentIndex =
        let lastIndex = List.length sprites - 1

        if currentIndex = lastIndex then 0 else (currentIndex + 1)



    let private updateAnimatedSprite (gt: GameTime) (animState: AnimatedSpriteState) =

        let currentIndex = animState.currentSpriteIndex
        let sprites = animState.sprites

        let nextElapsedTime =
            (float32) gt.ElapsedGameTime.TotalSeconds
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



    let private getTextureToDraw sprite =
        match sprite with
        | SingleSprite sprite -> sprite.texture
        | AnimatedSprite animState ->
            animState.sprites.[animState.currentSpriteIndex]
                .texture



    let createSpriteTexture (g: Game) assetName =
        { texture = g.Content.Load<Texture2D>(assetName) }



    let createSpriteTextures (g: Game) assetNames =
        assetNames |> List.map (createSpriteTexture g)



    let updateSpriteState (gt: GameTime) sprite =
        match sprite with
        | SingleSprite _ -> sprite
        | AnimatedSprite animState -> updateAnimatedSprite gt animState



    let createAnimatedSprite frameTime sprite =

        AnimatedSprite
            { sprites = sprite
              currentSpriteIndex = 0
              elapsedTimeSinceLastFrame = 0f
              frameTime = frameTime }



    let drawSprite (spriteBatch: SpriteBatch) (entity: IGameEntity) =

        let texture = getTextureToDraw entity.Sprite

        spriteBatch.Draw(texture, entity.Position, Color.White)
