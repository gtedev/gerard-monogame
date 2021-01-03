namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module BonhommeEntity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.BonhommeConstants



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity): GameEntity =

        match currentEntity with
        | Bonhomme allEntityProps ->

            let bonhommeProps = snd allEntityProps
            BonhommeUpdate.updateEntity gt gs currentEntity bonhommeProps

        | _ -> currentEntity



    let initEntity (g: Game) (gs: GameState) =

        let spSheet =
            BonhommeSprite.createBonhommeSpriteSheet g

        let bonhommeProps =
            BonhommeProperties
                { movementStatus = Idle Right
                  spriteSheet = spSheet }
            |> Some

        let entityProps =
            { id = BonhommeConstants.EntityId
              position = new Vector2(POSITION_X_STARTING, FLOOR_HEIGHT)
              sprite = SingleSprite spSheet.rightIdleSprite
              isEnabled = true }

        GameEntity.createEntity entityProps bonhommeProps updateEntity
