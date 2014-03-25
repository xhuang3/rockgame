using GameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CosmicRocks
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CosmicRocks : GameHost
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public CosmicRocks()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.IsMouseVisible = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            Textures.Add("Spaceship", this.Content.Load<Texture2D>("Spaceship"));
            //Textures.Add("Bullet", this.Content.Load<Texture2D>("Bullet"));
            Textures.Add("Rock1", this.Content.Load<Texture2D>("Rock1"));
            Textures.Add("Rock2", this.Content.Load<Texture2D>("Rock2"));
            Textures.Add("Rock3", this.Content.Load<Texture2D>("Rock3"));
            Textures.Add("Star", this.Content.Load<Texture2D>("Star"));
            Textures.Add("SmokeParticle", this.Content.Load<Texture2D>("SmokeParticle"));

            Fonts.Add("Miramonte", this.Content.Load<SpriteFont>("Miramonte"));
            // TODO: use this.Content to load your game content here
            ResetGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            UpdateAll(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawSprites(gameTime, spriteBatch, Textures["Star"]);
            DrawSprites(gameTime, spriteBatch, Textures["Rock1"]);
            DrawSprites(gameTime, spriteBatch, Textures["Rock2"]);
            DrawSprites(gameTime, spriteBatch, Textures["Rock3"]);
            DrawSprites(gameTime, spriteBatch, Textures["Spaceship"]);

            DrawText(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            DrawSprites(gameTime, spriteBatch, Textures["SmokeParticle"]);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            string rockTextureName;
            GameObjects.Clear();
            for (int i = 0; i < 5; ++i)
            {
                rockTextureName = "Rock" + GameHelper.RandomNext(1, 4).ToString();
                GameObjects.Add(new RockObject(this, Textures[rockTextureName], 3, 0.5f, 2.0f));
            }
            for (int i = 0; i < 50; ++i)
            {
                GameObjects.Add(new StarObject(this, Textures["Star"]));
            }
            GameObjects.Add(new SpaceshipObject(this, Textures["Spaceship"]));
        }

        internal ParticleObject[] GetParticleObjects(int particleCount)
        {
            ParticleObject[] particles = new ParticleObject[particleCount];
            GameObjectBase obj;
            int particleIndex = 0;
            int objectCount = GameObjects.Count;

            for(int i = 0; i < objectCount; i++)
            {
                obj = GameObjects[i];
                if(obj is ParticleObject)
                {
                    if(((ParticleObject)obj).IsActive == false)
                    {
                        particles[particleIndex] = (ParticleObject)obj;
                        particleIndex += 1;
                        if (particleIndex == particleCount) break;
                    }
                }
            }
            for (; particleIndex < particleCount; particleIndex++)
            {
                // Add a new object to the array
                particles[particleIndex] = new ParticleObject(this);
                // Add to the GameObjects list
                GameObjects.Add(particles[particleIndex]);
            }

            return particles;
        }
    }
}
