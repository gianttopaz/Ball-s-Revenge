using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timers.GameStates
{
    public class Crosshair
    {
        public Rectangle rect;

        private int moveSpeed = 200;

        public Crosshair()
        {
            rect = new Rectangle(Globals.WIDTH / 2 - 5, Globals.HEIGHT / 2 - 5, 10, 10);
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.W) && rect.Y > 50)
            {
                rect.Y -= (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (kstate.IsKeyDown(Keys.D) && rect.X < Globals.WIDTH - rect.Width)
            {
                rect.X += (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (kstate.IsKeyDown(Keys.S) && rect.Y < Globals.HEIGHT - rect.Height)
            {
                rect.Y += (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (kstate.IsKeyDown(Keys.A) && rect.X > 0)
            {
                rect.X -= (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public void Draw()
        {
            Globals.spriteBatch.Draw(Globals.pixel, rect, Color.White);
        }

    }

}
