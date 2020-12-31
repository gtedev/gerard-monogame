[<RequireQualifiedAccess>]
module GameEntity

open GameTypes

let createGameEntity properties customProperties updateEntity =
    { new IGameEntity with
        member x.CustomEntityProperties = customProperties
        member x.Properties = properties
        member x.UpdateEntity gameTime gameState currentGameEntity = updateEntity gameTime gameState currentGameEntity
        member x.Position = properties.position
        member x.Sprite = properties.sprite }
