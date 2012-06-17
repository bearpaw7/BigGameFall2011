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
//        KeyboardState lastKeyState;

        //Sounds
        SoundEffect moveSound;
        SoundEffectInstance moveLoop;

        static int kills;
        int timeLastFired;

        //Constructor
        public Player(Vector2 position) : base(position)
        {
            speed = 10;
            //We can later add a parameter to change controls, but use KB for now
            moveUP = Keys.Up;
            moveDN = Keys.Down;
            moveLT = Keys.Left;
            moveRT = Keys.Right;
            shoot = Keys.Space;

            health = 100;
            kills = 0;
            timeLastFired = 0;
        }

        //Public Functions
        public override void Load(ContentManager Content)
        {
            moveSound = Content.Load<SoundEffect>("Sounds/25761__wolfsinger__space-engine");
            moveLoop = moveSound.CreateInstance();
            moveLoop.IsLooped = true;
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
            System.Console.WriteLine("Player health now " + health.ToString() + " after " + damageTaken + " damage taken");
            if (health > 0)
            {
                return;
            }
            //Unload();
            base.OnCollision(damageTaken);
        }

        private void keyboardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            int fireRate = (int)MathHelper.Clamp(250 / (kills + 1), 20, 300); // shots per second increase with kills
            if (Environment.TickCount - timeLastFired > fireRate)
            {
                if (keyState.IsKeyDown(shoot))//&& lastKeyState.IsKeyUp(shoot))
                {
                    float barrelTip = this.GetPosition().Y - (texture.Height / 2) - 10;
                    Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
                    Laser shotLaser = new Laser(muzzle, CollisionManager.Side.Player);
                    shotLaser.Load(Shmup.contentManager);
                    Shmup.GameObjects.Add(shotLaser);
                    timeLastFired = Environment.TickCount;
                }
            }
            if (keyState.IsKeyDown(moveDN)) direction.Y++;
            else if (keyState.IsKeyDown(moveUP)) direction.Y--;
            if (keyState.IsKeyDown(moveRT)) direction.X++;
            else if (keyState.IsKeyDown(moveLT)) direction.X--;
            direction.Normalize();
            if (keyState.IsKeyUp(moveLT) && keyState.IsKeyUp(moveRT) && keyState.IsKeyUp(moveUP) && keyState.IsKeyUp(moveDN))
            {
                moveLoop.Pause();
            }
            else
            {
                velocity = direction * speed;
                moveLoop.Play();
            }
            //            lastKeyState = keyState;
        }

        private void gamepadInput()
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            int fireRate = (int)MathHelper.Clamp(250 / (kills + 1), 20, 300); // shots per second increase with kills
            if (Environment.TickCount - timeLastFired > fireRate)
            {
                if (padState.Buttons.A == ButtonState.Pressed)
                {
                    float barrelTip = this.GetPosition().Y - (texture.Height / 2) - 10;
                    Vector2 muzzle = new Vector2(this.GetPosition().X, barrelTip);
                    Laser shotLaser = new Laser(muzzle, CollisionManager.Side.Player);
                    shotLaser.Load(Shmup.contentManager);
                    Shmup.GameObjects.Add(shotLaser);
                    timeLastFired = Environment.TickCount;
                }
            }
            if (padState.DPad.Down == ButtonState.Pressed) direction.Y++;
            else if (padState.DPad.Up == ButtonState.Pressed) direction.Y--;
            if (padState.DPad.Right == ButtonState.Pressed) direction.X++;
            else if (padState.DPad.Left == ButtonState.Pressed) direction.X--;
            direction.Normalize();
            if (padState.IsButtonUp(Buttons.DPadLeft) && padState.IsButtonUp(Buttons.DPadRight) &&
                 padState.IsButtonUp(Buttons.DPadUp) && padState.IsButtonUp(Buttons.DPadDown))
            {
                moveLoop.Pause();
            }
            else
            {
                velocity = direction * speed;
                moveLoop.Play();
            }
        }

        public override void Update()
        {
            direction = Vector2.Zero;
            velocity = Vector2.Zero;

            #if WINDOWS
                keyboardInput();
            #elif XBOX
                gamepadInput();
            #endif

            base.Update();
            
            //Clamp player's movements so that they can't go off screen.
            position = new Vector2(
                MathHelper.Clamp(position.X, 0+(texture.Width/2), Shmup.SCREEN_WIDTH-(texture.Width/2)),
                MathHelper.Clamp(position.Y, 0+(texture.Height/2), Shmup.SCREEN_HEIGHT-(texture.Height/2)));
        }

        public static void addKill()
        {
            kills++;
        }
        public override void Draw(GameTime gameTime)
        {
            Rectangle r = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2), 
                                        (int)size.X, (int)size.Y);
            Shmup.spriteBatch.Draw(texture, r, Color.White);

//            Shmup.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Shmup.spriteBatch.DrawString(Shmup.contentManager.Load<SpriteFont>("Sprites/SpriteFont1"),
                "Player: " + health.ToString(), new Vector2(20, 40), Color.White);
            Shmup.spriteBatch.DrawString(Shmup.contentManager.Load<SpriteFont>("Sprites/SpriteFont1"),
                "Kills: " + kills.ToString(), new Vector2(20, 60), Color.White);
//            Shmup.spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
