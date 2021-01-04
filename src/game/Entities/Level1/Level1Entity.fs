namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module Level1Entity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.Level1Constants



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity): GameEntity =

        match currentEntity with
        | Level1 allEntityProps ->

            let lvl1Props = snd allEntityProps
            Level1Update.updateEntity gt gs currentEntity lvl1Props

        | _ -> currentEntity



    let initEntity (game: Game) (gs: GameState) =

        let spSheet =
            Level1Sprite.createLevel1SpriteSheet game

        let lvl1Props =
            Level1Properties { spriteSheet = spSheet }

        let entityProps =
            { id = Level1Constants.EntityId
              position = new Vector2(0f, LEVEL1_Y_POSITION)
              sprite = SingleSprite spSheet.level1Sprite
              isEnabled = true }

        GameEntity.createEntity entityProps lvl1Props updateEntity
