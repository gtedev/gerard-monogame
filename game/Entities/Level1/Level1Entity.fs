module Level1Entity

open Microsoft.Xna.Framework
open Types


let updateEntity (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (Level1Properties properties) -> Level1Update.updateEntity gameTime currentGameEntity properties

    | _ -> currentGameEntity


let initializeEntity (game: Game) =

    let spriteSheet =
        Level1Sprite.createLevel1SpriteSheet game

    let level1Properties =
         Level1Properties
            { spriteSheet = spriteSheet }
        |> Some

    let properties =
        { position = new Vector2(0f, Level1Constants.LEVEL1_Y_POSITION)
          sprite = SingleSprite spriteSheet.level1Sprite
          isEnabled = true }

    GameEntity.createGameEntity properties level1Properties updateEntity
