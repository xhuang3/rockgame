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
    }
}
