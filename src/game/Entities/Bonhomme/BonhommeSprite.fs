namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module BonhommeSprite =

    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.BonhommeConstants



    type BonhommeSpriteManager(spriteSheet) =

        let _newAnimatedSprite =
            Sprites.createAnimatedSprite BonhommeConstants.ANIMATION_FRAME_TIME

        let _jumpSprite dir =
            GameHelper.matchDirection dir spriteSheet.leftJumpingSprite spriteSheet.rightJumpingSprite
            |> SingleSprite

        let _runningSprite dir =
            GameHelper.matchDirection dir spriteSheet.leftRunningSprites spriteSheet.rightRunningSprites
            |> _newAnimatedSprite

        let _duckSprite dir =
            GameHelper.matchDirection dir spriteSheet.leftDuckSprite spriteSheet.rightDuckSprite
            |> SingleSprite

        let _idleSprite dir =
            GameHelper.matchDirection dir spriteSheet.leftIdleSprite spriteSheet.rightIdleSprite
            |> SingleSprite

        member this.jumpSprite dir = _jumpSprite dir
        member this.runningSprite dir = _runningSprite dir
        member this.duckSprite dir = _duckSprite dir
        member this.idleSprite dir = _idleSprite dir
        member this.newAnimatedSprite dir = _newAnimatedSprite dir


    let createBonhommeSpriteSheet (g: Game) =

        let f = Sprites.SpriteTextureFactory(g)

        { leftJumpingSprite = f.createSpriteTexture ASSET_BONHOMME_LEFT_JUMPING_SPRITE
          rightJumpingSprite = f.createSpriteTexture ASSET_BONHOMME_RIGHT_JUMPING_SPRITE
          leftDuckSprite = f.createSpriteTexture ASSET_BONHOMME_LEFT_DUCK_SPRITE
          rightDuckSprite = f.createSpriteTexture ASSET_BONHOMME_RIGHT_DUCK_SPRITE
          rightIdleSprite = f.createSpriteTexture ASSET_BONHOMME_RIGHT_IDLE_SPRITE
          leftIdleSprite = f.createSpriteTexture ASSET_BONHOMME_LEFT_IDLE_SPRITE

          rightRunningSprites =
              f.createSpriteTextures [ ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_1
                                       ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_2 ]

          leftRunningSprites =
              f.createSpriteTextures [ ASSET_BONHOMME_LEFT_RUNNING_SPRITE_1
                                       ASSET_BONHOMME_LEFT_RUNNING_SPRITE_2 ] }



    let updateSprite gameTime (currentEntity: GameEntity) (props: BonhommeProperties) currentMovState nextMovState =

        let spSheet = props.spriteSheet

        let m = BonhommeSpriteManager(spSheet)


        let nextSprite =
            match (currentMovState, nextMovState) with
            | _, Jumping (Toward dir, _) -> m.jumpSprite dir
            | _, Jumping (Up dir, _) -> m.jumpSprite dir
            | _, Idle dir -> m.idleSprite dir
            | _, Duck dir -> m.duckSprite dir
            | Idle _, Running dir -> m.runningSprite dir
            | Duck _, Running dir -> m.runningSprite dir
            | Running Left, Running Right -> m.newAnimatedSprite spSheet.rightRunningSprites
            | Running Right, Running Left -> m.newAnimatedSprite spSheet.leftRunningSprites
            | Running _, Running _ -> currentEntity.properties.sprite
            | _ -> currentEntity.properties.sprite

        Sprites.updateSpriteState gameTime nextSprite
