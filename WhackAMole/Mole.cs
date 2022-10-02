using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            holePos = new Vector2(150 + widthSpace, 300+heightSpace);
            molePos = new Vector2(150 + widthSpace, 125 + heightSpace);
            grassPos = new Vector2(150 + widthSpace, 300 + heightSpace);
            moleVelo = new Vector2(0, 2);
        }

        public void Update()
        {
            molePos = molePos + moleVelo;

            if(molePos.Y < grassPos.Y-175 || molePos.Y > grassPos.Y)
            {
                moleVelo.Y *= -1;
            }


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
        }
    }
}
