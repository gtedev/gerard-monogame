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
    abstract Properties : GameEntityProperties
    abstract UpdateEntity : GameTime ->  IGameEntity -> IGameEntity
    abstract Position : Vector2
    abstract Sprite : Sprite

let createGameEntity properties updateEntity  =
 { new IGameEntity with
     member x.Properties = properties
     member x.UpdateEntity gameTime currentGameEntity = updateEntity gameTime currentGameEntity
     member x.Position = properties.position
     member x.Sprite = properties.sprite }

type GameState =   {
    entities: IGameEntity list
}