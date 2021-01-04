namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module Sprites =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Graphics
    open FSharp.Core.Extensions


    type SpriteTextureFactory(game: Game) =

        let _createSpriteTexture assetName =
            { texture = game.Content.Load<Texture2D>(assetName) }

        let _createSpriteTextures (assetNames: string list) =
            assetNames |> List.map (_createSpriteTexture)

        member this.createSpriteTexture assetName = _createSpriteTexture assetName
        member this.createSpriteTextures assetNames = _createSpriteTextures assetNames



    let private nextSpriteIndex sprites (currentIndex: CurrentSpriteIndex) =
        let lastIndex = List.length sprites - 1
        let (CurrentSpriteIndex currentIndex) = currentIndex

        let nextIndex =
            if currentIndex = lastIndex then 0 else (currentIndex + 1)

        CurrentSpriteIndex nextIndex


    let private updateAnimatedSprite (gt: GameTime) (animState: AnimatedSpriteState) =

        let currentIndex = animState.currentSpriteIndex
        let sprites = animState.sprites
        let (ElapsedTimeSinceLastFrame elapsedTime) = animState.elapsedTimeSinceLastFrame

        let nextElapsedTime =
            (float32 gt.ElapsedGameTime.TotalSeconds)
            + elapsedTime

        let (nextIndex, nextElapsedTime) =
            if (nextElapsedTime > animState.frameTime)
            then (nextSpriteIndex sprites currentIndex, 0f)
            else (currentIndex, nextElapsedTime)

        AnimatedSprite
            { animState with
                  sprites = animState.sprites
                  currentSpriteIndex = nextIndex
                  elapsedTimeSinceLastFrame = ElapsedTimeSinceLastFrame nextElapsedTime }



    let private getTextureToDraw sprite =
        match sprite with
        | SingleSprite sprite -> sprite.texture
        | AnimatedSprite animState ->
            let (CurrentSpriteIndex currentIndex) = animState.currentSpriteIndex
            animState.sprites.[currentIndex].texture



    let updateSpriteState (gt: GameTime) sprite =
        match sprite with
        | SingleSprite _ -> sprite
        | AnimatedSprite animState -> updateAnimatedSprite gt animState



    let createAnimatedSprite frameTime sprites =

        AnimatedSprite
            { sprites = sprites
              currentSpriteIndex = CurrentSpriteIndex 0
              elapsedTimeSinceLastFrame = ElapsedTimeSinceLastFrame 0f
              frameTime = frameTime }



    let private drawEntity (spriteBatch: SpriteBatch) (key, entity: GameEntity) =

        let texture =
            getTextureToDraw entity.properties.sprite

        spriteBatch.Draw(texture, entity.properties.position, Color.White)



    let drawEntities (sb: SpriteBatch) (entities: readonlydict<GameEntityId, GameEntity>) =

        entities
        |> ReadOnlyDict.filter (fun (_, entity) -> entity.properties.isEnabled)
        |> ReadOnlyDict.iter (drawEntity sb)
