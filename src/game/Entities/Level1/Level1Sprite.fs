namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module Level1Sprite =
    open Types
    open Microsoft.Xna.Framework
    open Microsoft.Xna.Framework.Graphics
    open GerardMonogame.Constants.Level1Constants


    let createLevel1SpriteSheet (g: Game) =

        let lvl1Txture =
            { texture = g.Content.Load<Texture2D>(ASSET_LEVEL1_SPRITE) }

        { level1Sprite = lvl1Txture }
