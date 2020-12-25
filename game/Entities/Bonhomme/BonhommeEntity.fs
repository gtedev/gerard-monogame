module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics
open BonhommeConstants


let updateEntity gameTime (currentGameEntity: IGameEntity) (properties: BonhommeProperties) =

    let vectorMovement =
        KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

    let previousMovement = properties.movementStatus

    let (nextMovementState, nextPositionMovement, nextJumpVelocity) =
        BonhommeUpdate.updateBonhommeStateAndPosition gameTime currentGameEntity.Properties properties vectorMovement

    let newSprite =
        BonhommeSprite.updateSprite gameTime currentGameEntity properties previousMovement nextMovementState

    let newProperties =
        { properties with
              movementStatus = nextMovementState
              jumpVelocityState = nextJumpVelocity }

    let nextBonhommeProperties = Some(BonhommeProperties newProperties)

    let newVector =
        Vector2.Add(currentGameEntity.Position, nextPositionMovement)

    let nextGameEntityProperties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = newSprite }

    GameEntity.createGameEntity nextGameEntityProperties nextBonhommeProperties currentGameEntity.UpdateEntity


let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (BonhommeProperties properties) -> updateEntity gameTime currentGameEntity properties

    | _ -> currentGameEntity


let initializeEntity (game: Game) =

    let spriteSheet =
        BonhommeSprite.createBonhommeSpriteSheet game

    let bonhommeProperties =
        BonhommeProperties
            { jumpVelocityState = None
              movementStatus = Inactive Right
              spriteSheet = spriteSheet }
        |> Some

    let properties =
        { position = new Vector2(0f, 350f)
          sprite = SingleSprite spriteSheet.rightStaticSprite
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties update
