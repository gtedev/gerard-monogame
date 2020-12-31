[<RequireQualifiedAccess>]
module Level1Sprite

open GameTypes
open Level1Constants
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics



let createLevel1SpriteSheet (game: Game) =

    let level1Texture =
        { texture = game.Content.Load<Texture2D>(ASSET_LEVEL1_SPRITE) }

    { level1Sprite = level1Texture }
