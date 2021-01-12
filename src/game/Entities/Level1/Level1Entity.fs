namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module Level1Entity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.Level1Constants



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity): GameEntity =

        match currentEntity with
        | Level1 lvl1Props ->

            Level1Update.updateEntity gt gs currentEntity lvl1Props

        | _ -> currentEntity



    let initEntity (game: Game) (gs: GameState) =

        let spSheet =
            Level1Sprite.createLevel1SpriteSheet game

        let lv1Pos = new Vector2(0f, LEVEL1_Y_POSITION)

        let lvl1Props =
            Level1Properties
                { spriteSheet = spSheet
                  position = lv1Pos }

        let spriteProps =
            { texture = spSheet.level1Sprite
              position = lv1Pos }

        { id = GameEntityId Level1Constants.EntityId
          sprite = SingleSprite spriteProps
          extendProperties = lvl1Props
          updateEntity = updateEntity
          drawEntity = Sprites.drawEntity
          isEnabled = true }
