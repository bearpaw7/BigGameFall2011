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

namespace BigGameF2011.GameObjects
{
    public class Background : GameObject
    {
        float ScrollVel;

        Vector2 Pos1, Pos2, Pos3;
        Texture2D Texture1, Texture2, Texture3;

        public Background(Vector2 pos1) : base(pos1)
        {
            ScrollVel = (float)3;
            Pos1 = pos1;
        }

        public override void Load(ContentManager Content)
        {
            Texture1 = Content.Load<Texture2D>("Backgrounds/Background1");
            Texture2 = Content.Load<Texture2D>("Backgrounds/Background2");
            Texture3 = Content.Load<Texture2D>("Backgrounds/Background3");

            Pos2.X = Pos1.X;
            Pos3.X = Pos1.X;

            Pos2.Y = Pos1.Y - Texture1.Height;
            Pos3.Y = Pos2.Y - Texture2.Height;

        }

        public override void Draw(GameTime gameTime)
        {
            
            Shmup.spriteBatch.Draw(Texture1, Pos1, Color.White);
            Shmup.spriteBatch.Draw(Texture2, Pos2, Color.White);
            Shmup.spriteBatch.Draw(Texture3, Pos3, Color.White);
            base.Draw(gameTime);
        }

        public override void Update()
        {

            Pos1.Y += ScrollVel;
            Pos2.Y += ScrollVel;
            Pos3.Y += ScrollVel;

            if (Pos1.Y > Texture1.Height)
            {
                Pos1.Y = Pos3.Y - Texture3.Height;
            }
            if (Pos2.Y > Texture2.Height)
            {
                Pos2.Y = Pos1.Y - Texture1.Height;
            }
            if (Pos3.Y > Texture3.Height)
            {
                Pos3.Y = Pos2.Y - Texture2.Height;
            }
            base.Update();
        }
    }
}
