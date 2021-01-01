namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module Level1Sprite =
    open Types
    open Microsoft.Xna.Framework
    open Microsoft.Xna.Framework.Graphics
    open GerardMonogame.Constants


    let createLevel1SpriteSheet (game: Game) =

        let level1Texture =
            { texture = game.Content.Load<Texture2D>(Level1Constants.ASSET_LEVEL1_SPRITE) }

        { level1Sprite = level1Texture }
