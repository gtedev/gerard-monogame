module BonhommeEntity

open Microsoft.Xna.Framework
open Types


let updateEntity (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (BonhommeProperties properties) -> BonhommeUpdate.updateEntity gameTime currentGameEntity properties

    | _ -> currentGameEntity


let initializeEntity (game: Game) =

    let spriteSheet =
        BonhommeSprite.createBonhommeSpriteSheet game

    let bonhommeProperties =
        BonhommeProperties
            { movementStatus = Idle Right
              spriteSheet = spriteSheet }
        |> Some

    let properties =
        { position = new Vector2(BonhommeConstants.POSITION_X_STARTING, BonhommeConstants.FLOOR_HEIGHT)
          sprite = SingleSprite spriteSheet.rightIdleSprite
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties updateEntity
