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
    class Menu : Microsoft.Xna.Framework.Game
    {
        //keyboard
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        //font
        SpriteFont spriteFont;
        
        //images
        Texture2D background;

        //creating a list to store menu options
        List<string> menuList = new List<string>();

        //selector
        int selected = 0;

        public Menu()
        {
            //menu list
            menuList.Add("Play");
            
            menuList.Add("Exit");
        }

        //Content to Load on menu screen
        public void LoadMenu(ContentManager Content)
        {
            //load font
            spriteFont = Content.Load<SpriteFont>("mFont");
            //load background image
            background = Content.Load<Texture2D>("images\\background2");
        }
        //Update on key press
        public void UpdateMenu(GameTime gameTime)
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
                if (selected < menuList.Count - 1)
                {
                    //move selector down
                    selected++;
                }
            }
            if (CheckKeyboard(Keys.Enter) || CheckKeyboard(Keys.Space))
            {
                switch(selected)
                {
                    //first case
                    case 0:
                        //GameState updated to Play
                        Game1.GameState = "Play";
                        break;

                    //second case
                    case 1:
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

        //Draw the menu: background and menu options
        public void DrawMenu(SpriteBatch spriteBatch)
        {
            Color colour;
            int linePadding = 3;
            spriteBatch.Begin();
            //Image used as the background
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
            //for loop used to display the selected position in menu list
            for (int i = 0; i < menuList.Count; i++)
            {
                //colour alteration used as a selecting tool
                //selected menu option will be displayed as yellow text
                //non-selected menu options will be displayed as black text
                if (i == selected)colour = Color.Yellow; 
                else colour = Color.Black; 
                //Draw menu options
                //X and Y postions are set to the centre of the screen window
                //The colour is determined by the above if statement
                spriteBatch.DrawString(spriteFont, menuList[i], new Vector2((Game1.sWidth/2) - ((spriteFont.MeasureString(menuList[i]).X/2)),
                    (Game1.sHeight/2) - (spriteFont.LineSpacing * menuList.Count/2) + ((spriteFont.LineSpacing + linePadding) * i)+30), colour);  
            }
            spriteBatch.End();
        }
    }
}
