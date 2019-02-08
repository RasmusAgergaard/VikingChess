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
        private MouseState oldState;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Game
        private enum gameState {whiteTurn, blackTurn, moving, fighting };
        private gameState currentGameState;

        //Sprites
        Texture2D spritePieceBlack;
        Texture2D spritePieceWhite;
        Texture2D spriteBoard;
        Texture2D spriteSelectedRing;
        Texture2D spriteLegalMove;

        //Board
        private int rows = 11;
        private int columns = 11;
        private int boardX = 30;
        private int boardY = 30;
        private int squareSize = 40;
        private Piece[,] board;
        private Piece[,] legalMoves;

        //Selected piece
        private Piece selectedPiece;
        private int selectedPieceRow;
        private int selectedPieceColumn;

        //Piece whites
        private Piece pieceWhite1;
        private Piece pieceWhite2;
        private Piece pieceWhite3;
        private Piece pieceWhite4;
        private Piece pieceWhite5;
        private Piece pieceWhite6;

        //Pieces blacks
        private Piece pieceBlack1;
        private Piece pieceBlack2;
        private Piece pieceBlack3;
        private Piece pieceBlack4;
        private Piece pieceBlack5;
        private Piece pieceBlack6;
        private Piece pieceBlack7;
        private Piece pieceBlack8;
        private Piece pieceBlack9;
        private Piece pieceBlack10;
        private Piece pieceBlack11;
        private Piece pieceBlack12;
        private Piece pieceBlack13;

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

            //Game
            currentGameState = gameState.whiteTurn;

            //Board
            board = new Piece[rows, columns];
            legalMoves = new Piece[rows, columns];

            //Selected piece
            selectedPiece = null;

            //Pices - White
            pieceWhite1 = new Piece(1);
            pieceWhite2 = new Piece(1);
            pieceWhite3 = new Piece(1);
            pieceWhite4 = new Piece(1);
            pieceWhite5 = new Piece(1);
            pieceWhite6 = new Piece(1);

            //Piecs - Black
            pieceBlack1 = new Piece(2);
            pieceBlack2 = new Piece(2);
            pieceBlack3 = new Piece(2);
            pieceBlack4 = new Piece(2);
            pieceBlack5 = new Piece(2);
            pieceBlack6 = new Piece(2);
            pieceBlack7 = new Piece(2);
            pieceBlack8 = new Piece(2);
            pieceBlack9 = new Piece(2);
            pieceBlack10 = new Piece(2);
            pieceBlack11 = new Piece(2);
            pieceBlack12 = new Piece(2);
            pieceBlack13 = new Piece(2);


            //Add pieces to board - White
            board[3, 0] = pieceWhite1;
            board[4, 0] = pieceWhite2;
            board[5, 0] = pieceWhite3;
            board[5, 1] = pieceWhite4;
            board[6, 0] = pieceWhite5;
            board[7, 0] = pieceWhite6;

            //Add pieces to board - Black
            board[3, 5] = pieceBlack1;
            board[4, 4] = pieceBlack2;
            board[4, 5] = pieceBlack3;
            board[4, 6] = pieceBlack4;
            board[5, 3] = pieceBlack5;
            board[5, 4] = pieceBlack6;
            board[5, 5] = pieceBlack7;
            board[5, 6] = pieceBlack8;
            board[5, 7] = pieceBlack9;
            board[6, 4] = pieceBlack10;
            board[6, 5] = pieceBlack11;
            board[6, 6] = pieceBlack12;
            board[7, 5] = pieceBlack13;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spritePieceBlack = Content.Load<Texture2D>("piece_black");
            spritePieceWhite = Content.Load<Texture2D>("piece_white");
            spriteBoard = Content.Load<Texture2D>("board");
            spriteSelectedRing = Content.Load<Texture2D>("selected_ring");
            spriteLegalMove = Content.Load<Texture2D>("legal_move");
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

            //Find legal moves
            FindLegalMoves();

            //Select and move pieces
            for (int row = 0; row < rows; row++)
            {
                //Columns
                for (int column = 0; column < columns; column++)
                {
                    //Collision check
                    if (PointCollisionWithBox(mousePoint.X, mousePoint.Y, boardX + (squareSize * row), boardY + (squareSize * column), squareSize, squareSize))
                    {
                        //Mouse click
                        if (mouseState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                        {
                            //Empty square
                            if (board[column, row] == null && legalMoves[column, row] != null)
                            {
                                //If a piece is selected
                                if (selectedPiece != null)
                                {
                                    //MovePiece
                                    board[column, row] = selectedPiece;
                                    board[selectedPieceColumn, selectedPieceRow] = null;

                                    ChangeTurn();
                                    DeselectPiece();
                                }
                            }
                            //Not empty square
                            else
                            {
                                //Deselect piece
                                if (selectedPiece == board[column,row])
                                {
                                    DeselectPiece();
                                }
                                //Select piece
                                else
                                {
                                    if (board[column, row] != null)
                                    {
                                        if (currentGameState == gameState.whiteTurn)
                                        {
                                            if (board[column, row].myTeam == 1)
                                            {
                                                selectedPiece = board[column, row];
                                                selectedPieceRow = row;
                                                selectedPieceColumn = column;
                                            }
                                        }
                                        else
                                        {
                                            if (board[column, row].myTeam == 2)
                                            {
                                                selectedPiece = board[column, row];
                                                selectedPieceRow = row;
                                                selectedPieceColumn = column;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Reassigns the old state so that it is ready for next time
                        oldState = mouseState;
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

            //Selected piece
            if (selectedPiece != null)
            {
                //Draw selected ring
                DrawSprite(30 + (selectedPieceRow * squareSize), 30 + (selectedPieceColumn * squareSize), spriteSelectedRing, Color.White, 1f);

                //Draw legal moves
                DrawLegalMoves(columns, rows);
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

        private void DeselectPiece()
        {
            selectedPiece = null;
        }

        private void MovePiece()
        {
            
        }

        private void ChangeTurn()
        {
            if (currentGameState == gameState.whiteTurn)
            {
                currentGameState = gameState.blackTurn;
            }
            else
            {
                currentGameState = gameState.whiteTurn;
            }
        }

        private void FindLegalMoves()
        {
            if (selectedPiece != null)
            {
                //Clear array
                legalMoves = new Piece[rows, columns];

                //Add rows
                for (int i = 0; i < rows; i++)
                {
                    legalMoves[selectedPieceColumn, i] = pieceBlack1;
                }

                //Add column
                for (int i = 0; i < columns; i++)
                {
                    legalMoves[i, selectedPieceRow] = pieceBlack1;
                }

                //Remove middle move
                legalMoves[selectedPieceColumn, selectedPieceRow] = null;

                //Remove pieces
                //Rows
                for (int row = 0; row < rows; row++)
                {
                    //Column
                    for (int column = 0; column < columns; column++)
                    {
                        //A piece is found
                        if (board[column,row] != null)
                        {
                            //If the found piece is ABOVE the selected piece
                            if (column < selectedPieceColumn)
                            {
                                for (int i = column; i >= 0; i--)
                                {
                                    legalMoves[column - i, row] = null;
                                }
                            }

                            //If the found piece is BELOW the selected piece
                            if (column > selectedPieceColumn)
                            {
                                for (int i = column; i < columns; i++)
                                {
                                    legalMoves[i, row] = null;
                                }
                            }

                            //If the found piece is RIGHT of the selected piece
                            if (row > selectedPieceRow)
                            {
                                for (int i = row; i < rows; i++)
                                {
                                    legalMoves[column, i] = null;
                                }
                            }

                            //If the found piece is LEFT of the selected piece
                            if (row < selectedPieceRow)
                            {
                                for (int i = row; i >= 0; i--)
                                {
                                    legalMoves[column, row - i] = null;
                                }
                            }
                        }
                    }
                }
            }
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
                        //White
                        if (board[row, column].myTeam == 1)
                        {
                            DrawSprite(10 + margin1, 10 + margin2, spritePieceWhite, Color.White, 1f);
                        }
                        //Black
                        else
                        {
                            DrawSprite(10 + margin1, 10 + margin2, spritePieceBlack, Color.White, 1f);
                        }
                        
                    }

                    margin1 = margin1 + 40;
                }

                margin2 = margin2 + 40;
            }
        }

        //Draw legal moves
        private void DrawLegalMoves(int numberOfColumns, int numberOfRows)
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
                    if (legalMoves[row, column] != null)
                    {
                        DrawSprite(30 + (column * squareSize), 30 + (row * squareSize), spriteLegalMove, Color.White, 1f);
                    }

                    margin1 = margin1 + 40;
                }

                margin2 = margin2 + 40;
            }
        }

    }
}
