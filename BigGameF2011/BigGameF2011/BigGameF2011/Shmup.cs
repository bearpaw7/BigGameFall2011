using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BigGameF2011.GameObjects;

namespace BigGameF2011
{
    /// <summary>
    /// This is the game container. It's where the game is run.
    /// </summary>
    public class Shmup : Microsoft.Xna.Framework.Game
    {
        //Screen Data  (80% of available screen)
        public static int SCREEN_WIDTH = (int)(0.8 * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
        public static int SCREEN_HEIGHT = (int)(0.8 * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

        //Objects used for drawing
        public static CollisionManager collisions;
        public static GraphicsDeviceManager graphics;
        public static Shmup game;
        public static SpriteBatch spriteBatch;

        //Everybody needs a bit of entropy
        public static Random random;

        //Containers for objects
        public static List<GameObject> GameObjects;

        public Shmup()
        {
            collisions = new CollisionManager(this);
            this.Components.Add(collisions);
            collisions.Initialize();

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";

            random = new Random();

            GameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize wisll enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameObjects.Add(new Background(new Vector2(0, 0)));
            GameObjects.Add(new Player(new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT * 0.9f), base.Content));

            GameObjects.Add(new Enemy(new Vector2(SCREEN_WIDTH / 3, SCREEN_HEIGHT * 0.2f), base.Content));
            GameObjects.Add(new Laser(new Vector2(25, SCREEN_HEIGHT), CollisionManager.Side.Player));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load sprites for our game objects
            foreach(GameObject obj in GameObjects)
                obj.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            for(int i = GameObjects.Count(); i != 0; i--)
                GameObjects[i-1].Unload();
        }

        // elapsed time is 3 seconds in the future as a game-start buffer
        int debugTimeElapsed = Environment.TickCount + 3000; 
        private void debugSpawnEnemy(GameTime gameTime)
        {
            // minimum, maximum time between spawning enemies
            if (Environment.TickCount - debugTimeElapsed > (random.Next(2000, 5000)))
            {
                Enemy badguy = new Enemy(new Vector2(-100, -100), base.Content);
                badguy.Load(base.Content);

                float x = (float)Shmup.random.NextDouble() * Shmup.SCREEN_WIDTH;
                float y = -1.0f * 10; // badguy.texture.Height;
                badguy.SetPosition(new Vector2(x, y));

                Shmup.GameObjects.Add(badguy);
                debugTimeElapsed = Environment.TickCount;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exits the game if Enter or gamepad Back is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                System.Console.WriteLine("DEBUG Game Objects => " + GameObjects.Count().ToString());
                System.Console.WriteLine("DEBUG Collision Objects => " + collisions.getCombinedCount().ToString());
                this.Exit();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            for (int i = 0; i < GameObjects.Count(); i++)
            {
                GameObjects[i].Update();
            }

            debugSpawnEnemy(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int i = 0; i < GameObjects.Count(); ++i)
            {
                GameObjects[i].Draw(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
