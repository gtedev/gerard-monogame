[<RequireQualifiedAccess>]
module BonhommeSprite

open GameTypes
open BonhommeConstants
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let private createBonhommeAnimatedSprite =
    Sprites.createAnimatedSprite ANIMATION_FRAME_TIME


let createBonhommeSpriteSheet (game: Game) =

    let rightJumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_JUMPING_SPRITE) }

    let leftJumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_JUMPING_SPRITE) }

    let leftIdleSprite =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_IDLE_SPRITE) }

    let rightIdleTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_IDLE_SPRITE) }

    let leftDuckSprite =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_DUCK_SPRITE) }

    let rightDuckTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_DUCK_SPRITE) }

    let rightRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_2) } ]

    let leftRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_2) } ]

    { leftJumpingSprite = leftJumpingTexture
      rightJumpingSprite = rightJumpingTexture
      leftDuckSprite = leftDuckSprite
      rightDuckSprite = rightDuckTexture
      rightIdleSprite = rightIdleTexture
      leftIdleSprite = leftIdleSprite
      rightRunningSprites = rightRunningTextures
      leftRunningSprites = leftRunningTextures }


let updateSprite gameTime
                 (currentGameEntity: IGameEntity)
                 (properties: BonhommeProperties)
                 prevMovState
                 currentMovState
                 =

    let ss = properties.spriteSheet

    let nextSprite =
        match (prevMovState, currentMovState) with
        | _, Jumping (dir, _) ->

            let jumpingSprite =
                GameHelper.matchDirection dir ss.leftJumpingSprite ss.rightJumpingSprite

            SingleSprite jumpingSprite

        | _, Idle dir ->

            let idleSprite =
                GameHelper.matchDirection dir ss.leftIdleSprite ss.rightIdleSprite

            SingleSprite idleSprite

        | _, Duck dir ->

            let duckSprite =
                GameHelper.matchDirection dir ss.leftDuckSprite ss.rightDuckSprite

            SingleSprite duckSprite

        | Idle _, Running dir ->

            let runningSprites =
                GameHelper.matchDirection dir ss.leftRunningSprites ss.rightRunningSprites

            createBonhommeAnimatedSprite runningSprites

        | Duck _, Running dir ->

            let runningSprites =
                GameHelper.matchDirection dir ss.leftRunningSprites ss.rightRunningSprites

            createBonhommeAnimatedSprite runningSprites

        | Running Left, Running Right -> createBonhommeAnimatedSprite ss.rightRunningSprites
        | Running Right, Running Left -> createBonhommeAnimatedSprite ss.leftRunningSprites
        | Running _, Running _ -> currentGameEntity.Sprite
        | _ -> currentGameEntity.Sprite

    Sprites.updateSpriteState gameTime nextSprite
