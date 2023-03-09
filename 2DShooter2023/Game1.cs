using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Emit;


namespace _2DShooter2023
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        float mouseTargetDist;
        Texture2D target_Sprite;
        Texture2D falling_Sprite;
        Texture2D crosshair_Sprite;
        Texture2D background_Sprite;
        Vector2 targetPosition = new Vector2(300, 300);
        Vector2 fallingPosition = new Vector2(700, 700);
        SpriteFont gameFont;
        MouseState mState;
        const int TARGET_RADIUS = 45;
        int score = 0;
        Boolean mRelease = true;
        float timer = 10f; // count down for 10 seconds
        int duckSpeed = 2;
        private Vector2 cursorPos;
        new Vector2 score_location = new Vector2(400, 10);
        gameState game_started = gameState.start;
        bool goRight = true;
        private Vector2 timer_location = new Vector2(0, 0);
        private Vector2 enter_location = new Vector2(175,200);
        private Vector2 exit_location = new Vector2(900, 900);

        public enum gameState
        {
            start,
            end,
            play
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            target_Sprite = Content.Load<Texture2D>("duck_target_white");
            crosshair_Sprite = Content.Load<Texture2D>("crosshair_blue_small");
            background_Sprite = Content.Load<Texture2D>("forest");
            gameFont = Content.Load<SpriteFont>("galleryFont");
            falling_Sprite = Content.Load<Texture2D>("duck_target_white");
            gameState game_started = gameState.start;
        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (game_started)
            {
                
                case gameState.start:
                    timer = 10f;
                    score_location = new Vector2(400, 10);
                    targetPosition = new Vector2(300, 300);
                    fallingPosition = new Vector2(700, 700);
                    enter_location = new Vector2(175, 200);
                    exit_location = new Vector2(900, 900);
                    timer_location = new Vector2(0, 0);
                    score = 0;
                    duckSpeed = 2;
                    mState = Mouse.GetState();
                    cursorPos = new Vector2(mState.X, mState.Y);
                    mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));
                    if (mState.LeftButton == ButtonState.Pressed && mRelease == true && mouseTargetDist < TARGET_RADIUS)
                    {
                        game_started = gameState.play;
                    }
                    break;
                case gameState.play:
                    enter_location = new Vector2(900, 900);
                    if (timer > 0)
                    {
                        timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (fallingPosition.Y < 700)
                        {
                            fallingPosition.Y += 14;
                        }
                        if (!goRight)
                        {
                            targetPosition.X -= duckSpeed;

                            if (targetPosition.X < TARGET_RADIUS)
                            {
                                Random rand = new Random();
                                targetPosition.X =  TARGET_RADIUS;
                                targetPosition.Y = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                                duckSpeed = rand.Next(2, 6);
                                goRight = true;
                            }
                        }
                        else
                        {
                            targetPosition.X += duckSpeed;

                            if (targetPosition.X > _graphics.PreferredBackBufferWidth - TARGET_RADIUS)
                            {
                                Random rand = new Random();
                                targetPosition.X = _graphics.PreferredBackBufferWidth - TARGET_RADIUS;
                                targetPosition.Y = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                                duckSpeed = rand.Next(2, 6);
                                goRight = false;
                            }
                        }
                        if (mState.LeftButton == ButtonState.Pressed && mRelease == true)
                        {
                            if (mouseTargetDist < TARGET_RADIUS)
                            {
                                score++;
                                Random rand = new Random();
                                fallingPosition.X = targetPosition.X;
                                fallingPosition.Y = targetPosition.Y;
                                targetPosition.X = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1); 
                                targetPosition.Y = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                                duckSpeed = rand.Next(2, 6);
                            }
                            mRelease = false;
                        }
                    }
                    mState = Mouse.GetState();
                    cursorPos = new Vector2(mState.X, mState.Y);
                    mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));

                    if (mState.LeftButton == ButtonState.Released)
                    {
                        mRelease = true;
                    }
                    base.Update(gameTime);
                    if (timer <= 0)
                    {
                        game_started = gameState.end;
                    }
                    break;
                case gameState.end:
                    score_location = new Vector2(900, 900);
                    targetPosition = new Vector2(900, 900);
                    fallingPosition = new Vector2(900, 900);
                    timer_location = new Vector2(900, 900);
                    exit_location = new Vector2(200, 200);
                    mState = Mouse.GetState();
                    cursorPos = new Vector2(mState.X, mState.Y);
                    mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));

                    if (mState.LeftButton == ButtonState.Released)
                    {
                        mRelease = true;
                    }
                    if (mState.LeftButton == ButtonState.Pressed && mRelease == true)
                    {
                        game_started = gameState.start;
                    }
                        break;
            }
            // TODO: Add your update logic here

            
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(gameFont, score.ToString(), score_location, Color.Black);
            _spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
            _spriteBatch.Draw(falling_Sprite, new Vector2(fallingPosition.X - TARGET_RADIUS, fallingPosition.Y - TARGET_RADIUS), Color.White);
            _spriteBatch.DrawString(gameFont, Math.Ceiling(timer).ToString(), timer_location, Color.Black);
            _spriteBatch.Draw(crosshair_Sprite, cursorPos, Color.White);
            _spriteBatch.DrawString(gameFont, "Shoot Duck To Start!", enter_location, Color.Black);
            _spriteBatch.DrawString(gameFont, "Time's Up! \nYour Score is "+score.ToString()+"! \nClick Mouse To Return To Start!", exit_location, Color.Black);
            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}