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
            match (currentMovState, nextMovState) with
            | _, Jumping (Toward dir, _) -> m.jumpSprite dir
            | _, Jumping (Up dir, _) -> m.jumpSprite dir
            | _, Idle dir -> m.idleSprite dir
            | _, Duck dir -> m.duckSprite dir
            | Idle _, Running dir -> m.runningSprite dir
            | Duck _, Running dir -> m.runningSprite dir
            | Running Left, Running Right -> m.newAnimatedSprite spSheet.rightRunningSprites
            | Running Right, Running Left -> m.newAnimatedSprite spSheet.leftRunningSprites
            | Running _, Running _ ->

                Sprites.tryUpdateAnimatedSprite gameTime currentEntity.sprite nextPosition

            | _ -> currentEntity.sprite


        nextSprite
