﻿namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions

    let createGameEntity props customProps update =
        { new IGameEntity with
            member x.CustomEntityProperties = customProps
            member x.Properties = props

            member x.UpdateEntity gameTime gameState currentGameEntity =
                update gameTime gameState currentGameEntity

            member x.Position = props.position
            member x.Sprite = props.sprite }



    let getEntity (gs: GameState) entityId =
        gs.entities
        |> ReadOnlyDict.tryGetValue entityId