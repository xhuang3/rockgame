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
    class RockObject : SpriteObject
    {
        private CosmicRocks _game;
        // The speed value passed to the constructor
        private float _constructorSpeed;
        // The actual speed at which the rock is falling
        private float _moveSpeed;
        // The direction of movement
        private Vector2 _direction;
        // The rate at which the rock is rotating
        private float _rotateSpeed;
        // The rock generation
        // This will be decremented by 1 each time the rock is hit and divides into two.
        // When it reaches zero, the rock will be destroyed.
        private int _generation;
        internal RockObject(CosmicRocks game, Texture2D texture, int generation, float size, float speed) : 
            base(game, Vector2.Zero, texture)
        {
            _game = game;
            _constructorSpeed = speed;
            _generation = generation;

            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);

            Origin = new Vector2(texture.Width, texture.Height) / 2;
            InitializeRock(size);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position += _moveSpeed * _direction;
            if (BoundingBox.Bottom < _game.GraphicsDevice.Viewport.Bounds.Top && _direction.Y < 0)
            {
                PositionY = _game.GraphicsDevice.Viewport.Bounds.Height + SpriteTexture.Height;
            }
            if (BoundingBox.Top > _game.GraphicsDevice.Viewport.Bounds.Bottom && _direction.Y > 0)
            {
                PositionY = -SpriteTexture.Height;
            }
            if (BoundingBox.Right < _game.GraphicsDevice.Viewport.Bounds.Left && _direction.X < 0)
            {
                PositionX = _game.GraphicsDevice.Viewport.Bounds.Width + SpriteTexture.Width;
            }
            if (BoundingBox.Left > _game.GraphicsDevice.Viewport.Bounds.Right && _direction.X > 0)
            {
                PositionX = -SpriteTexture.Width;
            }
            Angle += MathHelper.ToRadians(_rotateSpeed);
        }

        private void InitializeRock(float size)
        {
            _moveSpeed = _constructorSpeed + GameHelper.RandomNext(0, _constructorSpeed);
            _rotateSpeed = GameHelper.RandomNext(-5.0f, 5.0f);
            do
            {
                _direction = new Vector2(GameHelper.RandomNext(-1.0f, 1.0f), GameHelper.RandomNext(-1.0f, 1.0f));
            } while (_direction == Vector2.Zero);
            

            _direction.Normalize();
        }

        internal void DamageRock()
        {
            RockObject newRock;
            ParticleObject[] particleObjects;
            ParticleObject particleObj;

            particleObjects = _game.GetParticleObjects(5);
            for(int i = 0; i < particleObjects.Length; ++i)
            {
                particleObj = particleObjects[i];
                particleObj.ResetProperties(Position, _game.Textures["SmokeParticle"]);
                switch(GameHelper.RandomNext(3))
                {
                    case 0: particleObj.SpriteColor = Color.DarkGray; break;
                    case 1: particleObj.SpriteColor = Color.LightGray; break;
                    default: particleObj.SpriteColor = Color.White; break;
                }
                particleObj.Scale = new Vector2(0.4f, 0.4f);
                particleObj.IsActive = true;
                particleObj.Intensity = 255;
                particleObj.IntensityFadeAmount = 3 + GameHelper.RandomNext(1.5f);
                particleObj.Speed = GameHelper.RandomNext(3.0f);
                particleObj.Inertia = 0.9f + GameHelper.RandomNext(0.015f);
            }

            if(_generation == 0)
            {
                _game.GameObjects.Remove(this);
            }
            else
            {
                InitializeRock(ScaleX * 0.7f);
                _generation -= 1;
                // Create another rock alongside this one
                newRock = new RockObject(_game, SpriteTexture, _generation, ScaleX, _constructorSpeed);
                // Position the new rock exactly on top of this rock
                newRock.Position = Position;
                // Add the new rock to the game
                _game.GameObjects.Add(newRock);
            }
        }
    }
}
