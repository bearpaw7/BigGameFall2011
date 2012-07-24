using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace BigGameF2011.GameObjects
{
    public class Laser : Weapon
    {
        public Laser(Vector2 _pos, CollisionManager.Side _side)
            : base(_pos, _side)
        {
            speed = 10;
            damage = 5;
            SoundEffect shotSound = Shmup.contentManager.Load<SoundEffect>("Sounds/30935__aust-paul__possiblelazer");
            shotSound.Play(0.5f, 0f, 0f);
            if (_side == CollisionManager.Side.Player)
            {
                textureFile = "Sprites/laserBlue";
            }
            else
            {
                textureFile = "Sprites/laserRed";
            }
        }
    }
    public class Missile : Weapon
    {
        public Missile(Vector2 _pos, CollisionManager.Side _side) : base(_pos, _side)
        {
            textureFile = "Sprites/missile";
            speed = 5;
            damage = 20;
            SoundEffect shotSound = Shmup.contentManager.Load<SoundEffect>("Sounds/47252__nthompson__rocketexpl");
            shotSound.Play(0.5f, 0f, 0f);
        }
    }

    public class HeatSeekingMissile : Weapon
    {
        private readonly Enemy closestEnemyOnCreate;

        public HeatSeekingMissile(Vector2 _pos, CollisionManager.Side _side)
            : base(_pos, _side)
        {
            closestEnemyOnCreate = Shmup.GameObjects.Where(e => e is Enemy).OrderByDescending(e => Vector2.Distance(e.GetPosition(), this.GetPosition())).FirstOrDefault() as Enemy;
            textureFile = "Sprites/HeatSeekingMissile";
            speed = 5;
            damage = 20;
            SoundEffect shotSound = Shmup.contentManager.Load<SoundEffect>("Sounds/47252__nthompson__rocketexpl");
            shotSound.Play(0.5f, 0f, 0f);
        }
        
        public override void Draw(GameTime gameTime)
        {
            if(closestEnemyOnCreate == null)
            {
                base.Draw(gameTime);
                return;
            }

            Rectangle r = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2),
                                        (int)size.X, (int)size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);
        }

        public override void Update()
        {
            if (closestEnemyOnCreate != null && Shmup.GameObjects.Contains(closestEnemyOnCreate)) //This is expensive. Instead, I think GameObject should have a 'deleted' flag, that gets set when UnLoad is called.
            {
                velocity = closestEnemyOnCreate.GetPosition() - position;
                velocity.Normalize();
                velocity *= speed;
            }
            base.Update();
        }
    }

    public class Weapon : GameObject
    {
        public CollisionManager.Side side;
        protected String textureFile;
        protected int speed;
        protected int damage;

        public Weapon(Vector2 _pos, CollisionManager.Side _side) : base(_pos)
        {
            this.side = _side;
        }

        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>(textureFile);
            Debug.Assert((texture != null), "Weapon :: Error - Texture has not been provided");
            base.Load(Content);
            Shmup.collisions.AddCollider(collider, side);
        }

        public override void Unload()
        {
            Shmup.GameObjects.Remove(this);
            Shmup.collisions.RemoveCollider(collider);
            base.Unload();
        }

        public override void OnCollision(int damageTaken)
        {
            damage -= damageTaken;
            if (damage > 0)
            {
                return;
            }
            base.OnCollision(damageTaken);
            Unload();
        }

        public override void Update()
        {
            base.Update();
            if(position.Y > Shmup.SCREEN_HEIGHT || position.Y < 0 ||
                position.X > Shmup.SCREEN_WIDTH || position.X < 0)
            { // unload if weapon travels outside visible screen
                this.Unload();
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (side == CollisionManager.Side.Player)
            {
                position.Y -= speed;
            }
            else if (side == CollisionManager.Side.Enemy)
            {
                position.Y += speed;
            }
            Rectangle r = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2),
                                        (int)size.X, (int)size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);
            base.Draw(gameTime);
        }

        public override int giveDamage()
        {
            return damage;
        }

    }

}
