namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module BonhommeSprite =

    open Types
    open Microsoft.Xna.Framework
    open Microsoft.Xna.Framework.Graphics
    open GerardMonogame.Constants.BonhommeConstants

    let private createBonhommeAnimatedSprite =
        Sprites.createAnimatedSprite BonhommeConstants.ANIMATION_FRAME_TIME



    let createBonhommeSpriteSheet (g: Game) =

        let rightJmpTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_JUMPING_SPRITE) }

        let leftJmpTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_JUMPING_SPRITE) }

        let leftIdleTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_IDLE_SPRITE) }

        let rightIdleTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_IDLE_SPRITE) }

        let leftDuckTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_DUCK_SPRITE) }

        let rightDuckTxture =
            { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_DUCK_SPRITE) }

        let rightRunningTxtures =
            [ { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_1) }
              { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_2) } ]

        let leftRunningTxtures =
            [ { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_1) }
              { texture = g.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_2) } ]

        { leftJumpingSprite = leftJmpTxture
          rightJumpingSprite = rightJmpTxture
          leftDuckSprite = leftDuckTxture
          rightDuckSprite = rightDuckTxture
          rightIdleSprite = rightIdleTxture
          leftIdleSprite = leftIdleTxture
          rightRunningSprites = rightRunningTxtures
          leftRunningSprites = leftRunningTxtures }



    let updateSprite gameTime (currentEntity: IGameEntity) (props: BonhommeProperties) prevMovState currentMovState =

        let ss = props.spriteSheet

        let nextSprite =
            match (prevMovState, currentMovState) with
            | _, Jumping (dir, _) ->

                GameHelper.matchDirection dir ss.leftJumpingSprite ss.rightJumpingSprite
                |> SingleSprite

            | _, Idle dir ->

                GameHelper.matchDirection dir ss.leftIdleSprite ss.rightIdleSprite
                |> SingleSprite

            | _, Duck dir ->

                GameHelper.matchDirection dir ss.leftDuckSprite ss.rightDuckSprite
                |> SingleSprite

            | Idle _, Running dir ->

                GameHelper.matchDirection dir ss.leftRunningSprites ss.rightRunningSprites
                |> createBonhommeAnimatedSprite

            | Duck _, Running dir ->

                GameHelper.matchDirection dir ss.leftRunningSprites ss.rightRunningSprites
                |> createBonhommeAnimatedSprite

            | Running Left, Running Right -> createBonhommeAnimatedSprite ss.rightRunningSprites
            | Running Right, Running Left -> createBonhommeAnimatedSprite ss.leftRunningSprites
            | Running _, Running _ -> currentEntity.Sprite
            | _ -> currentEntity.Sprite

        Sprites.updateSpriteState gameTime nextSprite
