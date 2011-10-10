using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BigGameF2011.GameObjects
{
    /// <summary>
    /// This is an abstract class for all objects in our game.
    /// This should be used by anything that is on screen (outside of the background).
    /// </summary>
    public class GameObject
    {
        //Data Members
        //      Movement
        protected Vector2 Direction;
        protected Vector2 Position;
        protected Vector2 Velocity;

        //      Drawing
        public Vector2 Size;

        //Constructor
        protected GameObject(Vector2 Position)
        {
            this.Position = Position;
        }

        //Virtual Functions
        public virtual void Load(ContentManager Content) { }

        public virtual void Unload() { }

        public virtual void Update() 
        { 
            //move object based upon velocity
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
