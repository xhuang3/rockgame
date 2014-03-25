using GameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmicRocks
{
    class SpaceshipObject : SpriteObject
    {
        private GameHost _game;
        public SpaceshipObject(GameHost game, Texture2D texture) : base(game, Vector2.Zero, texture)
        {
            _game = game;
            Origin = new Vector2(texture.Width, texture.Height) / 2;
            PositionX = _game.GraphicsDevice.Viewport.Bounds.Width / 2;
            PositionY = _game.GraphicsDevice.Viewport.Bounds.Height / 2;
            Scale = new Vector2(0.2f, 0.2f);
            ExplosionUpdateCount = 0;
        }

        internal SpriteObject HasCollided()
        {
            SpriteObject spriteObj;
            Rectangle shipBox;
            float shipSize;
            float objectSize;
            float objectDistance;

            shipSize = SpriteTexture.Width / 2.0f * ScaleX;
            shipBox = BoundingBox;

            foreach(GameObjectBase gameObj in _game.GameObjects)
            {
                if(gameObj is RockObject)
                {
                    spriteObj = (SpriteObject)gameObj;
                    if(spriteObj.BoundingBox.Intersects(shipBox))
                    {
                        objectSize = spriteObj.SpriteTexture.Width / 2.0f * spriteObj.ScaleX;
                        objectDistance = Vector2.Distance(Position, spriteObj.Position);
                        if (objectDistance < shipSize + objectSize)
                        {
                            return spriteObj;
                        }
                    }
                }
            }
            return null;
        }


        private int ExplosionUpdateCount { get; set; }
        internal bool IsExploding
        {
            get { return (ExplosionUpdateCount > 0); }
        }

        public override void Update(GameTime gameTime)
        {

            SpriteObject collisionObj;

            if(IsExploding)
            {
                ExplosionUpdateCount -= 1;
            }
            else
            {
                collisionObj = HasCollided();
                if (collisionObj is RockObject)
                {
                    ((RockObject)collisionObj).DamageRock();
                }
                if (collisionObj != null)
                {
                    Explode();
                }
            }
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!IsExploding) base.Draw(gameTime, spriteBatch);
        }

        private void Explode()
        {
            ParticleObject[] particleObjects;
            ParticleObject particleObj;

            ExplosionUpdateCount = 120;

            particleObjects = ((CosmicRocks)_game).GetParticleObjects(150);

            for (int i = 0; i < particleObjects.Length; ++i)
            {
                particleObj = particleObjects[i];
                particleObj.ResetProperties(Position, _game.Textures["SmokeParticle"]);

                switch(GameHelper.RandomNext(4))
                {
                    case 0: particleObj.SpriteColor = Color.Yellow; break;
                    case 1: particleObj.SpriteColor = Color.Orange; break;
                    case 2: particleObj.SpriteColor = Color.Red; break;
                    default: particleObj.SpriteColor = Color.White; break;
                }

                particleObj.Scale = new Vector2(0.5f, 0.5f);
                particleObj.IsActive = true;
                particleObj.Intensity = 255;
                particleObj.IntensityFadeAmount = 2 + GameHelper.RandomNext(1.5f);
                particleObj.Speed = GameHelper.RandomNext(5.0f);
                particleObj.Inertia = 0.9f + GameHelper.RandomNext(0.015f);
            }
        }
    }
}
