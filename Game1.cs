using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimationTest2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Explosion _explosion; //this is our silly explosion object
        
        private Goblin _goblin; //this is our awesome goblin object

        private RatFolk _ratFolk;

        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //explosion is easy
            _explosion = new Explosion(Content.Load<Texture2D>("explosionSpriteSheet"), 3, 3);
            
            //goblin needs a bit of work. We could easily put this in a loop and do it 
            //for many goblins. Cool.
            _goblin = new Goblin(100, 100);
            _goblin.loadAttackSprite(Content.Load<Texture2D>("goblinAttackSpriteSheet"), 1, 9);
            _goblin.loadIdleSprite(Content.Load<Texture2D>("goblinIdleSpriteSheet"), 1, 8);
            _goblin.loadWalkSprite(Content.Load<Texture2D>("goblinWalkSpriteSheet"), 1, 8);


            //ratfolk just needs the sprite sheet and the rows and columns, no work gets done after the fact
            _ratFolk = new RatFolk(300, 300, Content.Load<Texture2D>("RatfolkTamerSpritesheet"), 6, 8);

            _font = Content.Load<SpriteFont>("GoblinFont");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //call our object's updates
            _explosion.Update();
            _goblin.Update();
            _ratFolk.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            //just some text...
            _spriteBatch.DrawString(_font, "Animation demo - not a real game.", new Vector2(200,15), Color.White);
            _spriteBatch.DrawString(_font, "Goblin - Arrow keys + Space. Rat - WASD + LShift.", new Vector2(80, 440), Color.White);
            _spriteBatch.End();

            //call our object's draws
            _explosion.Draw(_spriteBatch);
            _goblin.Draw(_spriteBatch);
            _ratFolk.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}