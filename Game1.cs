using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame_Part_2___Lists_and_Loops
{
    public class Game1 : Game
    {
        enum Screen 
        { 
            intro,
            game,
            outro
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Random gen = new Random();
        Texture2D backgroundTexture;
        List<Texture2D> planetTextures;
        List<Rectangle> rects;
        int width, height;  
        Screen screen;
        SpriteFont spriteFont;
        KeyboardState keyboardState;
        KeyboardState prevKeyboardState;
        MouseState mouseState;
        MouseState prevMouseState;
        Texture2D mouseTexture;
        Point mousePosition;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;
            screen = Screen.intro;
        }

        protected override void Initialize()
        {

            rects = new List<Rectangle>();
            for (int i = 0; i < 30; i++)
            {
                bool pass = true;
                Rectangle rect = new();
                do
                {
                    pass = true;
                    int length = gen.Next(10,21) * 5;
                    rect = new Rectangle(gen.Next(width - length), gen.Next(height - length), length, length);
                    for (int j = 0; j < i; j++)
                    {
                        if (rect.Intersects(rects[j]))
                        {
                            pass = false;
                        }
                    }
                }
                while (!pass);
                rects.Add(rect);
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("space_background");
            planetTextures = new List<Texture2D>();
            for (int i = 1; i < 14; i++)
            {
                planetTextures.Add(Content.Load<Texture2D>("16-bit-planet" + i));
            }
            spriteFont = Content.Load<SpriteFont>("SpriteFont");
            mouseTexture = Content.Load<Texture2D>("hammer");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (screen == Screen.intro)
            {
                if (keyboardState.IsKeyUp(Keys.Space) && prevKeyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.game;
                }
            }
            else if (screen == Screen.game)
            {
                this.Window.Title = mouseState.ScrollWheelValue.ToString();
                if (Math.Abs(mouseState.ScrollWheelValue%360) >= 0&& Math.Abs(mouseState.ScrollWheelValue % 360) <120)
                {
                    mouseTexture = Content.Load<Texture2D>("hammer");
                }
                else if (Math.Abs(mouseState.ScrollWheelValue % 360) >= 120 && Math.Abs(mouseState.ScrollWheelValue % 360) < 240)
                {
                    mouseTexture = Content.Load<Texture2D>("laser_gun");
                }
                else if (Math.Abs(mouseState.ScrollWheelValue % 360) >= 240 && Math.Abs(mouseState.ScrollWheelValue % 360) < 360)
                {
                    mouseTexture = Content.Load<Texture2D>("black_hole");
                }
                mousePosition = mouseState.Position;
            }
            else if (screen == Screen.outro)
            {
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, width, height), Color.White);
            if (screen == Screen.intro)
            {
                _spriteBatch.DrawString(spriteFont,"Press SPACE to Start",new Vector2(width/2, height/2) - spriteFont.MeasureString("Press SPACE to Start")/2,Color.White);  
            }
            else if (screen == Screen.game)
            {
                for (int i = 0; i < planetTextures.Count; i++)
                {
                    _spriteBatch.Draw(planetTextures[i], rects[i], Color.White);
                }
                _spriteBatch.Draw(mouseTexture, new Rectangle(mousePosition.X-25,mousePosition.Y-25,50,50), Color.White);
            }
            else if (screen == Screen.outro)
            {
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
