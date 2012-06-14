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
        protected int health;
        protected float speed;
        //Stuff for weapons?

        //Constructor
        protected Ship(Vector2 position) : base(position) { }

        //Functions
        public virtual void Accelerate(float rate) { }
        public virtual void Heal(int amount) 
        {
            health += amount;
        }
        public virtual void Hurt(int amount) 
        {
            health -= amount;
        }

        public override int giveDamage()
        {
            // A ship crashing into something usually gives damage proportional to how strong it has.
            return 2 * Math.Abs(health);
        }
    }
}
