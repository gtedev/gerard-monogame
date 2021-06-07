namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open Microsoft.Xna.Framework.Input
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module MechantUpdate =
    open Microsoft.Xna.Framework
    open Types



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity) (mechantProps: MechantProperties) =

        let newPosition =
            new Vector2(mechantProps.position.X - 1f, mechantProps.position.Y)

        let newProps =
            { mechantProps with
                  position = newPosition }

        let nextSprite =
            SingleSprite
                { position = newPosition
                  texture = mechantProps.spriteSheet.mechantSprite }

        { currentEntity with
              sprite = nextSprite
              extendProperties = MechantProperties newProps }
