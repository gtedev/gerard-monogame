namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module BonhommeSprite =

    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.BonhommeConstants



    let createBonhommeSpriteSheet (g: Game) =

        let createTexture = Sprites.createSpriteTexture g

        let createTextures = Sprites.createSpriteTextures g

        { leftJumpingSprite = createTexture ASSET_BONHOMME_LEFT_JUMPING_SPRITE
          rightJumpingSprite = createTexture ASSET_BONHOMME_RIGHT_JUMPING_SPRITE
          leftDuckSprite = createTexture ASSET_BONHOMME_LEFT_DUCK_SPRITE
          rightDuckSprite = createTexture ASSET_BONHOMME_RIGHT_DUCK_SPRITE
          rightIdleSprite = createTexture ASSET_BONHOMME_RIGHT_IDLE_SPRITE
          leftIdleSprite = createTexture ASSET_BONHOMME_LEFT_IDLE_SPRITE

          rightRunningSprites =
              createTextures [ ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_1
                               ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_2 ]

          leftRunningSprites =
              createTextures [ ASSET_BONHOMME_LEFT_RUNNING_SPRITE_1
                               ASSET_BONHOMME_LEFT_RUNNING_SPRITE_2 ] }



    let private newAnimatedSprite =
        Sprites.createAnimatedSprite BonhommeConstants.ANIMATION_FRAME_TIME



    let updateSprite gameTime (currentEntity: GameEntity) (props: BonhommeProperties) currentMovState nextMovState =

        let spSheet = props.spriteSheet

        let jumpSprite dir =
            GameHelper.matchDirection dir spSheet.leftJumpingSprite spSheet.rightJumpingSprite
            |> SingleSprite

        let runningSprite dir =
            GameHelper.matchDirection dir spSheet.leftRunningSprites spSheet.rightRunningSprites
            |> newAnimatedSprite

        let duckSprite dir =
            GameHelper.matchDirection dir spSheet.leftDuckSprite spSheet.rightDuckSprite
            |> SingleSprite

        let idleSprite dir =
            GameHelper.matchDirection dir spSheet.leftIdleSprite spSheet.rightIdleSprite
            |> SingleSprite


        let nextSprite =
            match (currentMovState, nextMovState) with
            | _, Jumping (Toward dir, _) -> jumpSprite dir
            | _, Jumping (Up dir, _) -> jumpSprite dir
            | _, Idle dir -> idleSprite dir
            | _, Duck dir -> duckSprite dir
            | Idle _, Running dir -> runningSprite dir
            | Duck _, Running dir -> runningSprite dir
            | Running Left, Running Right -> newAnimatedSprite spSheet.rightRunningSprites
            | Running Right, Running Left -> newAnimatedSprite spSheet.leftRunningSprites
            | Running _, Running _ -> currentEntity.properties.sprite
            | _ -> currentEntity.properties.sprite

        Sprites.updateSpriteState gameTime nextSprite
