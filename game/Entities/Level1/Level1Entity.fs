namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

module Level1Entity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants

    let updateEntity (gameTime: GameTime) (gameState: GameState) (currentGameEntity: IGameEntity): IGameEntity =

        match currentGameEntity with
        | Level1 allEntityProperties ->

            let level1Properties = snd allEntityProperties
            Level1Update.updateEntity gameTime gameState currentGameEntity level1Properties

        | _ -> currentGameEntity



    let initializeEntity (game: Game) (gameState: GameState) =

        let spriteSheet =
            Level1Sprite.createLevel1SpriteSheet game

        let level1Properties =
            Level1Properties { spriteSheet = spriteSheet }
            |> Some

        let properties =
            { id = Level1Constants.EntityId
              position = new Vector2(0f, Level1Constants.LEVEL1_Y_POSITION)
              sprite = SingleSprite spriteSheet.level1Sprite
              isEnabled = true }

        GameEntity.createGameEntity properties level1Properties updateEntity
