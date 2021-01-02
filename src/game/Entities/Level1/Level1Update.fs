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



    let private ``update level1 movement from right to left`` (allBonhommeProps: GameEntityProperties * BonhommeProperties)
                                                              (vectorMov: Vector2)
                                                              =
        let (entityProps, bonhommeProps) = allBonhommeProps

        let bonhommePosX = entityProps.position.X
        let idleLvl1Vector = new Vector2(0f, 0f)

        let moveLvl1Vector =
            new Vector2(vectorMov.X * (-1f) * SPEED_MOVING_FLOOR, vectorMov.Y)

        match bonhommeProps.movementStatus with
        | Duck _ -> idleLvl1Vector
        | _ when bonhommePosX > LEVEL1_BONHOMME_X_POSITION_MOVE_TRIGGER -> moveLvl1Vector
        | _ -> idleLvl1Vector



    let private ``make sure level1 never moves vertically`` (vectorMov: Vector2) =
        // maintain level background to same Y position !
        new Vector2(vectorMov.X, 0f)



    let private updateLevel1Entity (allBonhommeProps: GameEntityProperties * BonhommeProperties)
                                   (currentEntity: IGameEntity)
                                   (lvl1Props: Level1Properties)
                                   =
        let vectorMov =
            KeyboardState.getMovementVector (Keyboard.GetState())

        let nextVectorPos =
            vectorMov
            |> ``update level1 movement from right to left`` allBonhommeProps
            |> ``make sure level1 never moves vertically``
            |> ``make sure level1 never moves too much to the right`` currentEntity.Position.X

        let nextLvl1Props = Level1Properties(lvl1Props) |> Some

        let nextGameEntityProps =
            { currentEntity.Properties with
                  position = Vector2.Add(currentEntity.Position, nextVectorPos) }

        GameEntity.createEntity nextGameEntityProps nextLvl1Props currentEntity.UpdateEntity



    let updateEntity (gt: GameTime) (gs: GameState) (currentEntity: IGameEntity) (lvl1Props: Level1Properties) =

        let bhEntity =
            GameEntity.tryGetEntity gs BonhommeConstants.EntityId

        match bhEntity with
        | SomeBonhomme allBonhommeProps -> updateLevel1Entity allBonhommeProps currentEntity lvl1Props
        | _ -> currentEntity
