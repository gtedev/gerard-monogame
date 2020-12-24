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

    let rightJumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_JUMPING_SPRITE) }
        
    let leftJumpingTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_JUMPING_SPRITE) }

    let leftStaticSprite =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_STATIC_SPRITE) }

    let rightStaticTexture =
        { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_STATIC_SPRITE) }

    let rightRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_RIGHT_RUNNING_SPRITE_2) } ]

    let leftRunningTextures =
        [ { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_1) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_LEFT_RUNNING_SPRITE_2) } ]

    let bonhommeProperties =
        BonhommeProperties
            { jumpVelocityState = None
              movementStatus = Inactive Right
              leftJumpingSprite = leftJumpingTexture
              rightJumpingSprite = rightJumpingTexture
              rightStaticSprite = rightStaticTexture
              leftStaticSprite = leftStaticSprite
              rightRunningAnimatedSprite = rightRunningTextures
              leftRunningAnimatedSprite = leftRunningTextures }
        |> Some

    let properties =
        { position = new Vector2(0f, 350f)
          sprite = SingleSprite rightStaticTexture
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties update
