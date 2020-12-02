module GameState

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type SpriteTexture = {
 x: float
 y: float
 height: int
 width: int
 texture: Texture2D 
}

type GameEntity = {
 name: string
 isEnabled: bool
 position: Vector2
 collisionBoxes: Rectangle list
 sprite: SpriteTexture
}

type GameState = {
   entities: GameEntity list
}
