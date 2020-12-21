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

    let newProperties =
        { properties with
              movementStatus = nextMovementState
              jumpState = nextJumpVelocity }

    let nextBonhommeProperties = Some(BonhommeProperties newProperties)

    let newVector =
        Vector2.Add(currentGameEntity.Position, nextPositionMovement)

    let newSprite =
        BonhommeSprite.updateSprite gameTime currentGameEntity properties previousMovement nextMovementState

    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = newSprite }

    GameEntity.createGameEntity properties nextBonhommeProperties currentGameEntity.UpdateEntity


let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with
    | Some (BonhommeProperties properties) -> updateEntity gameTime currentGameEntity properties

    | _ -> currentGameEntity


let initializeEntity (game: Game) =

    let jumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_JUMPING) }

    let staticTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }

    let animatedRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE3) } ]

    let bonhommeProperties =
        BonhommeProperties
            { jumpState = None
              movementStatus = Inactive
              jumpingSprite = jumpingTexture
              staticSprite = staticTexture
              runningAnimatedSprite = animatedRunningTextures }
        |> Some

    let properties =
        { position = new Vector2(0f, 350f)
          sprite = SingleSprite staticTexture
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties update
