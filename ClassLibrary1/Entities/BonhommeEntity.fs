module BonhommeEntity

open Microsoft.Xna.Framework
open Types
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Graphics

let ASSET_BONHOMME_SPRITE = "bonhomme1"
let SPEED_BONHOMME_SPRITE = 2f

let initializeEntity (game:Game) =

    let bonhommeSprite = game.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE)
    let bonhommeSpriteTexture =  { texture = bonhommeSprite }

    {
        entityType = BonhommeEntity
        position = new Vector2(0f,100f);
        sprite = SingleSprite bonhommeSpriteTexture; 
        isEnabled = true 
    }

let update gameTime currentGameEntity: GameEntity = 
         
         let updateBonhommeEntity gameEntity vectorMovement = 
            { 
                gameEntity
                with position = vectorMovement;
            }

         let vectorMovement = KeyboardState.getMovementVector (Keyboard.GetState()) SPEED_BONHOMME_SPRITE
         let newVector = Vector2.Add(currentGameEntity.position, vectorMovement)

         let updatedEntity =
            match currentGameEntity.entityType with 
            | BonhommeEntity  -> updateBonhommeEntity currentGameEntity newVector
            | _ -> currentGameEntity

         updatedEntity

