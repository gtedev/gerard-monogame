module EntityHelper

open Types

let createGameEntity properties updateEntity =
    { new IGameEntity with
        member x.Properties = properties
        member x.UpdateEntity gameTime currentGameEntity = updateEntity gameTime currentGameEntity
        member x.Position = properties.position
        member x.Sprite = properties.sprite }
