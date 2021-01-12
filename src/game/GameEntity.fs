namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions



    ////let createEntity id sprite extendProps update draw =

    // before making entity a record
    // gameEntity was using F# class
    // in attempt to use pseudo inheritance

    ////{ new GameEntity with
    ////    member x.ExtendProperties = extendProps
    ////    member x.Properties = props

    ////    member x.UpdateEntity gameTime gameState currentGameEntity =
    ////        update gameTime gameState currentGameEntity

    ////    member x.Position = props.position
    ////    member x.Sprite = props.sprite }

    ////{ id = id
    ////  extendProperties = extendProps
    ////  sprite = sprite
    ////  drawEntity = draw
    ////  updateEntity = update }



    let updateEntity sprite extendProps (currentEntity: GameEntity) =

        { currentEntity with
              extendProperties = extendProps
              sprite = sprite }


    let tryGetEntity (gs: GameState) (entityId: GameEntityId) =
        gs.entities |> ReadOnlyDict.tryGetValue entityId
