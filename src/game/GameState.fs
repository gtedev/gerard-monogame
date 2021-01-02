﻿namespace GerardMonogame.Game

open GerardMonogame.Game.Entities

[<RequireQualifiedAccess>]
module GameState =

    open Types
    open Microsoft.Xna.Framework
    open FSharp.Core.Extensions

    let updateEntities gameTime (gs: GameState) =

        let updateEntity (key, e: IGameEntity) = (key, e.UpdateEntity gameTime gs e)

        let newEntities =
            gs.entities |> ReadOnlyDict.map updateEntity

        { entities = newEntities }



    let initEntities<'T when 'T :> Game> (game: 'T) (gs: GameState) =

        let bh = BonhommeEntity.initEntity game gs

        let l1 = Level1Entity.initEntity game gs

        let entities =
            [ l1; bh ]
            |> List.toReadOnlyDict (fun e -> (e.Properties.id, e))

        { gs with entities = entities }
