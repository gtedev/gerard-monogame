[<RequireQualifiedAccess>]
module BonhommeSprite

open Types
open BonhommeConstants

let private createBonhommeAnimatedSprite =
    Sprites.createAnimatedSprite ANIMATION_FRAME_TIME

let updateSprite gameTime
                 (currentGameEntity: IGameEntity)
                 (properties: BonhommeProperties)
                 previousMovementState
                 currentMovementState
                 =

    let spriteToPass =
        match (previousMovementState, currentMovementState) with
        | _, Jumping -> SingleSprite properties.jumpingSprite
        | Jumping, Inactive -> SingleSprite properties.rightStaticSprite
        | Inactive, Running direction ->

            let runningSprite =
                match direction with
                | Left -> properties.leftRunningAnimatedSprite
                | Right -> properties.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Running Left, Running Right -> createBonhommeAnimatedSprite properties.rightRunningAnimatedSprite
        | Running Right, Running Left -> createBonhommeAnimatedSprite properties.leftRunningAnimatedSprite
        | Running _, Running _ -> currentGameEntity.Sprite
        | Running prevDir, Inactive ->

            let staticSprite =
                match prevDir with
                | Left -> properties.leftStaticSprite
                | Right -> properties.rightStaticSprite

            SingleSprite staticSprite
        | _ -> currentGameEntity.Sprite

    Sprites.updateSpriteState gameTime spriteToPass
