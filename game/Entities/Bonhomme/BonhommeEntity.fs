module BonhommeEntity

open Microsoft.Xna.Framework
open GameTypes


let updateEntity (gameTime: GameTime) (gameState: GameState) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (BonhommeProperties properties) -> BonhommeUpdate.updateEntity gameTime gameState currentGameEntity properties

    | _ -> currentGameEntity


let initializeEntity (game: Game)(gameState: GameState) =

    let spriteSheet =
        BonhommeSprite.createBonhommeSpriteSheet game

    let bonhommeProperties =
        BonhommeProperties
            { movementStatus = Idle Right
              spriteSheet = spriteSheet }
        |> Some

    let properties =
        { id = BonhommeConstants.EntityId
          position = new Vector2(BonhommeConstants.POSITION_X_STARTING, BonhommeConstants.FLOOR_HEIGHT)
          sprite = SingleSprite spriteSheet.rightIdleSprite
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties updateEntity
