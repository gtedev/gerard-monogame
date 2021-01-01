module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open FSharp.Core.Extensions

type GameEntityId = string
type CurrentSpriteIndex = int
type ElapsedTimeSinceLastFrame = float32
type CurrentJumpVelocity = float32

type SpriteTexture = { texture: Texture2D }


type AnimatedSpriteState =
    { sprites: SpriteTexture list
      currentSpriteIndex: CurrentSpriteIndex
      elapsedTimeSinceLastFrame: ElapsedTimeSinceLastFrame
      frameTime: float32 }


type Sprite =
    | SingleSprite of SpriteTexture
    | AnimatedSprite of AnimatedSpriteState


type GameEntityProperties =
    { id: GameEntityId
      isEnabled: bool
      position: Vector2
      sprite: Sprite }

type Direction =
    | Left
    | Right

type BonhommeMovemementState =
    | Idle of Direction
    | Running of Direction
    | Jumping of Direction * CurrentJumpVelocity
    | Duck of Direction

type BonhommeSpriteSheet =
    { rightIdleSprite: SpriteTexture
      leftIdleSprite: SpriteTexture
      rightDuckSprite: SpriteTexture
      leftDuckSprite: SpriteTexture
      rightJumpingSprite: SpriteTexture
      leftJumpingSprite: SpriteTexture
      rightRunningSprites: SpriteTexture list
      leftRunningSprites: SpriteTexture list }

type Level1SpriteSheet = { level1Sprite: SpriteTexture }


type BonhommeProperties =
    { movementStatus: BonhommeMovemementState
      spriteSheet: BonhommeSpriteSheet }


type Level1Properties = { spriteSheet: Level1SpriteSheet }


type CustomEntityProperties =
    | BonhommeProperties of BonhommeProperties
    | Level1Properties of Level1Properties


// GameState<'TEntity> has been just introduced in order to be able to reference GameState in IGameEntity.UpdateEntity
// for at least to having something typed.
// As F# implies strict declaration order, it was not possible to reference GameState before.
type GameState<'TEntity> =
    { entities: readonlydict<GameEntityId, 'TEntity> }

type IGameEntity =
    abstract CustomEntityProperties: CustomEntityProperties option
    abstract Properties: GameEntityProperties
    abstract UpdateEntity: GameTime -> GameState<IGameEntity> -> IGameEntity -> IGameEntity
    abstract Position: Vector2
    abstract Sprite: Sprite


type GameState = GameState<IGameEntity>
