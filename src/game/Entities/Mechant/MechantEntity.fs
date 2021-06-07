namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open GerardMonogame.Constants

module MechantEntity =

    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.MechantConstants



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity): GameEntity =

        match currentEntity with
        | Mechant mechantProps ->

            MechantUpdate.updateEntity gt gs currentEntity mechantProps

        | _ -> currentEntity



    let initEntity (g: Game) (gs: GameState) =

        let spSheet = MechantSprite.createMechantSpriteSheet g

        let mechantPos =
            new Vector2(POSITION_X_STARTING, FLOOR_HEIGHT)

        let mechantProps =
            MechantProperties
                { position = mechantPos
                  spriteSheet = spSheet }

        let spriteProps =
            { texture = spSheet.mechantSprite
              position = mechantPos }

        { id = GameEntityId MechantConstants.EntityId
          isEnabled = true
          extendProperties = mechantProps
          sprite = SingleSprite spriteProps
          drawEntity = Sprites.drawEntity
          updateEntity = updateEntity }
