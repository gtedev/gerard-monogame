module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type CurrentSpriteIndex =  int

type SpriteTexture = {
 texture: Texture2D 
 //collisionBoxes: Rectangle list
}

type Sprite = 
 | UnloadedSprite
 | SingleSprite of SpriteTexture
 | AnimatedSprite of SpriteTexture list * CurrentSpriteIndex

type GameEntityType =
 | BonhommeEntity

type GameEntity = {
     isEnabled: bool
     position : Vector2
     sprite : Sprite
     entityType: GameEntityType
}

type GameState =   {
    entities: GameEntity list
}