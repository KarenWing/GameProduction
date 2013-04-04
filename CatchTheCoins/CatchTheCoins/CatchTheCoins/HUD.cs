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
    class HUD : Microsoft.Xna.Framework.Game
    {
        //font of the text
        SpriteFont font;

        //Content to load on HUD
        public void LoadHUD(ContentManager Content)
        {
            //load font
            font = Content.Load<SpriteFont>("mFont");
        }

        //Draw the HUD
        public void DrawHUD(SpriteBatch spriteBatch)
        {
            //only display HUD when Gamestate = Play
            if (Game1.GameState == "Play")
            {
                //Draw the score at the top left corner, coordinates of (0,0) 
                //Draw the lives underneath the score. Using line space for the Y coordinates means it is directly under the score, because score 
                //takes up one line spacing
                spriteBatch.DrawString(font, "Score: " + Game1.score, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, "Lives: " + Game1.lives, new Vector2(0, font.LineSpacing), Color.White);

            }
        }
    }
}
