[<RequireQualifiedAccess>]
module BonhommeSprite

open Types
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

    let leftStaticSprite =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_STATIC_SPRITE) }

    let rightStaticTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_STATIC_SPRITE) }

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
      rightStaticSprite = rightStaticTexture
      leftStaticSprite = leftStaticSprite
      rightRunningAnimatedSprite = rightRunningTextures
      leftRunningAnimatedSprite = leftRunningTextures }


let updateSprite gameTime
                 (currentGameEntity: IGameEntity)
                 (properties: BonhommeProperties)
                 prevMovState
                 currentMovState
                 =

    let spriteSheet = properties.spriteSheet

    let nextSprite =
        match (prevMovState, currentMovState) with
        | _, Jumping (dir, _) ->

            let jumpingSprite =
                match dir with
                | Left -> spriteSheet.leftJumpingSprite
                | Right -> spriteSheet.rightJumpingSprite

            SingleSprite jumpingSprite

        | _, Inactive dir ->

            let staticSprite =
                match dir with
                | Left -> spriteSheet.leftStaticSprite
                | Right -> spriteSheet.rightStaticSprite

            SingleSprite staticSprite

        | _, Duck dir ->

            let duckSprite =
                match dir with
                | Left -> spriteSheet.leftDuckSprite
                | Right -> spriteSheet.rightDuckSprite

            SingleSprite duckSprite

        | Inactive _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> spriteSheet.leftRunningAnimatedSprite
                | Right -> spriteSheet.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Duck _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> spriteSheet.leftRunningAnimatedSprite
                | Right -> spriteSheet.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Running Left, Running Right -> createBonhommeAnimatedSprite spriteSheet.rightRunningAnimatedSprite
        | Running Right, Running Left -> createBonhommeAnimatedSprite spriteSheet.leftRunningAnimatedSprite
        | Running _, Running _ -> currentGameEntity.Sprite
        | _ -> currentGameEntity.Sprite

    Sprites.updateSpriteState gameTime nextSprite
