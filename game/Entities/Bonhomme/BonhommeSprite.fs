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

    let nextSprite =
        match (prevMovState, currentMovState) with
        | _, Jumping dir ->

            let jumpingSprite =
                match dir with
                | Left -> properties.leftJumpingSprite
                | Right -> properties.rightJumpingSprite

            SingleSprite jumpingSprite

        | _, Inactive dir ->

            let staticSprite =
                match dir with
                | Left -> properties.leftStaticSprite
                | Right -> properties.rightStaticSprite

            SingleSprite staticSprite

        | _, Duck dir ->

            let duckSprite =
                match dir with
                | Left -> properties.leftDuckSprite
                | Right -> properties.rightDuckSprite

            SingleSprite duckSprite

        | Inactive _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> properties.leftRunningAnimatedSprite
                | Right -> properties.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Duck _, Running dir ->

            let runningSprite =
                match dir with
                | Left -> properties.leftRunningAnimatedSprite
                | Right -> properties.rightRunningAnimatedSprite

            createBonhommeAnimatedSprite runningSprite

        | Running Left, Running Right -> createBonhommeAnimatedSprite properties.rightRunningAnimatedSprite
        | Running Right, Running Left -> createBonhommeAnimatedSprite properties.leftRunningAnimatedSprite
        | Running _, Running _ -> currentGameEntity.Sprite
        | _ -> currentGameEntity.Sprite

    Sprites.updateSpriteState gameTime nextSprite
