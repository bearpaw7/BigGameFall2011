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
        int aiPatternChosen;
        int aiPatternPosition;
        int aiRunLength;
        int aiLifetime;

        public Enemy(Vector2 Pos)
            : base(Pos)
        {
            speed = 5;
            position = Pos;
            health = 50;
            aiPatternChosen = Shmup.random.Next() % 3;          // 0 is square, 1 is x-cross, 2 is kamikaze
            aiPatternPosition = 0;
            aiRunLength = 60 - Shmup.random.Next(0, 20);        // run length is between 40 and 60
            aiLifetime = Environment.TickCount + 20000;         // 20 second lifetime
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/ExampleShip2");
            base.Load(Content);
            Shmup.collisions.AddCollider(collider, CollisionManager.Side.Enemy);
        }

        public override void Unload()
        {
            Shmup.GameObjects.Remove(this);
            Shmup.collisions.RemoveCollider(collider);
            base.Unload();
        }

        public override void OnCollision(int damageTaken)
        {
            Hurt(damageTaken);
            if (health > 0)
            {
                return;
            }
            Player.addKill();
            Unload();
            base.OnCollision(damageTaken);
        }

        public override void Update()
        {
            velocity = new Vector2(0,0);
            if (Environment.TickCount > aiLifetime)
            {   // after lifetime, go to kamikaze
                aiPatternChosen = 3;
            }
            aiPlayPattern();
            base.Update();

            float yClamp = position.Y;
            if (yClamp >= (texture.Height / 2)) // Clamp after initial entry from above visible area
            {
                yClamp = MathHelper.Clamp(position.Y, 0 + (texture.Height / 2), Shmup.SCREEN_HEIGHT + (2 * texture.Height));
            }

            if (yClamp > Shmup.SCREEN_HEIGHT + texture.Height)
            {   // remove when lower than visible screen
                Unload();
                return;
            }
            position = new Vector2(
                MathHelper.Clamp(position.X, 0 + (texture.Width / 2), Shmup.SCREEN_WIDTH - (texture.Width / 2)),
//                MathHelper.Clamp(position.Y, 0 + (texture.Height / 2), Shmup.SCREEN_HEIGHT - (texture.Height / 2)));
                yClamp);
        }

        private void aiPlayPattern()
        {
            direction = Vector2.Zero;

            // initial flight into viewable area
            if (position.Y < (0.7f * texture.Height))
            {
                direction.Y++;
                velocity = direction * speed;
                return;
            }
            if (aiPatternChosen == 0)
            {
                aiSquarePattern();
            }
            else if (aiPatternChosen == 1)
            {
                aiCrossPattern();
            }
            else
            {
                aiKamikaze();
            }

            int randomFire = Shmup.random.Next(1,1000);
            if (randomFire < 20) // shoot weapons 2% of the updates
            {
                if (randomFire % 4 == 0)
                {
                    shootMissile();
                }
                else
                {
                    shootLaser();
                }
            }
            velocity = direction * speed;
        }

        private void aiSquarePattern()
        {
            aiPatternPosition++;
            if (aiPatternPosition < aiRunLength)
            {
                direction.X++;
            }
            else if (aiPatternPosition < (2 * aiRunLength))
            {
                direction.Y++;
            }
            else if (aiPatternPosition < (3 * aiRunLength))
            {
                direction.X--;
            }
            else if (aiPatternPosition < (4 * aiRunLength))
            {
                direction.Y--;
            }
            else
            {
                direction.X++;
                aiPatternPosition = 0;
            }
        }

        private void aiCrossPattern()
        {
            aiPatternPosition++;
            if (aiPatternPosition < aiRunLength)
            {
                direction.X++;
                direction.Y++;
            }
            else if (aiPatternPosition < (2 * aiRunLength))
            {
                direction.Y--;
            }
            else if (aiPatternPosition < (3 * aiRunLength))
            {
                direction.X--;
                direction.Y++;
            }
            else if (aiPatternPosition < (4 * aiRunLength))
            {
                direction.Y--;
            }
            else
            {
                direction.X++;
                aiPatternPosition = 0;
            }
        }

        private void aiKamikaze()
        {
            if (position.X > Shmup.mainPlayer.GetPosition().X + 10)
            {
                direction.X--;
            }
            else if (position.X < Shmup.mainPlayer.GetPosition().X - 10)
            {
                direction.X++;
            }
            direction.Y++;
        }

        private void shootLaser()
        {
            float barrelTip = this.GetPosition().Y + (texture.Height / 2);
            Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
            Laser shotLaser = new Laser(muzzle, CollisionManager.Side.Enemy);
            shotLaser.Load(Shmup.contentManager);
            Shmup.GameObjects.Add(shotLaser);
        }

        private void shootMissile()
        {
            float barrelTip = this.GetPosition().Y + (texture.Height / 2);
            Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
            Missile shotMissile = new Missile(muzzle, CollisionManager.Side.Enemy);
            shotMissile.Load(Shmup.contentManager);
            Shmup.GameObjects.Add(shotMissile);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle r = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2),
                                        (int)size.X, (int)size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);
            base.Draw(gameTime);
        }
    }
}
