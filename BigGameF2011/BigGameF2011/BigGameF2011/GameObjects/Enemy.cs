using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BigGameF2011.GameObjects
{
    public class Enemy : Ship
    {
        public Enemy(Vector2 Pos) : base(Pos)
        {
            Speed = 10;
            Position = Pos;
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/ExampleShip");
            base.Load(Content);
            Shmup.collisions.AddCollider(collider, CollisionManager.Side.Enemy);
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
            Velocity = new Vector2(0,0);
            base.Update();
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
