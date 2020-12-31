module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open GameTypes
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
        // TODO: Add your initialization logic here
        ()

    override x.LoadContent() =

        // TODO: use this.Content to load your game content here
        gameState <- GameState.initializeEntities game gameState
        song <- game.Content.Load<Song>("super-spike-vball-nes-music-chicago-match")
        MediaPlayer.IsRepeating <- true
        //MediaPlayer.Play(song)
        ()



    override x.Update(gameTime) =

        // TODO: Add your update logic here
        gameState <- GameState.updateEntities gameTime gameState

        ()


    override x.Draw(gameTime) =
        do x.GraphicsDevice.Clear Color.Black

        // TODO: Add your drawing code here
        spriteBatch.Begin()

        gameState.entities
        |> Dict.filter (fun (key, entity) -> entity.Properties.isEnabled)
        |> Dict.iter (fun (key, entity) -> Sprites.drawSprite spriteBatch entity)

        spriteBatch.End()

        base.Draw(gameTime)
        ()
