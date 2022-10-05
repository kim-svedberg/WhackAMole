using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Threading;


namespace WhackAMole
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Mole[,] mole2DArray;
        Mole mole;

        Texture2D moleTex;
        Texture2D grassTex;
        Texture2D holeTex;
        Texture2D bgTex;
        Texture2D moleHitTex;
        Texture2D menuTex;
        Texture2D gameOverTex;
        Texture2D[] gameStateTex;


        SpriteFont timeFont;
        SpriteFont scoreFont;
        SpriteFont menuFont;
        SpriteFont gameOverFont;

        int widthSpace;
        int heightSpace;

        Random rnd = new Random();

        enum GameState { Menu, Play, GameOver }
        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1250;   //Ändrar storlek på fönstret
            graphics.PreferredBackBufferHeight = 1250;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            if (graphics.GraphicsDevice.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
                graphics.GraphicsProfile = GraphicsProfile.HiDef;
            else
                graphics.GraphicsProfile = GraphicsProfile.Reach;

            graphics.ApplyChanges();

            //gameState = GameState.Menu;
            gameState = GameState.GameOver;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            moleTex = Content.Load<Texture2D>("mole");
            grassTex = Content.Load<Texture2D>("grass");
            holeTex = Content.Load<Texture2D>("molehole");
            bgTex = Content.Load<Texture2D>("bgmole");
            moleHitTex = Content.Load<Texture2D>("molehit2");
            menuTex = Content.Load<Texture2D>("blue");
            gameOverTex = Content.Load<Texture2D>("orange");

            timeFont = Content.Load<SpriteFont>("File");
            scoreFont = Content.Load<SpriteFont>("score");
            menuFont = Content.Load<SpriteFont>("menuFont");
            gameOverFont = Content.Load<SpriteFont>("gameOverFont");



            gameStateTex = new Texture2D[3];
            gameStateTex[(int)GameState.Play] = bgTex;
            gameStateTex[(int)GameState.Menu] = menuTex;
            gameStateTex[(int)GameState.GameOver] = gameOverTex;
            




            mole2DArray = new Mole[3, 3];


            for (int i = 0; i < mole2DArray.GetLength(0); i++)       //Sätter in mullvadarna i en array
            {
                for (int j = 0; j < mole2DArray.GetLength(1); j++)
                {
                    widthSpace = j * moleTex.Width * 2;
                    heightSpace = i * (moleTex.Height + 100);
                    mole = new Mole(moleTex, holeTex, grassTex, moleHitTex, widthSpace, heightSpace);
                    mole.Load();

                    mole2DArray[i, j] = mole;

                }


            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch(gameState)
            {
                case GameState.Menu:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            gameState = GameState.Play;
                        }
                    }
                    break;

                case GameState.Play:
                    {

                        for (int i = 0; i < mole2DArray.GetLength(0); i++)    //Uppdaterar positioner och hastighet i arrayen utifrån min mullvads-klass
                        {
                            for (int j = 0; j < mole2DArray.GetLength(1); j++)
                            {

                                mole2DArray[i, j].Update(rnd);

                            }


                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Z)) //Ska vara: när tiden når noll. 
                        {
                            gameState = GameState.GameOver;
                        }

                    }
                    break;

                case GameState.GameOver:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.X))
                        {
                            gameState = GameState.Play;
                        }
                        
                    }
                    break;

            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin(SpriteSortMode.BackToFront,null);
           


            spriteBatch.Draw(gameStateTex[(int)gameState],
                    new Vector2(0, 0),
                    null, Color.White,
                    0, new Vector2(0, 0),
                    2,
                    SpriteEffects.None,
                    1f);

            if (gameState == GameState.Play)
            {
                Color grassGreen = new Color(111, 209, 72, 255);
                GraphicsDevice.Clear(grassGreen);

                spriteBatch.DrawString(timeFont, "TIME: ", new Vector2(50, 50), Color.White);
                spriteBatch.DrawString(scoreFont, "SCORE: ", new Vector2(50, 120), Color.White);

                for (int i = 0; i < mole2DArray.GetLength(0); i++)
                    for (int j = 0; j < mole2DArray.GetLength(1); j++)
                    {
                        mole2DArray[i, j].Draw(spriteBatch);
                    }

            }

            if(gameState == GameState.Menu)
            {
                spriteBatch.DrawString(menuFont, "Press ENTER to play!", new Vector2(Window.ClientBounds.Width / 2 - 175, Window.ClientBounds.Height / 2 + 55), Color.White);
            }

            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(gameOverFont, "GAME OVER! X to play again. ESC to quit.", new Vector2(Window.ClientBounds.Width / 2 - 375, Window.ClientBounds.Height / 2 + 55), Color.White);
            }

            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}

//TODO: 
// 1. score
// 2. tid
// 3. game-states
// 4. animering