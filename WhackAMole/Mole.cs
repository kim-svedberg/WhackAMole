using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace WhackAMole
{
    internal class Mole
    {

        Texture2D moleTex;
        Texture2D holeTex;
        Texture2D grassTex;
        Texture2D moleHitTex;
        Texture2D[] moleStateTex;

        Vector2 molePos;
        Vector2 holePos;
        Vector2 grassPos;
        Vector2 moleVelo;

        int widthSpace;
        int heightSpace;
        public static int userScore; 

        Rectangle moleHitBox;
        Rectangle mouseBox;

        MouseState mouseState, oldMouseState;
        Point mousePos;

        enum MoleState
        {
            MovingUp, IsUp, MovingDown, IsDown, IsHit
        }

        MoleState moleState;


        public Mole(Texture2D moleTex, Texture2D holeTex, Texture2D grassTex, Texture2D moleHitTex, int widthSpace, int heightSpace)
        {
            this.moleTex = moleTex;
            this.holeTex = holeTex;
            this.grassTex = grassTex;
            this.widthSpace = widthSpace; //Till för att sätta avstånd mellan mull, gräs och hål
            this.heightSpace = heightSpace; //Samma som width fast för höjden istället för bredden
            this.moleHitTex = moleHitTex;

            moleStateTex = new Texture2D[5];
            moleStateTex[(int)MoleState.MovingUp] = moleTex;
            moleStateTex[(int)MoleState.IsUp] = moleTex;
            moleStateTex[(int)MoleState.MovingDown] = moleTex;
            moleStateTex[(int)MoleState.IsDown] = moleTex;
            moleStateTex[(int)MoleState.IsHit] = moleHitTex;


        }

        public void Load()
        {
            holePos = new Vector2(150 + widthSpace, 300 + heightSpace); 
            grassPos = new Vector2(150 + widthSpace, 300 + heightSpace);
            molePos = new Vector2(150 + widthSpace, grassPos.Y);
            moleVelo = new Vector2(0, 2.5f); //Hur snabbt mullvadarna går upp och ned
            moleState = MoleState.IsDown;
            mouseState = new MouseState();
            mousePos = new Point(mouseState.Position.X, mouseState.Position.Y);


        }

        public void Update(Random rnd) //Random så att varje mullvad inte måste få sin egna random
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
            mousePos = new Point(mouseState.Position.X, mouseState.Position.Y);

            int height = ((int)grassPos.Y - (int)molePos.Y);
            moleHitBox = new Rectangle((int)molePos.X, (int)molePos.Y +35, moleTex.Width, height); //Skapar mullvadens hitbox/rektangel. Ger den en dynamisk height. +35 och är bara finjustering. 
            mouseBox = new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1);

            
            switch (moleState)
            {

                case MoleState.MovingUp:
                case MoleState.IsUp:
                    if (mouseBox.Intersects(moleHitBox) && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        moleState = MoleState.IsHit;
                    }
                    break;
            }

            switch (moleState)
            {
                case MoleState.MovingUp:
                    molePos -= moleVelo;
                    if (molePos.Y < grassPos.Y - 175)
                    {
                        moleState = MoleState.IsUp;
                        molePos.Y = grassPos.Y - 175;
                    }
                    break;

                case MoleState.IsUp:
                    if (rnd.Next(1, 100) == 1)
                    {
                        moleState = MoleState.MovingDown;
                    }
                    break;

                case MoleState.MovingDown:
                    molePos += moleVelo;
                    if (molePos.Y > grassPos.Y)
                    {
                        moleState = MoleState.IsDown;
                        molePos.Y = grassPos.Y;
                    }
                    break;

                case MoleState.IsDown:
                    if (rnd.Next(1, 100) == 1)
                    {
                        moleState = MoleState.MovingUp;
                    }
                    break;

                case MoleState.IsHit: //När mole blir klickad ska det direkt bli IsHit. Inuti IsHit byts bilden till bonked & går ner. 
                    {
                        userScore++;
                        molePos += moleVelo *2;
                        if (molePos.Y > grassPos.Y)
                        {
                            moleState = MoleState.MovingDown;
                            molePos.Y = grassPos.Y;
                        }

                    }
                    break;
            }


           
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(moleStateTex[(int)moleState],
                molePos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.5f);

            spriteBatch.Draw(holeTex,
                holePos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                1f);

            spriteBatch.Draw(grassTex,
                grassPos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0f);

            //spriteBatch.DrawRectangle(moleHitBox, //Tillåter mig att se rektangeln
            //    Color.Red, 
            //    2f, 
            //    0);

        }
    }
}
