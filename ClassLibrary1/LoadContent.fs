module LoadContent

open Microsoft.Xna.Framework
open Types


let loadSpritesIntoState<'T when 'T :> Game> (game: 'T) (gameState: GameState) =

    let bonhommeGameEntity = BonhommeEntity.initializeEntity game

    { gameState with
          entities = [ bonhommeGameEntity ] }



