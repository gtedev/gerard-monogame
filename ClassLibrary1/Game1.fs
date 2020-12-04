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
    let mutable bonhommeGameEntity = Unchecked.defaultof<GameEntity>
    let speed = 150.0;

    let ASSET_BONHOMME_SPRITE = "bonhomme1";
        
    let (|KeyDown|_|) k (state: KeyboardState) =
        if state.IsKeyDown k then Some() else None

    let getMovementVector keyState = 
        match keyState with
        | KeyDown Keys.Up -> Vector2(0f, -1.f)
        | KeyDown Keys.Down  -> Vector2(0f, 1.f)
        | KeyDown Keys.Right  -> Vector2(1.f, 0f)
        | KeyDown Keys.Left  -> Vector2(-1.f, 0f)
        | _ -> Vector2.Zero

    override x.Initialize() =
        do spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        do base.Initialize()
         
         // TODO: Add your initialization logic here

        ()

    override x.LoadContent() =
        
         // TODO: use this.Content to load your game content here        
        bonhommeSprite <- x.Content.Load<Texture2D>(ASSET_BONHOMME_SPRITE);
        bonhommeSpriteTexture <-  { texture = bonhommeSprite }
        bonhommeGameEntity <-  { name = "bonhomme"; isEnabled = true;  spriteAnimation = [bonhommeSpriteTexture]; position = new Vector2(0f,100f) }
        ()
 
    override x.Update (gameTime) =

         // TODO: Add your update logic here
        let vectorMovement = getMovementVector (Keyboard.GetState())
        let newVector = Vector2.Add(bonhommeGameEntity.position, vectorMovement )
        bonhommeGameEntity <-  { bonhommeGameEntity with position = newVector }
        ()
 
    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.Black
        
        // TODO: Add your drawing code here
        spriteBatch.Begin();

        let sprite = List.head bonhommeGameEntity.spriteAnimation
        spriteBatch.Draw(sprite.texture, bonhommeGameEntity.position, Color.White)

        spriteBatch.End();

        base.Draw(gameTime);
        ()



