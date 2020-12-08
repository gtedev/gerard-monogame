module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE = "bonhomme1"
let SPEED_BONHOMME_SPRITE = 2f


let update (gameTime:GameTime) (currentGameEntity: IGameEntity) : IGameEntity = 
         
         let vectorMovement = KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE
         let newVector = Vector2.Add(currentGameEntity.Position, vectorMovement)

         let properties = { currentGameEntity.Properties with position = newVector }

         createGameEntity properties currentGameEntity.UpdateEntity


let initializeEntity (game:Game) =

        let bonhommeSprite = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE)
        let bonhommeSpriteTexture =  { texture = bonhommeSprite }

        let properties = {
            position = new Vector2(0f,100f);
            sprite = SingleSprite bonhommeSpriteTexture; 
            isEnabled = true 
        }

        createGameEntity properties update
