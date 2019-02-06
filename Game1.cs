using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingChess
{
    public class Game1 : Game
    {
        //System

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprites
        Texture2D spritePiece;

        Texture2D spriteRect;
        int rectX = 50;
        int rectY = 50;
        int spriteSize = 50;

        //Board
        private int rows = 3;
        private int columns = 3;
        private Piece[,] board;

        //Piece
        private Piece piece1;
        private Piece piece2;
        private Piece piece3;

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //System
            this.IsMouseVisible = true;

            //Board
            board = new Piece[rows, columns];

            //Pices
            piece1 = new Piece();
            piece2 = new Piece();
            piece3 = new Piece();

            //Add pieces to board
            board[0, 0] = piece1;
            board[0, 1] = piece2;
            board[0, 2] = piece3;
            board[1, 0] = piece1;
            board[1, 1] = piece2;
            board[1, 2] = piece3;
            board[2, 0] = piece1;
            board[2, 1] = piece2;
            board[2, 2] = piece3;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spritePiece = Content.Load<Texture2D>("piece");
            spriteRect = Content.Load<Texture2D>("rect");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //Update mouse
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);



            base.Update(gameTime);
        }

        /********** Update Methods ***********/
        /*
        private bool IsPointOverSprite(float x, float y, Sprite texture2D)
        {
            return(texture2D.get)
        }
        */

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Clear
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Update mouse
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);


            //Draw hover test
            if (Hover(mousePoint.X, mousePoint.Y, rectX, rectY, spriteSize, spriteSize))
            {
                DrawSprite(rectX, rectY, spriteRect, Color.Wheat, 1);
            }

            //Draw pieces
            int margin2 = 60;

            //Rows
            for (int row = 0; row < 3; row++)
            {
                int margin1 = 60;
                
                //Column
                for (int column = 0; column < 3; column++)
                {
                    if (board[row, column] != null)
                    {
                        DrawSprite(100 + margin1, 100 + margin2, spritePiece, Color.White, 0.04f);
                    }

                    margin1 = margin1 + 60;
                }

                margin2 = margin2 + 60;
            }
            

            base.Draw(gameTime);

            spriteBatch.End();
        }

        /********** Draw Methods **********/

        //Standard draw
        private void DrawSprite(float xpos, float ypos, Texture2D texture, Color color, float scale)
        {
            Vector2 DrawSprite = new Vector2(xpos, ypos);
            spriteBatch.Draw(texture, DrawSprite, null, color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }

        private bool Hover(float mouseX, float mouseY, float spriteX, float spriteY, int spriteWidth, int spriteHeight)
        {
            if (mouseX >= spriteX && mouseX <= spriteX + spriteWidth && mouseY >= spriteY && mouseY <= spriteY + spriteHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
