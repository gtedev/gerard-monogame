namespace GerardMonogame.Game

module Types =


    open Microsoft.Xna.Framework
    open Microsoft.Xna.Framework.Graphics
    open FSharp.Core.Extensions


    type GameEntityId = GameEntityId of string
    type CurrentSpriteIndex = CurrentSpriteIndex of int
    type ElapsedTimeSinceLastFrame = ElapsedTimeSinceLastFrame of float32
    type CurrentJumpVelocity = CurrentJumpVelocity of float32
    type SpritePosition = Vector2


    type SingleSpriteProperties =
        { texture: Texture2D
          position: SpritePosition }

    type AnimatedSpriteProperties =
        { sprites: Texture2D list
          position: SpritePosition
          currentSpriteIndex: CurrentSpriteIndex
          elapsedTimeSinceLastFrame: ElapsedTimeSinceLastFrame
          frameTime: float32 }

    type Sprite =
        | SingleSprite of SingleSpriteProperties
        | AnimatedSprite of AnimatedSpriteProperties
        | GroupOfSprites of SingleSpriteProperties list


    type Direction =
        | Left
        | Right

    type JumpDirection =
        | Toward of Direction
        | Up of Direction

    type BonhommeMovemementState =
        | Idle of Direction
        | Running of Direction
        | Jumping of JumpDirection * CurrentJumpVelocity
        | Duck of Direction

    type BonhommeSpriteSheet =
        { rightIdleSprite: Texture2D
          leftIdleSprite: Texture2D
          rightDuckSprite: Texture2D
          leftDuckSprite: Texture2D
          rightJumpingSprite: Texture2D
          leftJumpingSprite: Texture2D
          rightRunningSprites: Texture2D list
          leftRunningSprites: Texture2D list }

    type Level1SpriteSheet = { level1Sprite: Texture2D }


    type BonhommeProperties =
        { movementStatus: BonhommeMovemementState
          spriteSheet: BonhommeSpriteSheet
          position: SpritePosition }


    type Level1Properties =
        { spriteSheet: Level1SpriteSheet
          position: SpritePosition }


    type ExtendProperties =
        | BonhommeProperties of BonhommeProperties
        | Level1Properties of Level1Properties


    type GameEntity =
        { id: GameEntityId
          sprite: Sprite
          isEnabled: bool
          extendProperties: ExtendProperties
          updateEntity: GameTime -> GameState -> GameEntity -> GameEntity
          drawEntity: SpriteService -> GameEntity -> unit }

    and GameState =
        { entities: readonlydict<GameEntityId, GameEntity> }

    /// Service that contains bunch of Monogame singletons
    and SpriteService =
        { spriteBatch: SpriteBatch
          spriteFont: SpriteFont }
