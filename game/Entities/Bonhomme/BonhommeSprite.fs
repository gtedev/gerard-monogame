[<RequireQualifiedAccess>]
module BonhommeSprite

open Types
open BonhommeConstants

let private createBonhommeAnimatedSprite =
    Sprites.createAnimatedSprite ANIMATION_FRAME_TIME


let updateSprite gameTime
                 (currentGameEntity: IGameEntity)
                 (properties: BonhommeProperties)
                 prevMovState
                 currentMovState
                 =

    let spriteSheet = properties.spriteSheet

    let nextSprite =
        match (prevMovState, currentMovState) with
        | _, Jumping dir ->

            let jumpingSprite =
                match dir with
                | Left -> spriteSheet.leftJumpingSprite
                | Right -> spriteSheet.rightJumpingSprite

            SingleSprite jumpingSprite

        | _, Inactive dir ->

            let staticSprite =
                match dir with
                | Left -> spriteSheet.leftStaticSprite
                | Right -> spriteSheet.rightStaticSprite

            SingleSprite staticSprite

        | _, Duck dir ->

            let duckSprite =
                match dir with
                | Left -> spriteSheet.leftDuckSprite
                | Right -> spriteSheet.rightDuckSprite

            SingleSprite duckSprite

        | Inactive _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> spriteSheet.leftRunningAnimatedSprite
                | Right -> spriteSheet.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Duck _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> spriteSheet.leftRunningAnimatedSprite
                | Right -> spriteSheet.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Running Left, Running Right -> createBonhommeAnimatedSprite spriteSheet.rightRunningAnimatedSprite
        | Running Right, Running Left -> createBonhommeAnimatedSprite spriteSheet.leftRunningAnimatedSprite
        | Running _, Running _ -> currentGameEntity.Sprite
        | _ -> currentGameEntity.Sprite

    Sprites.updateSpriteState gameTime nextSprite
