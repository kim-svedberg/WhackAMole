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
        Vector2 pos;
        Vector2 startPos;

        public Mole(Texture2D moleTex, Texture2D holeTex, Texture2D grassTex, int posX, int posY)
        {
            this.moleTex = moleTex;
            this.holeTex = holeTex;
            this.grassTex = grassTex;
            pos = new Vector2(posX, posY);
            startPos = new Vector2(posX, posY);

        }

        public void Update()
        {
            while(true)
            {
                 
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(moleTex, 
                pos, 
                null, 
                Color.White, 
                0, 
                Vector2.Zero, 
                1.0f, 
                SpriteEffects.None,
                0.5f);

            spriteBatch.Draw(holeTex, 
                startPos, 
                null, 
                Color.White, 
                0, 
                Vector2.Zero, 
                1.0f, 
                SpriteEffects.None, 
                1f);

            spriteBatch.Draw(grassTex, 
                startPos, 
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
