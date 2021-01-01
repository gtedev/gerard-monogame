namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module BonhommeEntity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.BonhommeConstants

    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: IGameEntity): IGameEntity =

        match currentEntity with
        | Bonhomme allEntityProps ->

            let bonhommeProps = snd allEntityProps
            BonhommeUpdate.updateEntity gt gs currentEntity bonhommeProps

        | _ -> currentEntity



    let initEntity (g: Game) (gs: GameState) =

        let ss =
            BonhommeSprite.createBonhommeSpriteSheet g

        let bonhommeProperties =
            BonhommeProperties
                { movementStatus = Idle Right
                  spriteSheet = ss }
            |> Some

        let geProps =
            { id = BonhommeConstants.EntityId
              position = new Vector2(POSITION_X_STARTING, FLOOR_HEIGHT)
              sprite = SingleSprite ss.rightIdleSprite
              isEnabled = true }

        GameEntity.createGameEntity geProps bonhommeProperties updateEntity
