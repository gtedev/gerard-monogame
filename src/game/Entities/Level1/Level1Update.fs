namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open Microsoft.Xna.Framework.Input
open GerardMonogame.Constants

[<RequireQualifiedAccess>]
module Level1Update =
    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants.Level1Constants



    let private ``make sure level1 never moves too much to the right`` posX (vectorMov: Vector2) =

        let nextXPos =
            if posX + vectorMov.X > 0f then 0f else vectorMov.X

        new Vector2(nextXPos, vectorMov.Y)



    let private ``update level1 movement in opposite direction of bonhomme`` (allBonhommeProps: GameEntityProperties * BonhommeProperties)
                                                                             (vectorMov: Vector2)
                                                                             =
        let (bonhommEntityProps, bonhommeProps) = allBonhommeProps

        let idleLvl1Vector = new Vector2(0f, 0f)

        let ``and bonhomme is after minimum X Position`` =
            bonhommEntityProps.position.X > LEVEL1_BONHOMME_X_POSITION_MOVE_TRIGGER

        let nextMoveLvl1Vector dir =
            // move of the opposite direction
            let nextXMovPos = GameHelper.matchDirection dir (1f) (-1f)

            new Vector2(nextXMovPos * SPEED_MOVING_FLOOR, vectorMov.Y)


        match bonhommeProps.movementStatus with
        | Duck _ -> idleLvl1Vector
        | Jumping (Toward dir, _) when ``and bonhomme is after minimum X Position`` ->

            nextMoveLvl1Vector dir

        | Running dir when ``and bonhomme is after minimum X Position`` ->

            nextMoveLvl1Vector dir

        | _ -> idleLvl1Vector



    let private ``make sure level1 never moves vertically`` (vectorMov: Vector2) =
        // maintain level background to same Y position !
        new Vector2(vectorMov.X, 0f)



    let private updateLevel1Entity (allBonhommeProps: GameEntityProperties * BonhommeProperties)
                                   (currentEntity: GameEntity)
                                   (lvl1Props: Level1Properties)
                                   =
        let vectorMov =
            KeyboardState.getMovementVector (Keyboard.GetState())

        let nextVectorPos =
            vectorMov
            |> ``update level1 movement in opposite direction of bonhomme`` allBonhommeProps
            |> ``make sure level1 never moves vertically``
            |> ``make sure level1 never moves too much to the right`` currentEntity.properties.position.X

        let nextLvl1Props = Level1Properties(lvl1Props) |> Some

        let nextEntityProps =
            { currentEntity.properties with
                  position = Vector2.Add(currentEntity.properties.position, nextVectorPos) }

        currentEntity
        |> GameEntity.updateEntity nextEntityProps nextLvl1Props



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: GameEntity) (lvl1Props: Level1Properties) =

        let bhEntity =
            GameEntity.tryGetEntity gs BonhommeConstants.EntityId

        match bhEntity with
        | SomeBonhomme allBonhommeProps ->

            updateLevel1Entity allBonhommeProps currentEntity lvl1Props

        | _ -> currentEntity
