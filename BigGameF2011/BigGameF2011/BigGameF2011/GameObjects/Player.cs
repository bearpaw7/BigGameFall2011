using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BigGameF2011.Collisions;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

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
        KeyboardState lastKeyState;

        //Sounds
        SoundEffect moveSound;
        SoundEffectInstance moveLoop;
        SoundEffect shotSound;
        SoundEffectInstance shotLoop;

        //Content object reference
        ContentManager content;

        //Constructor
        public Player(Vector2 Position, ContentManager _content) : base(Position)
        {
            Speed = 10;
            content = _content;
            //We can later add a parameter to change controls, but use KB for now
            moveUP = Keys.Up;
            moveDN = Keys.Down;
            moveLT = Keys.Left;
            moveRT = Keys.Right;
            shoot = Keys.Space;

            Health = 100;
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            moveSound = Content.Load<SoundEffect>("Sounds/25761__wolfsinger__space-engine");
            moveLoop = moveSound.CreateInstance();
            moveLoop.IsLooped = true;
            shotSound = Content.Load<SoundEffect>("Sounds/30935__aust-paul__possiblelazer");
            shotLoop = shotSound.CreateInstance();
            shotLoop.IsLooped = false;
            texture = Content.Load<Texture2D>("Sprites/ExampleShip");
            base.Load(Content);
            Shmup.collisions.AddCollider(collider, CollisionManager.Side.Player);
        }

        public override void Unload()
        {
            Shmup.collisions.RemoveCollider(collider);
            base.Unload();
        }

        public override void OnCollision(int damageTaken)
        {
            Hurt(damageTaken);
            System.Console.WriteLine("Player health now " + Health.ToString());
            if (Health > 0)
            {
                return;
            }
            //Unload();
            base.OnCollision(damageTaken);
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            Direction = Vector2.Zero;

            if (keyState.IsKeyDown(shoot))//&& lastKeyState.IsKeyUp(shoot))
            {
                if(lastKeyState.IsKeyDown(shoot)){
                    shotLoop.Stop();
                }
                shotLoop.Play();
                float barrelTip = this.GetPosition().Y - (texture.Height / 2) - 25;
                Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
                Laser shotLaser = new Laser(muzzle, CollisionManager.Side.Player);
                shotLaser.Load(this.content);
                Shmup.GameObjects.Add(shotLaser);
            }

            if (keyState.IsKeyDown(moveDN))     Direction.Y++; 
            else if (keyState.IsKeyDown(moveUP))Direction.Y--; 
            if (keyState.IsKeyDown(moveRT))     Direction.X++;
            else if (keyState.IsKeyDown(moveLT))Direction.X--;
            Direction.Normalize();

            if (keyState.IsKeyUp(moveLT) && keyState.IsKeyUp(moveRT) &&
                keyState.IsKeyUp(moveUP) && keyState.IsKeyUp(moveDN))
            {
                Velocity = Vector2.Zero;
                moveLoop.Pause();
            }
            else
            {
                Velocity = Direction * Speed;
                moveLoop.Play();
            }
            lastKeyState = keyState;
            base.Update();
            
            //Clamp player's movements so that they can't go off screen.
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0+(texture.Width/2), Shmup.SCREEN_WIDTH-(texture.Width/2)),
                MathHelper.Clamp(Position.Y, 0+(texture.Height/2), Shmup.SCREEN_HEIGHT-(texture.Height/2)));
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
