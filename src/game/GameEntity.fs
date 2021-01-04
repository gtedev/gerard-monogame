namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions



    let createEntity props extendProps update =

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

        { extendProperties = extendProps
          properties = props
          updateEntity = update }



    let updateEntity props extendProps (currentEntity: GameEntity) =

        { currentEntity with
              extendProperties = extendProps
              properties = props }


    let tryGetEntity (gs: GameState) (entityId: GameEntityId) =
        gs.entities |> ReadOnlyDict.tryGetValue entityId
