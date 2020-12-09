module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Types
open Microsoft.Xna.Framework.Input

type Game1 () as game =
    inherit Game()
 
    do game.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(game)

    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable gameState = Unchecked.defaultof<GameState>


    override game.Initialize() =
        do spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        graphics.PreferredBackBufferWidth <-  512;
        graphics.PreferredBackBufferHeight <- 448;

        do base.Initialize()
         // TODO: Add your initialization logic here
        ()

    override x.LoadContent() =
        
         // TODO: use this.Content to load your game content here        
        gameState <- SpriteManager.loadSpritesIntoState game gameState
        ()
 


    override x.Update (gameTime) =

         // TODO: Add your update logic here
        gameState <- GameStateManager.updateEntities gameTime gameState
        
        ()
 

    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.Black
        
        // TODO: Add your drawing code here
        spriteBatch.Begin();

        gameState.entities
        |> List.iter (SpriteManager.drawSprite spriteBatch)

        spriteBatch.End();

        base.Draw(gameTime);
        ()



