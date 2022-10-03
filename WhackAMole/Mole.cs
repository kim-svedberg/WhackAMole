using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace WhackAMole
{
    internal class Mole
    {
        Texture2D moleTex;
        Texture2D holeTex;
        Texture2D grassTex;

        Vector2 molePos;
        Vector2 holePos;
        Vector2 grassPos;
        Vector2 moleVelo;

        int widthSpace;
        int heightSpace;

        Rectangle moleHitBox;

        enum MoleState
        {
            MovingUp, IsUp, MovingDown, IsDown, IsHit //IsDownHit
        }

        MoleState moleState;


        public Mole(Texture2D moleTex, Texture2D holeTex, Texture2D grassTex, int widthSpace, int heightSpace)
        {
            this.moleTex = moleTex;
            this.holeTex = holeTex;
            this.grassTex = grassTex;
            this.widthSpace = widthSpace;
            this.heightSpace = heightSpace;


        }

        public void Load()
        {
            holePos = new Vector2(150 + widthSpace, 300 + heightSpace);
            grassPos = new Vector2(150 + widthSpace, 300 + heightSpace);
            molePos = new Vector2(150 + widthSpace, grassPos.Y);
            moleVelo = new Vector2(0, 2.5f);
            moleState = MoleState.IsDown;
        }

        public void Update(Random rnd)
        {
            switch (moleState)
            {
                case MoleState.MovingUp:
                case MoleState.IsUp:
                    if(klick)
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
                    break;
            }


            int height = ((int)molePos.Y - (int)grassPos.Y);
            moleHitBox = new Rectangle((int)molePos.X, (int)molePos.Y - height + 35, moleTex.Width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(moleTex,
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

            spriteBatch.DrawRectangle(moleHitBox, Color.Red, 2f, 0);
        }
    }
}
