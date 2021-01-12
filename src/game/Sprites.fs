namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module Sprites =

    open Microsoft.Xna.Framework
    open Types
    open Microsoft.Xna.Framework.Graphics
    open FSharp.Core.Extensions


    type SpriteTextureFactory(game: Game) =

        let _createSpriteTexture assetName = game.Content.Load<Texture2D>(assetName)

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


    let private updateAnimatedSprite (gt: GameTime) (animSpriteProps: AnimatedSpriteProperties) nextPosition =

        let currentIndex = animSpriteProps.currentSpriteIndex
        let sprites = animSpriteProps.sprites

        let (ElapsedTimeSinceLastFrame elapsedTime) =
            animSpriteProps.elapsedTimeSinceLastFrame

        let nextElapsedTime =
            (float32 gt.ElapsedGameTime.TotalSeconds)
            + elapsedTime

        let (nextIndex, nextElapsedTime) =
            if (nextElapsedTime > animSpriteProps.frameTime)
            then (nextSpriteIndex sprites currentIndex, 0f)
            else (currentIndex, nextElapsedTime)

        let nextAnimProps =
            { animSpriteProps with
                  sprites = animSpriteProps.sprites
                  currentSpriteIndex = nextIndex
                  elapsedTimeSinceLastFrame = ElapsedTimeSinceLastFrame nextElapsedTime
                  position = nextPosition }

        AnimatedSprite nextAnimProps



    ////let private getTextureToDraw sprite =
    ////    match sprite with
    ////    | SingleSprite sprite -> [ sprite ]
    ////    | AnimatedSprite animProps ->
    ////        let (CurrentSpriteIndex currentIndex) = animProps.currentSpriteIndex
    ////        [ animProps.sprites.[currentIndex] ]
    ////    | GroupOfSprites list -> list



    let updateSpriteState (gt: GameTime) sprite position =
        match sprite with
        | AnimatedSprite animState -> updateAnimatedSprite gt animState position
        | SingleSprite props -> SingleSprite({ props with position = position })
        | GroupOfSprites _ -> sprite



    let createAnimatedSprite frameTime position sprites =

        AnimatedSprite
            { sprites = sprites
              currentSpriteIndex = CurrentSpriteIndex 0
              elapsedTimeSinceLastFrame = ElapsedTimeSinceLastFrame 0f
              frameTime = frameTime
              position = position }



    let drawEntity (game: Game) (spriteBatch: SpriteBatch) (entity: GameEntity) =

        let sprite = entity.sprite

        let drawSingleSprite (sp: SingleSpriteProperties) =
            spriteBatch.Draw(sp.texture, sp.position, Color.White)


        match sprite with
        | SingleSprite props ->

            drawSingleSprite props

        | AnimatedSprite animProps ->

            let (CurrentSpriteIndex currentIndex) = animProps.currentSpriteIndex
            let texture = animProps.sprites.[currentIndex]

            spriteBatch.Draw(texture, animProps.position, Color.White)

        | GroupOfSprites spList -> spList |> List.iter drawSingleSprite



    let drawEntities (g: Game) (sb: SpriteBatch) (entities: readonlydict<GameEntityId, GameEntity>) =

        entities
        |> ReadOnlyDict.filter (fun (_, entity) -> entity.isEnabled)
        |> ReadOnlyDict.iter (fun (_, entity) -> entity.drawEntity g sb entity)
