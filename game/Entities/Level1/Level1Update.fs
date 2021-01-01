namespace GerardMonogame.Game.Entities

open GerardMonogame.Game
open Microsoft.Xna.Framework.Input

[<RequireQualifiedAccess>]
module Level1Update =
    open Microsoft.Xna.Framework
    open Types
    open GerardMonogame.Constants



    let ``update level1 next movement X position with`` (allBonHommeProperties: GameEntityProperties * BonhommeProperties)
                                                        (vectorMovement: Vector2)
                                                        =

        let (gameEntityProperties, bonhommeProperties) = allBonHommeProperties

        let bonhommePositionX = gameEntityProperties.position.X
        let idleLevel1Vector = new Vector2(0f, 0f)

        let moveLevel1Vector =
            new Vector2(
                vectorMovement.X
                * (-1f)
                * Level1Constants.SPEED_MOVING_FLOOR,
                vectorMovement.Y
            )

        match bonhommeProperties.movementStatus with
        | Duck _ -> idleLevel1Vector
        | _ when bonhommePositionX > Level1Constants.LEVEL1_BONHOMME_X_POSITION_MOVE_TRIGGER -> moveLevel1Vector
        | _ -> idleLevel1Vector



    let ``update level1 next movement Y position`` (vectorMovement: Vector2) =
        // maintain level background to same Y position !
        new Vector2(vectorMovement.X, 0f)



    let updateLevel1Entity (allBonHommeProperties: GameEntityProperties * BonhommeProperties)
                           (currentGameEntity: IGameEntity)
                           (properties: Level1Properties)
                           =

        let vectorMovement =
            KeyboardState.getMovementVectorFromKeyState (Keyboard.GetState())

        let nextVectorPosition =
            vectorMovement
            |> ``update level1 next movement X position with`` allBonHommeProperties
            |> ``update level1 next movement Y position``

        let nextLevel1Properties = Level1Properties(properties) |> Some

        let nextGameEntityProperties =
            { currentGameEntity.Properties with
                  position = Vector2.Add(currentGameEntity.Position, nextVectorPosition) }

        GameEntity.createGameEntity nextGameEntityProperties nextLevel1Properties currentGameEntity.UpdateEntity



    let updateEntity gameTime (gameState: GameState) (currentGameEntity: IGameEntity) (properties: Level1Properties) =

        let someEntity =
            GameEntity.getEntityFromGameState gameState BonhommeConstants.EntityId

        match someEntity with
        | SomeBonhomme allBonHommeProperties -> updateLevel1Entity allBonHommeProperties currentGameEntity properties
        | _ -> currentGameEntity
