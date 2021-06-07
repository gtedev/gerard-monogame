namespace GerardMonogame.Game.Entities

open GerardMonogame.Game

[<RequireQualifiedAccess>]
module MechantSprite =

    open Types
    open Microsoft.Xna.Framework
    open GerardMonogame.Constants.MechantConstants

    let createMechantSpriteSheet (g: Game) =

        let f = Sprites.SpriteTextureFactory(g)

        { mechantSprite = f.createSpriteTexture ASSET_MECHANT_SPRITE }
