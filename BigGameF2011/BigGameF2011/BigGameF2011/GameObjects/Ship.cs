using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BigGameF2011.GameObjects
{
    /// <summary>
    /// This is an abstract class containing ship information.
    /// It should be inherited by the player, enemy, mothership, and boss classes.
    /// </summary>
    public class Ship : GameObject
    {
        //Data Members
        protected int Health;
        protected float Speed;
        //Stuff for weapons?

        //Constructor
        protected Ship(Vector2 Position) : base(Position) { }

        //Functions
        public virtual void Accelerate(float rate) { }
        public virtual void Heal(int amount) 
        {
            Health += amount;
        }
        public virtual void Hurt(int amount) 
        {
            Health -= amount;
        }
    }
}
