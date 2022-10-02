using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;

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

        int widthSpace;
        int heightSpace;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1000;   //Ändrar storlek på fönstret
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            moleTex = Content.Load<Texture2D>("mole");
            grassTex = Content.Load<Texture2D>("grass");
            holeTex = Content.Load<Texture2D>("molehole");
            bgTex = Content.Load<Texture2D>("bgmole"); 

           
            mole2DArray = new Mole[3, 3];


            for (int i = 0; i < mole2DArray.GetLength(0); i++)
            {
                for (int j = 0; j < mole2DArray.GetLength(1); j++)
                {
                    widthSpace = j * moleTex.Width * 2;
                    heightSpace = i * (moleTex.Height + 100);
                    mole = new Mole(moleTex, holeTex, grassTex, widthSpace, heightSpace);
                    mole.Load();
                    mole2DArray[i, j] = mole;

                }


            }



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            for (int i = 0; i < mole2DArray.GetLength(0); i++)
            {
                for (int j = 0; j < mole2DArray.GetLength(1); j++)
                {
                    mole2DArray[i, j].Update();

                }


            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,null);
            GraphicsDevice.Clear(Color.LawnGreen);
            //spriteBatch.Draw(bgTex, new Vector2(0, 0), null, Color.White, 0, new Vector2(0,0), 1, SpriteEffects.None, 0.05f);
            
                for (int i = 0; i < mole2DArray.GetLength(0); i++)
                    for (int j = 0; j < mole2DArray.GetLength(1); j++)
                    {
                        mole2DArray[i, j].Draw(spriteBatch);
                    }
            


            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}