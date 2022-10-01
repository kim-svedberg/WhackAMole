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

        Texture2D moleTex;
        Texture2D grassTex;
        Texture2D holeTex;

        int col;
        int row; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1000;   //Ändrar storlek på fönstret
            graphics.PreferredBackBufferHeight = 750;
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

            //mole = new Mole(moleTex, holeTex, grassTex, new Vector2(0, 0), new Vector2(0, 0));
            mole2DArray = new Mole[3, 3];
            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    col = i * 300;
                    row = j * 300;
                    mole2DArray[i, j] = new Mole(moleTex, holeTex, grassTex, col, row);
                }
            }




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,null);
            GraphicsDevice.Clear(Color.LawnGreen);

            for (int i = 0; i < mole2DArray.GetLength(0); i++)
                for (int j = 0; j < mole2DArray.GetLength(1); j++)
                {
                    mole2DArray[i,j].Draw(spriteBatch);

                }


            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}