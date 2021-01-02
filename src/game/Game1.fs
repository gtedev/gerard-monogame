module Game

open GerardMonogame.Game
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types
open Microsoft.Xna.Framework.Media


type Game1() as game =
    inherit Game()

    do game.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(game)
    let mutable song = Unchecked.defaultof<Song>

    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable gameState = Unchecked.defaultof<GameState>


    override game.Initialize() =
        do spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768

        graphics.ApplyChanges()
        do base.Initialize()
        ()

    override x.LoadContent() =

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

        gameState.entities
        |> Sprites.drawEntities spriteBatch

        spriteBatch.End()

        base.Draw(gameTime)
        ()
