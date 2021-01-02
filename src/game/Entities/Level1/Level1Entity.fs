namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module Level1Entity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.Level1Constants

    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: IGameEntity): IGameEntity =

        match currentEntity with
        | Level1 allEntityProps ->

            let lvl1Props = snd allEntityProps
            Level1Update.updateEntity gt gs currentEntity lvl1Props

        | _ -> currentEntity



    let initEntity (game: Game) (gs: GameState) =

        let ss =
            Level1Sprite.createLevel1SpriteSheet game

        let lvl1Props =
            Level1Properties { spriteSheet = ss } |> Some

        let geProps =
            { id = Level1Constants.EntityId
              position = new Vector2(0f, LEVEL1_Y_POSITION)
              sprite = SingleSprite ss.level1Sprite
              isEnabled = true }

        GameEntity.createGameEntity geProps lvl1Props updateEntity
