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

        Vector2 Pos1, Pos2, Pos3, Cloud_Pos1, Cloud_Pos2, Cloud_Pos3;
        Texture2D Texture1, Texture2, Texture3, Cloud1, Cloud2, Cloud3;

        SoundEffect warMusicMp3;

        public Background(Vector2 pos1) : base(pos1)
        {
            ScrollVel = (float)3;
            Pos1 = pos1;
            Cloud_Pos1 = pos1;
        }

        public override void Load(ContentManager Content)
        {
            Texture1 = Content.Load<Texture2D>("Backgrounds/Background1");
            Texture2 = Content.Load<Texture2D>("Backgrounds/Background2");
            Texture3 = Content.Load<Texture2D>("Backgrounds/Background3");
//            Texture1 = Content.Load<Texture2D>("Backgrounds/space_A_90");
//            Texture2 = Content.Load<Texture2D>("Backgrounds/space_B_90");
//            Texture3 = Content.Load<Texture2D>("Backgrounds/space_C_90");

            Cloud1 = Content.Load<Texture2D>("Backgrounds/Clouds");
            Cloud2 = Content.Load<Texture2D>("Backgrounds/Clouds");
            Cloud3 = Content.Load<Texture2D>("Backgrounds/Clouds");

            Pos2.X = Pos1.X;
            Pos3.X = Pos1.X;

            Cloud_Pos2.X = Cloud_Pos1.X;
            Cloud_Pos3.X = Cloud_Pos1.X;

            Pos2.Y = Pos1.Y - Texture1.Height;
            Pos3.Y = Pos2.Y - Texture2.Height;

            Cloud_Pos2.Y = Cloud_Pos1.Y - Cloud1.Height;
            Cloud_Pos3.Y = Cloud_Pos2.Y - Cloud2.Height;

            /// Content Importer: MP3 Audio File - XNA Framework
            /// Content Processor: Sound Effect - XNA Framework
            warMusicMp3 = Content.Load<SoundEffect>("Sounds/flight_of_the_bumblebeeM");
            warMusicMp3.Play();

        }

        public override void Draw(GameTime gameTime)
        {
            
            Shmup.spriteBatch.Draw(Texture1, Pos1, Color.White);
            Shmup.spriteBatch.Draw(Texture2, Pos2, Color.White);
            Shmup.spriteBatch.Draw(Texture3, Pos3, Color.White);
            Shmup.spriteBatch.Draw(Cloud1, Cloud_Pos1, Color.White);
            Shmup.spriteBatch.Draw(Cloud2, Cloud_Pos2, Color.White);
            Shmup.spriteBatch.Draw(Cloud3, Cloud_Pos3, Color.White);

            base.Draw(gameTime);
        }

        public override void Update()
        {

            Pos1.Y += ScrollVel;
            Pos2.Y += ScrollVel;
            Pos3.Y += ScrollVel;

            Cloud_Pos1.Y += (ScrollVel * 2);
            Cloud_Pos2.Y += (ScrollVel * 2);
            Cloud_Pos3.Y += (ScrollVel * 2);

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

            if (Cloud_Pos1.Y > Cloud1.Height)
            {
                Cloud_Pos1.Y = Cloud_Pos3.Y - Cloud3.Height;
            }
            if (Cloud_Pos2.Y > Cloud2.Height)
            {
                Cloud_Pos2.Y = Cloud_Pos1.Y - Cloud1.Height;
            }
            if (Cloud_Pos3.Y > Cloud3.Height)
            {
                Cloud_Pos3.Y = Cloud_Pos2.Y - Cloud2.Height;
            }

            base.Update();
        }
    }
}
