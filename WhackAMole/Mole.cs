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
        Vector2 startPos;
        Vector2 pos; 

        public Mole(Texture2D moleTex, Texture2D holeTex, Texture2D grassTex, Vector2 startPos, Vector2 pos)
        {
            this.moleTex = moleTex;
            this.holeTex = holeTex;
            this.grassTex = grassTex;
            this.startPos = startPos;
            this.pos = pos;
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(moleTex, pos, Color.White);
            spriteBatch.Draw(holeTex, startPos, Color.White);
            spriteBatch.Draw(grassTex, startPos, Color.White);
        }
    }
}
