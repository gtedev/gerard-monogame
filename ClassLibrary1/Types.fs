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


type GameEntityProperties = {
    isEnabled: bool
    position : Vector2
    sprite : Sprite
}

type IGameEntity = 
    abstract properties : GameEntityProperties
    abstract updateEntity : GameTime ->  IGameEntity -> IGameEntity

let createGameEntity properties updateEntity  =
 { new IGameEntity with
     member x.properties = properties
     member x.updateEntity gameTime currentGameEntity = updateEntity gameTime currentGameEntity }

type GameState =   {
    entities: IGameEntity list
}