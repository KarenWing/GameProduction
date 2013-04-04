using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace CatchTheCoins
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        Menu menu;
        HUD hud;
        GameOver gOver;
        
        //GameState set to Menu, allows the Menu to display
        public static string GameState = "Menu";

        //screen
        public static int sWidth = 600;
        public static int sHeight = 600;

        // The images we will draw
        Texture2D boxTexture;
        Texture2D coinTexture;

        //background image
        Texture2D background;

        // Sound 
        SoundEffect drop;

        //music;
        SoundEffect music;
        

        // The color data for the images; used for per pixel collision
        Color[] boxTextureData;
        Color[] coinTextureData;

        // The images will be drawn with this SpriteBatch
        SpriteBatch spriteBatch;

        // Basket
        Vector2 boxPosition;
        const int boxMoveSpeed = 6;

        // Coins
        List<Vector2> coinPositions = new List<Vector2>();
        float CoinSpawnProbability = 0.993f;
        const int CoinFallSpeed = 3;

        Random random = new Random();
        private int frameCount = 0;
        private int frameTimer = 0;

        // For when a collision is detected
        public static bool boxHit = false;

        // A rectangle used to ensure all drawn objects are within the screen and is visible 
        Rectangle safeBounds;

        // Percentage of the screen on every side used to set safeBounds
        const float SafeAreaPortion = 0.01f;

        //score
        public static int score = 0;

        
        //life
        public static int lives = 5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //game screen size
            graphics.PreferredBackBufferWidth = sWidth;
            graphics.PreferredBackBufferHeight = sHeight;

            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            
            //new menu instance used for calling later
            menu = new Menu();
            //new HUD instance used for calling later
            hud = new HUD();
            //new GameOver instance used for calling later
            gOver = new GameOver();

            // Calculate safeBounds based on current resolution
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            safeBounds = new Rectangle(
                (int)(viewport.Width * SafeAreaPortion),
                (int)(viewport.Height * SafeAreaPortion),
                (int)(viewport.Width * (1 - 2 * SafeAreaPortion)),
                (int)(viewport.Height * (1 - 2 * SafeAreaPortion)));

            base.Initialize();
 
        }

        protected override void LoadContent()
        {
            //calling the LoadMenu method from Menu class
            menu.LoadMenu(Content);

            //calling the LoadHUD method from HUD class
            hud.LoadHUD(Content);

            //calling the LoadGOver method from GameOver class
            gOver.LoadGOver(Content);
   
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            boxTexture = Content.Load<Texture2D>("images\\basket");
            coinTexture = Content.Load<Texture2D>("images\\coin");

            //load background image
            background = Content.Load<Texture2D>("images\\background");
            
            // Load sound
            music = Content.Load<SoundEffect>("audio\\backgroundMusic");
            drop = Content.Load<SoundEffect>("audio\\coinDrop");

            //creating a new inatance so the music loops
            SoundEffectInstance soundEffectInstance = music.CreateInstance();
            soundEffectInstance.IsLooped = true;

            //play background music
            soundEffectInstance.Play();            

            // Start the player in the center along the bottom of the screen
            boxPosition.X = (safeBounds.Width - boxTexture.Width) / 2;
            boxPosition.Y = safeBounds.Height - boxTexture.Height;

            // Extract collision data
            // When colour is different, this determines an object is detected 
            coinTextureData =
                new Color[coinTexture.Width * coinTexture.Height];
            coinTexture.GetData(coinTextureData);

            boxTextureData =
                new Color[boxTexture.Width * boxTexture.Height];
            boxTexture.GetData(boxTextureData);

            
        }

        protected override void Update(GameTime gameTime)
        {
            // Get input
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);       

            // Allows the game to exit
            if (gamePad.Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.Escape) ||
                GameState == "Exit")
            {
                this.Exit();
            }
            // Used to test game over screen
            if (keyboard.IsKeyDown(Keys.G))
            {
                GameState = "GameOver";
            }

            switch (GameState)
            {
                
                case "Menu":

                    //calling the UpdateMenu method from Menu class
                    menu.UpdateMenu(gameTime);

                    //reset score
                    score = 0;
                    
                    break;

                case "Play":
                    
                    // Move the player left and right with arrow keys or A an D keys (more options for players)
                    if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                    {
                        boxPosition.X -= boxMoveSpeed;
                    }
                    if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                    {
                        boxPosition.X += boxMoveSpeed;
                    }

                    // Prevent the person from moving off of the screen
                    Viewport viewport = graphics.GraphicsDevice.Viewport;
                    boxPosition.X = MathHelper.Clamp(boxPosition.X,
                    safeBounds.Left, safeBounds.Right - boxTexture.Width);

                    // Spawn new falling coins
                    //Spawn when the random number is more than the CoinSpawnProbability: generating a random spawn rate
                    //And when this is less than 2 coins on the screen: prevent game being impossible
                    if ((random.NextDouble() > CoinSpawnProbability) && (coinPositions.Count < 2))
                    {
                        float x = (float)random.NextDouble() *
                            (Window.ClientBounds.Width - coinTexture.Width);
                        coinPositions.Add(new Vector2(x, -coinTexture.Height));
                    }

                    // Get the bounding rectangle of the basket
                    Rectangle boxRectangle =
                        new Rectangle((int)boxPosition.X, (int)boxPosition.Y,
                        boxTexture.Width, boxTexture.Height);

                    // Update each coin
                    boxHit = false;


                    for (int i = 0; i < coinPositions.Count; i++)
                    {
                        // Animate coin falling
                        coinPositions[i] =
                            new Vector2(coinPositions[i].X,
                                        coinPositions[i].Y + CoinFallSpeed);

                        // Get the bounding rectangle of coin
                        Rectangle coinRectangle =
                        new Rectangle((int)coinPositions[i].X, (int)coinPositions[i].Y,
                        coinTexture.Width, 27);//Y must be the height of the frame, otherwise the height will be the height of the sprite sheet

                        // Check collision with basket
                        if (IntersectPixels(boxRectangle, boxTextureData,
                                           coinRectangle, coinTextureData))
                        {
                            boxHit = true;
                        }

                        // Remove this block if it have fallen off the screen
                        if (coinPositions[i].Y > Window.ClientBounds.Height)
                        {
                            coinPositions.RemoveAt(i);

                            // When removing a block, the next block will have the same index
                            // as the current block. Decrement i to prevent skipping a block.
                            i--;

                            //one life is lost when coin does not collide with basket and falls off the screen 
                            lives--;
                        }

                        //Remove coin once it hits the basket
                        //increase score by one when they collide
                        //play the coin drop sound effect when they colllide
                        if (boxHit)
                        {
                            coinPositions.RemoveAt(i);
                            score++;
                            drop.Play();
                        }

                        //Gamestate changed to GameOver when no lives left 
                        if (lives == 0)
                        {
                   GameState = "GameOver";
                            //reset life
                            lives = 5;
                        }
                    }
                        break;

                    case "GameOver":
                        //calling the UpdateGOver method from GameOver class
                        gOver.UpdateGOver(gameTime);
                        break;


            }
            //switching between frames in the sprite sheet to anime coin
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTimer > 100)
            {
                //move between frames
                frameCount++;
                // on the third frame, frame returns back to the first one
                if (frameCount > 2)
                {
                    frameCount = 0;
                }
                //after 100 milliseconds the frames start to animate again
                frameTimer = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            switch (GameState)
            {
                case "Menu":
                    //calling the DrawMenu method from Menu class
                    menu.DrawMenu(spriteBatch);
                    break;

                case "Play":

                    spriteBatch.Begin();

                    //Image used as the background
                    spriteBatch.Draw(background, Vector2.Zero, Color.White);

                    //calling the DrawHUD method from HUD class
                    hud.DrawHUD(spriteBatch);

                    // Draw basket
                    spriteBatch.Draw(boxTexture, boxPosition, Color.White);

                    // Draw coins. using new Rectangle to specify the first frame to display, then the size of the frames
                    foreach (Vector2 coinPosition in coinPositions)
                        spriteBatch.Draw(coinTexture, coinPosition, new Rectangle(0,27 * frameCount, 33, 27), Color.White);

                    spriteBatch.End();
                    break;

                 case "GameOver":
                    
                    //calling the DrawGOver method from GameOver class
                    gOver.DrawGOver(spriteBatch);
                    break;
            }

          base.Draw(gameTime);
        }


        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
