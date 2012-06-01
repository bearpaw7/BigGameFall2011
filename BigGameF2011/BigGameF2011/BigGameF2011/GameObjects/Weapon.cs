using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BigGameF2011.GameObjects
{
    public class Laser : Weapon
    {
        public Laser(Vector2 _pos, CollisionManager.Side _side)
            : base(_pos, _side)
        {
            speed = 10;
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
        }
    }

    public class Weapon : GameObject
    {
        public CollisionManager.Side side;
        protected String textureFile;
        protected int speed;
        public Weapon(Vector2 _pos, CollisionManager.Side _side) : base(_pos)
        {
            this.side = _side;
        }

        public override void Load(ContentManager Content)
        {
//            texture = Content.Load<Texture2D>("Sprites/Weapon");
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

        public override void OnCollision()
        {
            Unload();
            base.OnCollision();
        }

        public override void Update()
        {
            Velocity = new Vector2(0, 0);
            base.Update();
            if(Position.Y > Shmup.SCREEN_HEIGHT || Position.Y < 0 ||
                Position.X > Shmup.SCREEN_WIDTH || Position.X < 0)
            { // unload if weapon travels outside visible screen
                this.Unload();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (side == CollisionManager.Side.Player)
            {
                Position.Y -= speed;
            }
            else if (side == CollisionManager.Side.Enemy)
            {
                Position.Y += speed;
            }
            Rectangle r = new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2),
                                        (int)Size.X, (int)Size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);
            base.Draw(gameTime);
        }

    }

}
