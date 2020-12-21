﻿module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type CurrentSpriteIndex = int
type ElapsedTimeSinceLastFrame = float32
type CurrentJumpYPosition = float32
type CurrentJumpVelocity = float32

type SpriteTexture = { texture: Texture2D }


type AnimatedSpriteState =
    { sprites: SpriteTexture list
      currentSpriteIndex: CurrentSpriteIndex
      elapsedTimeSinceLastFrame: ElapsedTimeSinceLastFrame
      animatedFrameTime: float32 }


type Sprite =
    | SingleSprite of SpriteTexture
    | AnimatedSprite of AnimatedSpriteState


type GameEntityProperties =
    { isEnabled: bool
      position: Vector2
      sprite: Sprite }

type JumpType =
    | Ascending
    | Descending

type JumpState =
    { jumpType: JumpType
      //currentJumpYPosition: CurrentJumpYPosition
      currentJumpVelocity: CurrentJumpVelocity }

type BonhommeMovemementState =
    | Inactive
    | Running
    | Jumping


type BonhommeProperties =
    { jumpState: CurrentJumpVelocity option
      movementStatus: BonhommeMovemementState
      staticSprite: SpriteTexture
      runningAnimatedSprite: SpriteTexture list }


type CustomEntityProperties = BonhommeProperties of BonhommeProperties


type IGameEntity =
    abstract CustomEntityProperties: CustomEntityProperties option
    abstract Properties: GameEntityProperties
    abstract UpdateEntity: GameTime -> IGameEntity -> IGameEntity
    abstract Position: Vector2
    abstract Sprite: Sprite


type GameState = { entities: IGameEntity list }
