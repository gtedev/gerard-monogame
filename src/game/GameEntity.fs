namespace GerardMonogame.Game

[<RequireQualifiedAccess>]
module GameEntity =

    open Types
    open FSharp.Core.Extensions



    let updateEntity sprite extendProps (currentEntity: GameEntity) =

        { currentEntity with
              extendProperties = extendProps
              sprite = sprite }


    let tryGetEntity (gs: GameState) (entityId: GameEntityId) =
        gs.entities |> ReadOnlyDict.tryGetValue entityId
