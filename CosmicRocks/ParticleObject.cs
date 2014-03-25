using GameFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CosmicRocks
{
    class ParticleObject : SpriteObject
    {
        private GameHost _game;


        public ParticleObject(GameHost game) : base(game)
        {
            _game = game;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActive) return;
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            Position += Direction * Speed;
            Speed *= Inertia;
            Intensity -= IntensityFadeAmount;
            if(Intensity <= 0)
            {
                IsActive = false;
            }
        }

        internal void ResetProperties(Vector2 position, Texture2D texture2D)
        {
            Position = position;
            SpriteTexture = texture2D;
            Origin = new Vector2(SpriteTexture.Width / 2, SpriteTexture.Height / 2);
            Angle = MathHelper.ToRadians(GameHelper.RandomNext(360.0f));

            IsActive = true;
            Intensity = 255;
            IntensityFadeAmount = 3;

            do
            {
                Direction = new Vector2(GameHelper.RandomNext(-1.0f, 1.0f), GameHelper.RandomNext(-1.0f, 1.0f));
            } while (Direction == Vector2.Zero);
            Direction.Normalize();
            Inertia = 1.0f;
        }

        internal bool IsActive { get; set; }

        internal Vector2 Direction { get; set; }

        internal float Intensity { get; set; }

        internal float IntensityFadeAmount { get; set; }

        internal float Speed { get; set; }

        internal float Inertia { get; set; }

        public override Color SpriteColor
        {
            get
            {
                return base.SpriteColor;
            }
            set
            {
                base.SpriteColor = value;
            }
        }
    }
}
