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

        SpriteFont timeFont;

        int widthSpace;
        int heightSpace;

        Random rnd = new Random();

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
            timeFont = Content.Load<SpriteFont>("File");

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


            for (int i = 0; i < mole2DArray.GetLength(0); i++)    //Uppdaterar positioner och hastighet i arrayen utifrån min mullvads-klass
            {
                for (int j = 0; j < mole2DArray.GetLength(1); j++)
                {

                    mole2DArray[i, j].Update(rnd);

                }


            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin(SpriteSortMode.BackToFront,null);

            spriteBatch.DrawString(timeFont, "TIME: ", new Vector2(50,50), Color.White);

            spriteBatch.Draw(bgTex, 
                new Vector2(0, 0), 
                null, Color.White, 
                0, new Vector2(0, 0), 
                2, 
                SpriteEffects.None, 
                1f);
       
            Color grassGreen = new Color(111, 209, 72, 255);
                   GraphicsDevice.Clear(grassGreen);


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