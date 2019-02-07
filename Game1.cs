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
        Texture2D spriteBoard;
        Texture2D spriteSelectedRing;

        //Board
        private int rows = 11;
        private int columns = 11;
        private int boardX = 30;
        private int boardY = 30;
        private int squareSize = 40;
        private Piece[,] board;

        //Selected piece
        private Piece selectedPiece;
        private int selectedPieceRow;
        private int selectedPieceColumn;

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

            //Selected piece
            selectedPiece = null;

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
            board[3, 3] = piece3;
            board[5, 5] = piece3;
            board[8, 8] = piece3;
            board[9, 10] = piece3;
            board[10, 10] = piece3;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spritePiece = Content.Load<Texture2D>("piece");
            spriteBoard = Content.Load<Texture2D>("board");
            spriteSelectedRing = Content.Load<Texture2D>("selected_ring");
        }

        protected override void UnloadContent()
        {
        }

        /*********************************** Update ***********************************/
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

            //Select and move pieces
            for (int row = 0; row < rows; row++)
            {
                //Columns
                for (int column = 0; column < columns; column++)
                {
                    if (PointCollisionWithBox(mousePoint.X, mousePoint.Y, boardX + (squareSize * row), boardY + (squareSize * column), squareSize, squareSize))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            //Empty square
                            if (board[column, row] == null)
                            {
                                //If a piece is selected
                                if (selectedPiece != null)
                                {
                                    MovePiece();

                                    //Deselect piece
                                    selectedPiece = null;
                                }
                            }
                            //Not empty square - select the piece
                            else
                            {
                                selectedPiece = board[column, row];
                                selectedPieceRow = row;
                                selectedPieceColumn = column;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        /*********************************** Draw ***********************************/
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Clear
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Update mouse
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            //Draw board
            DrawSprite(boardX, boardY, spriteBoard, Color.White, 1);

            //Draw selected ring
            if (selectedPiece != null)
            {
                DrawSprite(30 + (selectedPieceRow * squareSize), 30 + (selectedPieceColumn * squareSize), spriteSelectedRing, Color.White, 1f);
            }

            //Draw pieces
            DrawPieces(rows, columns);

            base.Draw(gameTime);

            spriteBatch.End();
        }


        /*********************************** Methods ***********************************/

        /********** Update **********/

        //Point collision with box
        private bool PointCollisionWithBox(float pointX, float pointY, float boxX, float boxY, int boxWidth, int boxHeight)
        {
            if (pointX > boxX && pointX < boxX + boxWidth && pointY > boxY && pointY < boxY + boxHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MovePiece()
        {
            throw new NotImplementedException();
        }



        /********** Draw **********/

        //Draw sprite
        private void DrawSprite(float xpos, float ypos, Texture2D texture, Color color, float scale)
        {
            Vector2 DrawSprite = new Vector2(xpos, ypos);
            spriteBatch.Draw(texture, DrawSprite, null, color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }

        //Draw pieces
        private void DrawPieces(int numberOfRows, int numberOfColumns)
        {
            //Draw pieces
            int margin2 = 20;

            //Rows
            for (int row = 0; row < numberOfRows; row++)
            {
                int margin1 = 20;

                //Column
                for (int column = 0; column < numberOfColumns; column++)
                {
                    if (board[row, column] != null)
                    {
                        DrawSprite(10 + margin1, 10 + margin2, spritePiece, Color.White, 0.04f);
                    }

                    margin1 = margin1 + 40;
                }

                margin2 = margin2 + 40;
            }
        }

    }
}
