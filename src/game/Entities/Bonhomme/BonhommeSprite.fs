namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module BonhommeSprite =

    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.BonhommeConstants



    let private createBonhommeAnimatedSprite =
        Sprites.createAnimatedSprite BonhommeConstants.ANIMATION_FRAME_TIME



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



    let updateSprite gameTime (currentEntity: IGameEntity) (props: BonhommeProperties) currentMovState nextMovState =

        let spSheet = props.spriteSheet

        let nextSprite =
            match (currentMovState, nextMovState) with
            | _, Jumping (Toward dir, _) ->

                GameHelper.matchDirection dir spSheet.leftJumpingSprite spSheet.rightJumpingSprite
                |> SingleSprite

            | _, Jumping (Up dir, _) ->

                GameHelper.matchDirection dir spSheet.leftJumpingSprite spSheet.rightJumpingSprite
                |> SingleSprite

            | _, Idle dir ->

                GameHelper.matchDirection dir spSheet.leftIdleSprite spSheet.rightIdleSprite
                |> SingleSprite

            | _, Duck dir ->

                GameHelper.matchDirection dir spSheet.leftDuckSprite spSheet.rightDuckSprite
                |> SingleSprite

            | Idle _, Running dir ->

                GameHelper.matchDirection dir spSheet.leftRunningSprites spSheet.rightRunningSprites
                |> createBonhommeAnimatedSprite

            | Duck _, Running dir ->

                GameHelper.matchDirection dir spSheet.leftRunningSprites spSheet.rightRunningSprites
                |> createBonhommeAnimatedSprite

            | Running Left, Running Right ->

                createBonhommeAnimatedSprite spSheet.rightRunningSprites

            | Running Right, Running Left ->

                createBonhommeAnimatedSprite spSheet.leftRunningSprites

            | Running _, Running _ ->

                currentEntity.Sprite

            | _ -> currentEntity.Sprite

        Sprites.updateSpriteState gameTime nextSprite
