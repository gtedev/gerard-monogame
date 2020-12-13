module GameEntity

open Types

let createGameEntity properties customProperties updateEntity =
    { new IGameEntity with
        member x.CustomEntityProperties = customProperties
        member x.Properties = properties
        member x.UpdateEntity gameTime currentGameEntity = updateEntity gameTime currentGameEntity
        member x.Position = properties.position
        member x.Sprite = properties.sprite }
