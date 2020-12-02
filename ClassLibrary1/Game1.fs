module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open GameState
open Microsoft.Xna.Framework.Input

type Game1 () as x =
    inherit Game()
 
    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable bonhommeSprite = Unchecked.defaultof<Texture2D>
    let mutable bonhommeSpriteTexture = Unchecked.defaultof<SpriteTexture>
    let speed = 100.0;

    //let mutable contentManager =  new Content.ContentManager()

    let ASSET_BONHOMME_SPRITE = "bonhomme1";

    override x.Initialize() =
        do spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        do base.Initialize()
         
         // TODO: Add your initialization logic here

        ()

    override x.LoadContent() =
        
         // TODO: use this.Content to load your game content here        
        bonhommeSprite <- x.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE);
        bonhommeSpriteTexture <-  { texture = bonhommeSprite; x = 0.0;  y = 100.0; height = 100; width=100 }
        ()
 
    override x.Update (gameTime) =

         // TODO: Add your update logic here
        let keyboardState = Keyboard.GetState()

        if(keyboardState.IsKeyDown(Keys.Right)) then
            let posX = bonhommeSpriteTexture.x + gameTime.ElapsedGameTime.TotalSeconds
            bonhommeSpriteTexture <-  { texture = bonhommeSprite; y = bonhommeSpriteTexture.y;  x = bonhommeSpriteTexture.x + speed*gameTime.ElapsedGameTime.TotalSeconds; height = 100; width=100 }
        else
            ()
        ()
 
    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.Black
        
        // TODO: Add your drawing code here
        spriteBatch.Begin();

        spriteBatch.Draw(bonhommeSpriteTexture.texture, new Vector2((float32)bonhommeSpriteTexture.x,(float32)bonhommeSpriteTexture.y), Color.White)

        spriteBatch.End();

        base.Draw(gameTime);
        ()



