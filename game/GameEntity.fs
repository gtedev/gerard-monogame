namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions

    let createGameEntity properties customProperties updateEntity =
        { new IGameEntity with
            member x.CustomEntityProperties = customProperties
            member x.Properties = properties

            member x.UpdateEntity gameTime gameState currentGameEntity =
                updateEntity gameTime gameState currentGameEntity

            member x.Position = properties.position
            member x.Sprite = properties.sprite }


    let getEntityFromGameState (gameState: GameState) entityId =
        gameState.entities
        |> ReadOnlyDict.tryGetValue entityId
