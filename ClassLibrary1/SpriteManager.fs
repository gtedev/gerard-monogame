module SpriteManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types

let getTextureToDraw sprite = 
    match sprite with
    | SingleSprite sprite -> sprite.texture
    | _ -> Unchecked.defaultof<Texture2D>


let loadSpritesIntoState<'T when 'T :> Game> (game: 'T)(gameState: GameState) = 

    let bonhommeGameEntity = BonhommeEntity.initializeEntity game
    { gameState with entities = [bonhommeGameEntity]}


let drawSprite (spriteBatch:SpriteBatch) (gameEntity: IGameEntity)  =

     let textureToDraw = getTextureToDraw gameEntity.Sprite

     spriteBatch.Draw(textureToDraw, gameEntity.Position, Color.White)