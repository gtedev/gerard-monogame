namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions



    let createEntity props extendProps update =
        { new IGameEntity with
            member x.ExtendProperties = extendProps
            member x.Properties = props

            member x.UpdateEntity gameTime gameState currentGameEntity =
                update gameTime gameState currentGameEntity

            member x.Position = props.position
            member x.Sprite = props.sprite }



    let updateEntity props extendProps (currentEntity: IGameEntity) =
        { new IGameEntity with
            member x.ExtendProperties = extendProps
            member x.Properties = props

            member x.UpdateEntity gameTime gameState currentGameEntity =
                currentEntity.UpdateEntity gameTime gameState currentGameEntity

            member x.Position = props.position
            member x.Sprite = props.sprite }


    let tryGetEntity (gs: GameState) entityId =
        gs.entities |> ReadOnlyDict.tryGetValue entityId
