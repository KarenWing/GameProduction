using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CatchTheCoins
{
    class GameOver : Microsoft.Xna.Framework.Game
    {
        //keyboard
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        //font
        SpriteFont spriteFont;
        SpriteFont scoreFont;

        //scoring text
        public Vector2 fontPos;
        

        //images
        Texture2D background;

        //creating a list to store game over options
        List<string> gOverList = new List<string>();

        //selector
        int selected = 0;

        public GameOver()
        {
            //Game Over list
            gOverList.Add("Play again");
            gOverList.Add("Main Menu");
            gOverList.Add("Exit");
        }
        //Content to Load on game over screen
        public void LoadGOver(ContentManager Content)
        {
            //load font
            spriteFont = Content.Load<SpriteFont>("mFont");
            scoreFont = Content.Load<SpriteFont>("spriteScore");

            //score text position
            fontPos = new Vector2((Game1.sWidth / 2) - 105, (Game1.sHeight / 2) -90);

            //load background image
            background = Content.Load<Texture2D>("images\\background3");
        }
        //Update on key press
        public void UpdateGOver(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();

            //check if up arrow is pressed
            if (CheckKeyboard(Keys.Up))
            {
                //make sure the selector is not on the first option
                if (selected > 0)
                {
                    //move selector up
                    selected--;
                }
            }
            //check is down arrow is pressed
            if (CheckKeyboard(Keys.Down))
            {
                //make sure the selector is not on the last options
                if (selected < gOverList.Count - 1)
                {
                    //move selector down
                    selected++;
                }
            }
            if (CheckKeyboard(Keys.Enter) || CheckKeyboard(Keys.Space))
            {
                switch (selected)
                {
                    //first option in list
                    case 0:
                        //GameState updated to Play
                        Game1.GameState = "Play";

                        //reset score when user pressed play again
                        Game1.score = 0;
                        break;
                    //second option in list
                    case 1:
                        //GameState updated to Main
                        Game1.GameState = "Menu";
                        break;
                    //third option in list
                    case 2:
                        //GameState updated to Exit
                        Game1.GameState = "Exit";
                        break;
                }
                
            }
            //Determine the previous keyboard input
            prevKeyboard = keyboard;
        }
        // Check if a key has been press and does not equal the previous key
        public bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }

        //Draw the game over screen: background and options
        public void DrawGOver(SpriteBatch spriteBatch)
        {
            Color colour;
            int linePadding = 3;
            
            spriteBatch.Begin();
            //Image used as the background
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
            //for loop used to display the values in options list
            for (int i = 0; i < gOverList.Count; i++)
            {
                //colour alteration used as a selecting tool
                //selected option will be displayed as yellow text
                //non-selected options will be displayed as black text
                if (i == selected)colour = Color.Yellow; 
                else colour = Color.Black;
                
                
                //
                spriteBatch.DrawString(scoreFont, "Your Score Was: " + Game1.score, fontPos, Color.Crimson);

                //Draw game over options
                        //X and Y postions are set to the centre of the screen window
                        //The colour is determined by the above if statement
                spriteBatch.DrawString(spriteFont, gOverList[i], new Vector2((Game1.sWidth / 2) - (spriteFont.MeasureString(gOverList[i]).X / 2),
                    (Game1.sHeight / 2) - (spriteFont.LineSpacing * gOverList.Count / 2) + ((spriteFont.LineSpacing + linePadding) * i)), colour);

                spriteBatch.DrawString(spriteFont, gOverList[i], new Vector2((Game1.sWidth / 2) - (spriteFont.MeasureString(gOverList[i]).X / 2),
                    (Game1.sHeight / 2) - (spriteFont.LineSpacing * gOverList.Count / 2) + ((spriteFont.LineSpacing + linePadding) * i)), colour);
                
            }
           
            spriteBatch.End();
        }
    }
}

