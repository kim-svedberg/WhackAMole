﻿using Microsoft.Xna.Framework;
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
        Texture2D stoneSheet;
        Texture2D malletTex;

        SpriteFont timeFont;
        SpriteFont scoreFont;
        SpriteFont menuFont;
        SpriteFont gameOverFont;

        int widthSpace;
        int heightSpace;
        int gameTimer = 60;
        int visualTime = 30;
        double timeSinceLastFrame = 0;
        double timeBetweenFrames = 0.1;

        Point sheetSize;
        Point frameSize;
        Point currentFrame;

        MouseState mouseState;
        Vector2 mousePos;

        Random rnd = new Random();

        enum GameState { Menu, Play, GameOver }
        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            //Ändrar storlek på fönstret
            graphics.PreferredBackBufferWidth = 1250;   
            graphics.PreferredBackBufferHeight = 1250;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Ser till så att programmet inte kraschar om datorn inte klarar av bilderna
            if (graphics.GraphicsDevice.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
                graphics.GraphicsProfile = GraphicsProfile.HiDef;
            else
                graphics.GraphicsProfile = GraphicsProfile.Reach;  

            graphics.ApplyChanges();

            //Börjar i menyn när spelet börjar
            gameState = GameState.Menu; 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Alla bilder laddas in
            spriteBatch = new SpriteBatch(GraphicsDevice);
            moleTex = Content.Load<Texture2D>("mole");
            grassTex = Content.Load<Texture2D>("grass");
            holeTex = Content.Load<Texture2D>("molehole");
            bgTex = Content.Load<Texture2D>("bgmole");
            moleHitTex = Content.Load<Texture2D>("molehit2");
            menuTex = Content.Load<Texture2D>("blue");
            gameOverTex = Content.Load<Texture2D>("orange");
            stoneSheet = Content.Load<Texture2D>(@"spritesheet_stone");
            malletTex = Content.Load<Texture2D>("mallet");

            //Text/fonts laddas in
            timeFont = Content.Load<SpriteFont>("File");
            scoreFont = Content.Load<SpriteFont>("score");
            menuFont = Content.Load<SpriteFont>("menuFont");
            gameOverFont = Content.Load<SpriteFont>("gameOverFont");

            //En array som utnyttjar att enum-states kan skrivas som siffror. Ger bakgrund till olika Game states. 
            gameStateTex = new Texture2D[3];
            gameStateTex[(int)GameState.Play] = bgTex;
            gameStateTex[(int)GameState.Menu] = menuTex;
            gameStateTex[(int)GameState.GameOver] = gameOverTex;

            //Beskriver hur stor spritesheeten är till animeringen. 
            sheetSize = new Point(4, 4);
            frameSize = new Point(258 / 4 - 1, 258 / 4);
            currentFrame = new Point(0, 0);


            CreateArray();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MousePosition();

            GameStates(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.BackToFront, null);

            DrawBGForStates(spriteBatch);
            DrawStateSprites(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
     
        }
       
        public void CreateArray() //Sätter in mullvadar, gräs och hål i en 2D array. 
        {
            mole2DArray = new Mole[3, 3];


            for (int i = 0; i < mole2DArray.GetLength(0); i++)       
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
        public void DrawBGForStates(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameStateTex[(int)gameState],
                 new Vector2(0, 0),
                 null, Color.White,
                 0, new Vector2(0, 0),
                 2,
                 SpriteEffects.None,
                 1f);
        }
        public void DrawStateSprites(SpriteBatch spriteBatch)
        {
            if (gameState == GameState.Play)
            {
                //Samma färg på gräset som bakgrundsbilden
                Color grassGreen = new Color(111, 209, 72, 255);
                GraphicsDevice.Clear(grassGreen);

                //Overhead Display (HUD)
                spriteBatch.DrawString(timeFont, "TIME: " + visualTime, new Vector2(50, 50), Color.White);
                spriteBatch.DrawString(scoreFont, "SCORE: " + Mole.userScore, new Vector2(50, 120), Color.White);
                spriteBatch.DrawString(scoreFont, "LIVES: " + Mole.userLives, new Vector2(500, 50), Color.White);

                for (int i = 0; i < mole2DArray.GetLength(0); i++)
                    for (int j = 0; j < mole2DArray.GetLength(1); j++)
                    {
                        mole2DArray[i, j].Draw(spriteBatch);
                    }

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    spriteBatch.Draw(malletTex, mousePos, new Rectangle(0, 0, malletTex.Width, malletTex.Height), Color.White, rotation: 0f, new Vector2(malletTex.Width, malletTex.Height), 1, SpriteEffects.None, 0f);

                }
                else
                {
                    spriteBatch.Draw(malletTex, mousePos, new Rectangle(0, 0, malletTex.Width, malletTex.Height), Color.White, 0.3f, new Vector2(malletTex.Width, malletTex.Height), 1, SpriteEffects.None, 0f);
                }

            }

            if (gameState == GameState.Menu)
            {
                //Animeringen i menyn 
                Rectangle source = new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y);
                spriteBatch.Draw(stoneSheet, new Vector2(Window.ClientBounds.Width / 2 - 300, Window.ClientBounds.Height / 2 - 70), source, Color.White);

                spriteBatch.DrawString(menuFont, "Press ENTER to play!", new Vector2(Window.ClientBounds.Width / 2 - 175, Window.ClientBounds.Height / 2 - 70), Color.White);
            }

            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(gameOverFont, "GAME OVER! ESC to quit. \n Score: " + Mole.userScore, new Vector2(Window.ClientBounds.Width / 2 - 375, Window.ClientBounds.Height / 2 - 70), Color.White);
            }

        }

        public void MousePosition()
        {
            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X + malletTex.Width - 40, mouseState.Y + 40);
        }

        public void GameStates(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    {
                        timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
                        if (timeSinceLastFrame >= timeBetweenFrames)
                        {
                            timeSinceLastFrame -= timeBetweenFrames;
                            currentFrame.X++;
                            if (currentFrame.X >= sheetSize.X)

                            {
                                currentFrame.X = 0;
                                currentFrame.Y++;

                                if (currentFrame.Y >= sheetSize.Y)
                                {
                                    currentFrame.Y = 0;
                                }
                            }

                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))

                        {
                            gameState = GameState.Play;
                        }

                    }
                    break;

                case GameState.Play:
                    {
                        gameTimer--;
                        if (gameTimer <= 0)
                        {
                            visualTime--;
                            gameTimer = 60;
                            Mole.spawnTimer -= 10;
                        }



                        for (int i = 0; i < mole2DArray.GetLength(0); i++)    //Uppdaterar positioner och hastighet i arrayen utifrån min mullvads-klass
                        {
                            for (int j = 0; j < mole2DArray.GetLength(1); j++)
                            {

                                mole2DArray[i, j].Update(rnd);

                            }


                        }

                        if (visualTime <= 0 || Mole.userLives <= 0) //Ska vara: när tiden når noll. 
                        {
                            gameState = GameState.GameOver;


                        }



                    }
                    break;

                case GameState.GameOver:
                    break;

            }
        }
        
         
        
    }


}