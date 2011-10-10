using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BigGameF2011.GameObjects
{
    /// <summary>
    /// This is the class representing the player in our game.
    /// It contains the controls and the sprites for the player's ship.
    /// </summary>
    public class Player : Ship
    {
        //Data Members
        //      Controls
        Keys moveUP; Keys moveDN; Keys moveLT; Keys moveRT; Keys shoot;
        KeyboardState keyState;
        //      Drawing
        private Texture2D texture;

        //Constructor
        public Player(Vector2 Position) : base(Position)
        {
            Size = new Vector2(50, 50);
            Speed = 10;

            //We can later add a parameter to change controls, but use KB for now
            moveUP = Keys.Up;
            moveDN = Keys.Down;
            moveLT = Keys.Left;
            moveRT = Keys.Right;
            shoot = Keys.Space;
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/ExampleShip");
        }

        public override void Update()
        {
            Direction = Vector2.Zero;

            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(moveDN))
                Direction.Y++;
            else if (keyState.IsKeyDown(moveUP))
                Direction.Y--;
            if (keyState.IsKeyDown(moveRT))
                Direction.X++;
            else if (keyState.IsKeyDown(moveLT))
                Direction.X--;

            Direction.Normalize();

            if (keyState.IsKeyUp(moveLT) && keyState.IsKeyUp(moveRT) &&
                keyState.IsKeyUp(moveUP) && keyState.IsKeyUp(moveDN))
                Velocity = Vector2.Zero;
            else
                Velocity = Direction * Speed;
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
