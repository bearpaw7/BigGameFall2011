using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace BigGameF2011.GameObjects
{
    public class Enemy : Ship
    {
        SoundEffect shotSound;
        ContentManager content;

        public Enemy(Vector2 Pos, ContentManager _content) : base(Pos)
        {
            Speed = 5;
            content = _content;
            Position = Pos;
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/ExampleShip2");
            shotSound = Content.Load<SoundEffect>("Sounds/30935__aust-paul__possiblelazer");
            base.Load(Content);
            Shmup.collisions.AddCollider(collider, CollisionManager.Side.Enemy);
        }

        public override void Unload()
        {
            Shmup.GameObjects.Remove(this);
            Shmup.collisions.RemoveCollider(collider);
            base.Unload();
        }

        public void spawn(){
            Enemy badguy = new Enemy(new Vector2(-100, -100), content);
            badguy.Load(content);

            float x = (float)Shmup.random.NextDouble() * Shmup.SCREEN_WIDTH;
            float y = -1.0f * badguy.texture.Height;
            badguy.SetPosition(new Vector2(x, y));

            Shmup.GameObjects.Add(badguy);
        }

        public override void OnCollision()
        {
            Unload();
            base.OnCollision();

            // Ridiculous number of enemies in short time.
            spawn();
            spawn();
        }

        public override void Update()
        {
            Velocity = new Vector2(0,0);
            aiSquarePattern();
            base.Update();

            float yClamp = Position.Y;
            if (yClamp >= (texture.Height / 2)) // Clamp after initial entry from above visible area
            {
                yClamp = MathHelper.Clamp(Position.Y, 0 + (texture.Height / 2), Shmup.SCREEN_HEIGHT - (texture.Height / 2));
            }

            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0 + (texture.Width / 2), Shmup.SCREEN_WIDTH - (texture.Width / 2)),
//                MathHelper.Clamp(Position.Y, 0 + (texture.Height / 2), Shmup.SCREEN_HEIGHT - (texture.Height / 2)));
                yClamp);
        }

        private int aiPatternPosition = 0;
        private void aiSquarePattern()
        {
            Direction = Vector2.Zero;

            // initial flight into viewable area
            if (Position.Y < (1.5f * texture.Height))
            {
                Direction.Y++;
                Velocity = Direction * Speed;
                return;
            }

            aiPatternPosition++;
            int runLength = 50;
            if (aiPatternPosition < runLength)
            {
                Direction.X++;
            }
            else if (aiPatternPosition < (2*runLength))
            {
                Direction.Y++;
            }
            else if (aiPatternPosition < (3*runLength))
            {
                Direction.X--;
            }
            else if (aiPatternPosition < (4*runLength))
            {
                Direction.Y--;
            }
            else
            {
                Direction.X++;
                aiPatternPosition = 0;
            }
            Velocity = Direction * Speed;

            int randomFire = Shmup.random.Next(100);
            if (randomFire < 2) // shoot weapons 2% of the updates
            {
                if (randomFire % 2 == 0)
                {
                    shootLaser();
                }
                else
                {
                    shootLaser();
                }
            }
        }

        private void shootLaser()
        {
            shotSound.Play();
            float barrelTip = this.GetPosition().Y + (texture.Height / 2);
            Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
            Laser shotLaser = new Laser(muzzle, CollisionManager.Side.Enemy);
            shotLaser.Load(this.content);
            Shmup.GameObjects.Add(shotLaser);
        }

        private void shootMissile()
        {
            shotSound.Play(); // FIXME : new sound
            float barrelTip = this.GetPosition().Y + (texture.Height / 2);
            Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
            Missile shotMissile = new Missile(muzzle, CollisionManager.Side.Enemy);
            shotMissile.Load(this.content);
            Shmup.GameObjects.Add(shotMissile);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle r = new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2),
                                        (int)Size.X, (int)Size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);
            base.Draw(gameTime);
        }
    }
}
