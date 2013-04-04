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

namespace TheColourGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color lightColour;

        //images
        Texture2D how;
        Texture2D main;
        Texture2D over;
        Texture2D select;

        //music
        SoundEffect music;

        //position of selector
        Vector2 selectPos = new Vector2(65, 195);

        //varibles
        bool selected = false;
        bool bHow;
        bool bStart;
        bool bOver = false;
        bool bMain = true;

        //colour
        byte redIntensity = 99;
        byte greenIntensity = 99;
        byte blueIntensity = 99;

        //colouring changing varibles
        bool redUp = false;
        bool greenUp = false;
        bool blueUp = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //screen size
            graphics.PreferredBackBufferWidth = (int)(454);
            graphics.PreferredBackBufferHeight = (int)(340);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Load images
            how = Content.Load<Texture2D>("images\\game1How");
            main = Content.Load<Texture2D>("images\\game1main");
            over = Content.Load<Texture2D>("images\\game1Over");
            select = Content.Load<Texture2D>("images\\Rectangle 1");

            //Load music
            music = Content.Load<SoundEffect>("audio\\music");

            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            // Allows the game to exit
            if (newState.IsKeyDown(Keys.Escape))
                this.Exit();

            //Moving the selector with left and right arrow keys
            if (newState.IsKeyDown(Keys.Right))
            {
                selected = true;
            }
            if (newState.IsKeyDown(Keys.Left))
            {
                selected = false;
            }
            //Options for enter key
            if (newState.IsKeyDown(Keys.Enter))
            {
                //when selcetor is false (on the start option) start the game
                if (selected == false)
                {
                    bStart = true;
                    bMain = false;
                    redIntensity = 99;
                    greenIntensity = 99;
                    blueIntensity = 99;
                    //play music
                    music.Play();
                }
                // when selector is true (on the how to play option) load how to play screen
                else if (selected == true)
                {
                    bHow = true;
                    redIntensity = 99;
                    greenIntensity = 99;
                    blueIntensity = 99;
                }
            }

            if (bHow == true)
            {
                //when S key is pressed in the how to play screen start game
                if (newState.IsKeyDown(Keys.S))
                {
                    bHow = false;
                    bStart = true;
                    bMain = false;
                    bOver = false;

                }
            }

            //Once start has been set to true
            if (bStart == true)
            {
                
                // change intensity depending on the value
                if (redIntensity == 255) redUp = false;
                if (redIntensity == 0) redIntensity = 1;

                if (greenIntensity == 255) greenUp = false;
                if (greenIntensity == 0) greenIntensity = 1;

                if (blueIntensity == 255) blueUp = false;
                if (blueIntensity == 0) blueIntensity = 1;

                //make colour intenisity increase varible equal to true, when key is pressed
                if (newState.IsKeyDown(Keys.R))
                {
                    redUp = true;
                }
                if (newState.IsKeyDown(Keys.G))
                {
                    greenUp = true;
                }
                if (newState.IsKeyDown(Keys.B))
                {
                    blueUp = true;
                }

                //increase colour intensity when colour up is true

                if (redUp) redIntensity++; else redIntensity--;
                if (greenUp) greenIntensity++; else greenIntensity--;
                if (blueUp) blueIntensity++; else blueIntensity--;

                //If all intensity values are below 20(black screen), then game over varible is true 
                if (
                    ((redIntensity < 20) && (greenIntensity < 20) &&
                     (blueIntensity < 20)))
                {
                    bOver = true;
                    
                }
            }
            //when game over varible is true
            if (bOver == true)
            {
                // when the M key is pressed, load Main Menu sreen
                if (newState.IsKeyDown(Keys.M))
                {
                    bHow = false;
                    bStart = false;
                    bMain = true;
                    bOver = false;

                }
            }

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //when main menu screen varible is true
            if (bMain == true)
            {
                //when selector is false(on the start option), draw main menu background image 
                //and the selector image to be displayed over the start text
                if (selected == false)
                {
                    spriteBatch.Draw(main, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(select, selectPos, Color.White);
                    selectPos.X = 65;
                }
                //when selector is false(on the how to play option), draw main menu background image 
                //and the selector image to be displayed over the how to play text
                if (selected == true)
                {
                    spriteBatch.Draw(main, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(select, selectPos, Color.White);
                    selectPos.X = 260;
                }
            }

            //when how to play screen varible is true, draw the how to play image 
            if (bHow == true)
            {
                spriteBatch.Draw(how, new Vector2(0, 0), Color.White);

                bOver = false;
            }
            //when start screen varible is true, draw the initial background with the colours define above
            if (bStart == true)
            {

                lightColour = new Color(redIntensity, greenIntensity, blueIntensity);
                GraphicsDevice.Clear(lightColour);
            }
            //when game over screen varible is true, draw the game over image
            if (bOver == true)
            {
                spriteBatch.Draw(over, new Vector2(0, 0), Color.White);



            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}