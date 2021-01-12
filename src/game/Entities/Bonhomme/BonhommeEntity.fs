namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants
open Microsoft.Xna.Framework.Graphics

module BonhommeEntity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.BonhommeConstants



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity): GameEntity =

        match currentEntity with
        | Bonhomme props ->

            BonhommeUpdate.updateEntity gt gs currentEntity props

        | _ -> currentEntity



    let initEntity (g: Game) (gs: GameState) =

        let spSheet =
            BonhommeSprite.createBonhommeSpriteSheet g

        let bonhommePos =
            new Vector2(POSITION_X_STARTING, FLOOR_HEIGHT)

        let bonhommeProps =
            BonhommeProperties
                { movementStatus = Idle Right
                  spriteSheet = spSheet
                  position = bonhommePos }

        let spriteProps =
            { texture = spSheet.rightIdleSprite
              position = bonhommePos }

        let sprite = SingleSprite spriteProps


        { id = GameEntityId BonhommeConstants.EntityId
          isEnabled = true
          extendProperties = bonhommeProps
          sprite = sprite
          drawEntity = Sprites.drawEntity
          updateEntity = updateEntity }
