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
    class StarObject : SpriteObject
    {

        private GameHost _game;
        public StarObject(GameHost game, Texture2D texture) : base(game, Vector2.Zero, texture)
        {
            _game = game;
            PositionX = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Width);
            PositionY = GameHelper.RandomNext(0, _game.GraphicsDevice.Viewport.Bounds.Height);
            LayerDepth = 1.0f;
        }
    }
}
