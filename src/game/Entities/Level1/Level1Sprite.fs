namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module Level1Sprite =
    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.Level1Constants


    let createLevel1SpriteSheet (g: Game) =

        { level1Sprite = Sprites.createSpriteTexture g ASSET_LEVEL1_SPRITE }
