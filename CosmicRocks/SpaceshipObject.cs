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
        }
    }
}
