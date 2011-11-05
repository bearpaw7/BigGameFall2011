using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BigGameF2011.Collisions;

namespace BigGameF2011
{
    public class CollisionManager : GameComponent
    {
        //Data Members
        private static List<Collider> PlayerColliders;
        private static List<Collider> EnemyColliders;

        //Friend-or-Foe enumeration
        public enum Side { Neutral = 0, Player = 1, Enemy = 2}

        //Constructor
        public CollisionManager(Game game) : base(game) { }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            PlayerColliders = new List<Collider>();
            EnemyColliders = new List<Collider>();
            base.Initialize();
        }

        //Add and Remove colliders
        public void AddCollider(Collider coll, Side side)
        {
            switch (side)
            {
                case Side.Player:
                    PlayerColliders.Add(coll);
                    break;
                case Side.Enemy:
                    EnemyColliders.Add(coll);
                    break;
                default:
                    break;
            }
        }
        public void RemoveCollider(Collider coll)
        {
            if (PlayerColliders.Remove(coll))
                return;
            if (EnemyColliders.Remove(coll))
                return;
            //This would be an error!
            return;
        }

        //Check both sides for collisions and perform collision triggers if necessary.
        public override void Update(GameTime gameTime)
        {
            for(int i = PlayerColliders.Count(); i > 0; i--)
            {
                for (int j = EnemyColliders.Count(); j > 0; j--)
                {
                    if (PlayerColliders[i-1].isCollidingWith(EnemyColliders[j-1]))
                    {
                        PlayerColliders[i-1].Triggered();
                        EnemyColliders[j-1].Triggered();
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
