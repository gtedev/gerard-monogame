module BonhommeSprite

open Types
open BonhommeConstants

let updateSprite gameTime
                (currentGameEntity: IGameEntity)
                (properties: BonhommeProperties)
                previousMovement
                currentMovement =

    let spriteToPass =
        match (previousMovement, currentMovement) with
        | _, Jumping -> SingleSprite properties.jumpingSprite
        | Inactive, Running ->
            AnimatedSprite
                { sprites = properties.runningAnimatedSprite
                  currentSpriteIndex = 0
                  elapsedTimeSinceLastFrame = 0f
                  animatedFrameTime = ANIMATION_FRAME_TIME }
        | Running, Running -> currentGameEntity.Sprite
        | Running, Inactive -> SingleSprite properties.staticSprite
        | _ -> SingleSprite properties.staticSprite

    Sprites.updateSpriteState gameTime spriteToPass

