[<RequireQualifiedAccess>]
module Level1Update

open Microsoft.Xna.Framework
open Types
open Level1Constants
open Microsoft.Xna.Framework.Input


let updateEntity gameTime (currentGameEntity: IGameEntity) (properties: Level1Properties) =

    // temp implementation
    currentGameEntity
