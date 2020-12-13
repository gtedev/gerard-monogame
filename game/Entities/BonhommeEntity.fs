module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE1 = "bonhomme32-2-piskel"
let ASSET_BONHOMME_SPRITE2 = "bonhomme32-2-piskel"
let ASSET_BONHOMME_SPRITE3 = "bonhomme62-piskel"

let SPEED_BONHOMME_SPRITE = 2f
let ANIMATION_FRAME_TIME = 1f / 6f

let updateEntity gameTime (currentGameEntity: IGameEntity) (properties:BonhommeProperties) = 

    let vectorMovement =
        KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE

    let wasRunning = properties.isRunning
    let isRunning = if vectorMovement.X = 0f && vectorMovement.Y = 0f then false else true
    let newProperties = { properties with isRunning = isRunning }
    let bonhommeProperties = Some (BonhommeProperties newProperties)


    let spriteToPass = if not wasRunning && isRunning then 
                        AnimatedSprite
                            { sprites = properties.runningAnimatedSprite
                              currentSpriteIndex = 0
                              elapsedTimeSinceLastFrame = 0f }
                       elif wasRunning && isRunning then 
                        currentGameEntity.Sprite 
                       elif wasRunning && not isRunning then 
                        SingleSprite properties.staticSprite 
                       else  
                        SingleSprite properties.staticSprite 

    let nextSprite =  Sprites.updateSpriteState gameTime spriteToPass ANIMATION_FRAME_TIME

    let newVector =
        Vector2.Add(currentGameEntity.Position, vectorMovement)


    let properties =
        { currentGameEntity.Properties with
              position = newVector
              sprite = nextSprite }
    
    GameEntity.createGameEntity properties bonhommeProperties currentGameEntity.UpdateEntity


let update (gameTime: GameTime) (currentGameEntity: IGameEntity): IGameEntity =

    match currentGameEntity.CustomEntityProperties with 
    | Some (BonhommeProperties properties) ->
        updateEntity gameTime currentGameEntity properties

    |_ -> currentGameEntity


let initializeEntity (game: Game) =

    let staticTexture ={ texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE1) }

    let animatedRunningTextures =
        [
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE2) }
          { texture = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE3) }]

    let bonhommeProperties = BonhommeProperties { isRunning = false 
                                                  staticSprite = staticTexture 
                                                  runningAnimatedSprite = animatedRunningTextures  } |> Some

    let properties =
        { position = new Vector2(0f, 350f)
          sprite = SingleSprite staticTexture          
          isEnabled = true }

    GameEntity.createGameEntity properties bonhommeProperties update
