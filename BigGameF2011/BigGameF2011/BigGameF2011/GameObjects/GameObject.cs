using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BigGameF2011.Collisions;
using System.Diagnostics;

namespace BigGameF2011.GameObjects
{
    /// <summary>
    /// This is an abstract class for all objects in our game.
    /// This should be used by anything that is on screen (outside of the background).
    /// </summary>
    public class GameObject
    {
        //Data Members
        //      Collision
        protected Collider collider;

        //      Movement
        protected Vector2 Direction;
        protected Vector2 Position;
        protected Vector2 Velocity;

        //      Drawing
        public Vector2 Size;
        protected Texture2D texture;

        //Constructor
        protected GameObject(Vector2 Position)
        {
            this.Position = Position;
        }

        public Vector2 GetPosition() { return Position; }
        public void SetPosition(Vector2 pos) { Position = pos; }

        //Virtual Functions
        //When this is overloaded, the inheriting class MUST provide a texture!
        public virtual void Load(ContentManager Content)
        {
            Debug.Assert((texture != null), "Texture has not been provided");
            Size = new Vector2(texture.Width, texture.Height);
            collider = new Collisions.Collider(this, texture);
        }
        public virtual void OnCollision(int damageTaken) { }
        public virtual void Unload()
        {
            Shmup.GameObjects.Remove(this);
        }

        public virtual void Update() 
        { 
            //move object based upon velocity
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }

        public virtual int giveDamage(){ return 0;}
        public virtual void Draw(GameTime gameTime) { }
    }
}
