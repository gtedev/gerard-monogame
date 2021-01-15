namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module BonhommeSprite =

    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.BonhommeConstants



    type BonhommeSpriteManager(spriteSheet, position) =

        let _newSingleSprite texture =
            { texture = texture
              position = position }
            |> SingleSprite

        let _newAnimatedSprite =
            Sprites.createAnimatedSprite BonhommeConstants.ANIMATION_FRAME_TIME position

        let _jumpSprite dir =

            dir
            |> GameHelper.matchDirection spriteSheet.leftJumpingSprite spriteSheet.rightJumpingSprite
            |> _newSingleSprite

        let _runningSprite dir =

            dir
            |> GameHelper.matchDirection spriteSheet.leftRunningSprites spriteSheet.rightRunningSprites
            |> _newAnimatedSprite

        let _duckSprite dir =
            dir
            |> GameHelper.matchDirection spriteSheet.leftDuckSprite spriteSheet.rightDuckSprite
            |> _newSingleSprite

        let _idleSprite dir =
            dir
            |> GameHelper.matchDirection spriteSheet.leftIdleSprite spriteSheet.rightIdleSprite
            |> _newSingleSprite

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



    let updateSprite gameTime
                     (currentEntity: GameEntity)
                     (props: BonhommeProperties)
                     currentMovState
                     nextMovState
                     nextPosition
                     =

        let spSheet = props.spriteSheet

        let m =
            BonhommeSpriteManager(spSheet, nextPosition)


        let nextSprite =

            match (currentMovState, nextMovState, currentEntity.sprite) with

            | _____________, Jumping (Toward dir, _), __________ -> m.jumpSprite dir
            | _____________, Jumping (Up dir, _), ______________ -> m.jumpSprite dir
            | _____________, Idle dir, _________________________ -> m.idleSprite dir
            | _____________, Duck dir, _________________________ -> m.duckSprite dir
            | Idle ________, Running dir, ______________________ -> m.runningSprite dir
            | Duck ________, Running dir, ______________________ -> m.runningSprite dir
            | Running Left, Running Right, _____________________ -> m.newAnimatedSprite spSheet.rightRunningSprites
            | Running Right, Running Left, _____________________ -> m.newAnimatedSprite spSheet.leftRunningSprites
            | Running _____, Running _, AnimatedSprite animProps ->

                Sprites.updateAnimatedSprite gameTime animProps nextPosition

            | _ -> currentEntity.sprite


        nextSprite
