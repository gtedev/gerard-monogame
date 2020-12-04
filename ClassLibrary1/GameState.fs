module GameState

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type SpriteTexture = {
 texture: Texture2D 
 //collisionBoxes: Rectangle list
}

type GameEntity = {
 name: string
 isEnabled: bool
 position: Vector2
 spriteAnimation: SpriteTexture list
}

type GameState = {
   entities: GameEntity list
}
