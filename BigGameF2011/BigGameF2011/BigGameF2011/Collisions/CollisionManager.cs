using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BigGameF2011.Collisions;
using System.Diagnostics;
using BigGameF2011.GameObjects;

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
// xc            System.Console.WriteLine("CollisionManager initialized");
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
            {
//                Console.WriteLine("remove player collider");
                return;
            }
            if (EnemyColliders.Remove(coll))
            {
//                Console.WriteLine("remove enemy collider");
                return;
            }
            //This would be an error!
            Debug.Assert(false, "Error in CollisionManager::RemoveCollider(Collider coll)");
            return;
        }

        //Check both sides for collisions and perform collision triggers if necessary.
        public override void Update(GameTime gameTime)
        {
//            for (int i = PlayerColliders.Count(); i > 0; i--)
            for (int i = 0; i < PlayerColliders.Count(); ++i)
            {
//                for (int j = EnemyColliders.Count(); j > 0; j--)
                for (int j = 0; j < EnemyColliders.Count(); ++j)
                {
                    /** FIXME
                     * PlayerColliders size is changing after most collisions.
                     * I keep getting out of range errors and this is the best fix I
                     * could come up with before finishing for the night.
                     */
                    while (i >= PlayerColliders.Count())
                    {
                        --i;
                    }

                    if (PlayerColliders[i].isCollidingWith(EnemyColliders[j]))
                    {
                        System.Console.WriteLine("\nPlayerColliders[" + i + "].getGameObject().GetType().ToString() => " +
                            PlayerColliders[i].getGameObject().GetType().ToString());
                        System.Console.WriteLine("EnemyColliders[" + j + "].getGameObject().GetType().ToString() => " +
                            EnemyColliders[j].getGameObject().GetType().ToString());

                        PlayerColliders[i].Triggered();
                        EnemyColliders[j].Triggered();
                    }

                } // end enemy loop
            } // end player loop
            System.Console.WriteLine("\n*** end collision update ***");
            base.Update(gameTime);
        }
        public int getCombinedCount()
        {
            return (PlayerColliders.Count + EnemyColliders.Count);
        }
    }
}
