namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module Level1Sprite =
    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.Level1Constants



    let createLevel1SpriteSheet (g: Game) =

        let f = Sprites.SpriteTextureFactory(g)

        { level1Sprite = f.createSpriteTexture ASSET_LEVEL1_SPRITE }
