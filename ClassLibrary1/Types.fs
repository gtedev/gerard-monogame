module Types

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type CurrentSpriteIndex =  int

type ITextOutputSink =
 abstract WriteChar : char -> unit
 abstract WriteString : string -> unit

let simpleOutputSink writeCharFunction =
 { new ITextOutputSink with
 member x.WriteChar(c) = writeCharFunction(c)
 member x.WriteString(s) = s |> String.iter x.WriteChar }

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