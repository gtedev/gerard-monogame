﻿module Game

open GerardMonogame.Game
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types
open Microsoft.Xna.Framework.Media
open FSharp.Core.Extensions

type Game1() as game =
    inherit Game()

    do game.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(game)
    let mutable song = Unchecked.defaultof<Song>

    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable gameState = Unchecked.defaultof<GameState>
    let mutable spriteFont = Unchecked.defaultof<SpriteFont>

    override game.Initialize() =
        do spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768

        graphics.ApplyChanges()
        do base.Initialize()
        ()

    override x.LoadContent() =

        spriteFont <- x.Content.Load<SpriteFont>("pixel-arial")
        gameState <- GameState.initEntities game gameState


        song <- game.Content.Load<Song>("super-spike-vball-nes-music-chicago-match")
        MediaPlayer.IsRepeating <- true
        //MediaPlayer.Play(song)
        ()



    override x.Update(gameTime) =

        gameState <- GameState.updateEntities gameTime gameState

        ()


    override x.Draw(gameTime) =
        do x.GraphicsDevice.Clear Color.Black

        spriteBatch.Begin()

        let spriteService =
            { spriteBatch = spriteBatch
              spriteFont = spriteFont }

        gameState.entities
        |> Sprites.drawEntities spriteService

        spriteBatch.End()

        base.Draw(gameTime)
        ()
