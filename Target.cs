using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Timers.GameStates
{
    public class Target

    {
        KeyboardState kstate = Keyboard.GetState();
        public float despawn = 0;

        public Rectangle rect;

        Random random = new Random();
        int randX, randY;
        public Target() 
        { 
            randX = random.Next(50, Globals.WIDTH-20);
            randY = random.Next(50, Globals.HEIGHT-80);
            rect = new Rectangle(randX, randY, 20, 80); 

        }


        public void Update(GameTime gameTime)
        {
            despawn += (float)gameTime.ElapsedGameTime.TotalSeconds;


        }
        public void Draw()
        {
            Globals.spriteBatch.Draw(Globals.pixel2, rect, Color.White);
        }

    }
}
