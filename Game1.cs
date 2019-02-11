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
        private gameState lastTurn;

        //Sprites
        Texture2D spritePieceBlack;
        Texture2D spritePieceBlackKing;
        Texture2D spritePieceWhite;
        Texture2D spriteBoard;
        Texture2D spriteSelectedRing;
        Texture2D spriteLegalMove;
        Texture2D spriteRefuge;

        //Fonts
        private SpriteFont font;

        //Board
        private int boardRows = 11;
        private int boardColumns = 11;
        private int boardX = 30;
        private int boardY = 30;
        private int boardSquareSize = 40;
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
        private Piece pieceWhite7;
        private Piece pieceWhite8;
        private Piece pieceWhite9;
        private Piece pieceWhite10;
        private Piece pieceWhite11;
        private Piece pieceWhite12;
        private Piece pieceWhite13;
        private Piece pieceWhite14;
        private Piece pieceWhite15;
        private Piece pieceWhite16;
        private Piece pieceWhite17;
        private Piece pieceWhite18;
        private Piece pieceWhite19;
        private Piece pieceWhite20;
        private Piece pieceWhite21;
        private Piece pieceWhite22;
        private Piece pieceWhite23;
        private Piece pieceWhite24;

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

        //Refuges
        private Piece refuge;

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //System
            this.IsMouseVisible = true;

            //Game
            currentGameState = gameState.whiteTurn;
            lastTurn = gameState.blackTurn;

            //Board
            board = new Piece[boardRows, boardColumns];
            legalMoves = new Piece[boardRows, boardColumns];

            //Selected piece
            selectedPiece = null;

            //Pices - White
            pieceWhite1 = new Piece(1, 1);
            pieceWhite2 = new Piece(1, 1);
            pieceWhite3 = new Piece(1, 1);
            pieceWhite4 = new Piece(1, 1);
            pieceWhite5 = new Piece(1, 1);
            pieceWhite6 = new Piece(1, 1);
            pieceWhite7 = new Piece(1, 1);
            pieceWhite8 = new Piece(1, 1);
            pieceWhite9 = new Piece(1, 1);
            pieceWhite10 = new Piece(1, 1);
            pieceWhite11 = new Piece(1, 1);
            pieceWhite12 = new Piece(1, 1);
            pieceWhite13 = new Piece(1, 1);
            pieceWhite14 = new Piece(1, 1);
            pieceWhite15 = new Piece(1, 1);
            pieceWhite16 = new Piece(1, 1);
            pieceWhite17 = new Piece(1, 1);
            pieceWhite18 = new Piece(1, 1);
            pieceWhite19 = new Piece(1, 1);
            pieceWhite20 = new Piece(1, 1);
            pieceWhite21 = new Piece(1, 1);
            pieceWhite22 = new Piece(1, 1);
            pieceWhite23 = new Piece(1, 1);
            pieceWhite24 = new Piece(1, 1);

            //Piecs - Black
            pieceBlack1 = new Piece(2, 1);
            pieceBlack2 = new Piece(2, 1);
            pieceBlack3 = new Piece(2, 1);
            pieceBlack4 = new Piece(2, 1);
            pieceBlack5 = new Piece(2, 1);
            pieceBlack6 = new Piece(2, 1);
            pieceBlack7 = new Piece(2, 2);
            pieceBlack8 = new Piece(2, 1);
            pieceBlack9 = new Piece(2, 1);
            pieceBlack10 = new Piece(2, 1);
            pieceBlack11 = new Piece(2, 1);
            pieceBlack12 = new Piece(2, 1);
            pieceBlack13 = new Piece(2, 1);

            //Refuge
            refuge = new Piece(3, 1);

            //Add pieces to board - White ()
            board[0, 3] = pieceWhite1;
            board[0, 4] = pieceWhite2;
            board[0, 5] = pieceWhite3;
            board[0, 6] = pieceWhite4;
            board[0, 7] = pieceWhite5;
            board[1, 5] = pieceWhite6;
            board[3, 0] = pieceWhite7;
            board[4, 0] = pieceWhite8;
            board[5, 0] = pieceWhite9;
            board[5, 1] = pieceWhite10;
            board[6, 0] = pieceWhite11;
            board[7, 0] = pieceWhite12;
            board[3, 10] = pieceWhite13;
            board[4, 10] = pieceWhite14;
            board[5, 10] = pieceWhite15;
            board[5, 9] = pieceWhite16;
            board[6, 10] = pieceWhite17;
            board[7, 10] = pieceWhite18;
            board[9, 5] = pieceWhite19;
            board[10, 3] = pieceWhite20;
            board[10, 4] = pieceWhite21;
            board[10, 5] = pieceWhite22;
            board[10, 6] = pieceWhite23;
            board[10, 7] = pieceWhite24;

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

            //Add Refuges to board
            board[0, 0] = refuge;
            board[0, 10] = refuge;
            board[10, 0] = refuge;
            board[10, 10] = refuge;
            //board[5, 5] = refuge;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Sprites
            spritePieceBlack = Content.Load<Texture2D>("piece_black");
            spritePieceBlackKing = Content.Load<Texture2D>("piece_black_king");
            spritePieceWhite = Content.Load<Texture2D>("piece_white");
            spriteBoard = Content.Load<Texture2D>("board");
            spriteSelectedRing = Content.Load<Texture2D>("selected_ring");
            spriteLegalMove = Content.Load<Texture2D>("legal_move");
            spriteRefuge = Content.Load<Texture2D>("refuge");

            //Fonts
            font = Content.Load<SpriteFont>("turn");
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

            //Place refuge when king is moved
            PlaceRefuge();

            //Find legal moves
            FindLegalMoves();

            //Select and move pieces
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Collision check
                    if (PointCollisionWithBox(mousePoint.X, mousePoint.Y, boardX + (boardSquareSize * row), boardY + (boardSquareSize * column), boardSquareSize, boardSquareSize))
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
                                if (selectedPiece == board[column, row])
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
            GraphicsDevice.Clear(Color.White);

            //Update mouse
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            //Draw board
            DrawSprite(boardX, boardY, spriteBoard, Color.White, 1);

            //Selected piece
            if (selectedPiece != null)
            {
                //Draw selected ring
                DrawSprite(30 + (selectedPieceRow * boardSquareSize), 30 + (selectedPieceColumn * boardSquareSize), spriteSelectedRing, Color.White, 1f);

                //Draw legal moves
                DrawLegalMoves(boardColumns, boardRows);
            }

            //Draw pieces
            DrawPieces(boardRows, boardColumns);

            //Draw text
            spriteBatch.DrawString(font, "GameState: " + currentGameState.ToString(), new Vector2(30, 480), Color.Black);

            base.Draw(gameTime);

            spriteBatch.End();
        }


        /*********************************** Methods ***********************************/

        /********** Update **********/

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
            //Save last turn
            lastTurn = currentGameState;

            //Moveing fase
            currentGameState = gameState.moving;

            //Fighting fase
            currentGameState = gameState.fighting;
            KillCheck(gameState.whiteTurn, 2, 1);
            KillCheck(gameState.blackTurn, 1, 2);

            //Start new turn
            if (lastTurn == gameState.whiteTurn)
            {
                currentGameState = gameState.blackTurn;
            }
            else
            {
                currentGameState = gameState.whiteTurn;
            }
        }

        private void PlaceRefuge()
        {
            if (board[5, 5] == null)
            {
                board[5, 5] = refuge;
            }
        }

        private void FindLegalMoves()
        {
            if (selectedPiece != null)
            {
                //Clear array
                legalMoves = new Piece[boardRows, boardColumns];

                //Add rows
                for (int i = 0; i < boardRows; i++)
                {
                    legalMoves[selectedPieceColumn, i] = pieceBlack1;
                }

                //Add column
                for (int i = 0; i < boardColumns; i++)
                {
                    legalMoves[i, selectedPieceRow] = pieceBlack1;
                }

                //Remove self move
                legalMoves[selectedPieceColumn, selectedPieceRow] = null;

                //Remove refuge squares
                legalMoves[0, 0] = null;
                legalMoves[0, 10] = null;
                legalMoves[10, 0] = null;
                legalMoves[10, 10] = null;
                legalMoves[5, 5] = null;

                //Remove pieces - Rows
                for (int row = 0; row < boardRows; row++)
                {
                    //Column
                    for (int column = 0; column < boardColumns; column++)
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
                                for (int i = column; i < boardColumns; i++)
                                {
                                    legalMoves[i, row] = null;
                                }
                            }

                            //If the found piece is RIGHT of the selected piece
                            if (row > selectedPieceRow)
                            {
                                for (int i = row; i < boardRows; i++)
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

        private void KillCheck(gameState lastTurnToCheck, int oppositeTeam, int team)
        {
            if (lastTurn == lastTurnToCheck)
            {
                //Loop though the pieces
                for (int row = 0; row < boardRows; row++)
                {
                    //Columns
                    for (int column = 0; column < boardColumns; column++)
                    {
                        if (board[column, row] != null)
                        {
                            //If the piece is black
                            if (board[column, row].myTeam == oppositeTeam)
                            {
                                //Column
                                if (column > 0 && column < boardColumns - 1)
                                {
                                    var checkColumn1 = board[column + 1, row];
                                    var checkColumn2 = board[column - 1, row];

                                    if (checkColumn1 != null && checkColumn2 != null)
                                    {
                                        if ((checkColumn1.myTeam == team || checkColumn1.myTeam == 3) && (checkColumn2.myTeam == team || checkColumn2.myTeam == 3))
                                        {
                                            KillPiece(column, row);
                                        }
                                    }
                                }

                                //Row
                                if (row > 0 && row < boardRows - 1)
                                {
                                    var checkRow1 = board[column, row + 1];
                                    var checkRow2 = board[column, row - 1];

                                    if (checkRow1 != null && checkRow2 != null)
                                    {
                                        if ((checkRow1.myTeam == team || checkRow1.myTeam == 3) && (checkRow2.myTeam == team || checkRow2.myTeam == 3))
                                        {
                                            KillPiece(column, row);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void KillPiece(int column, int row)
        {
            board[column, row] = null;
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
                        //White - Attackers
                        if (board[row, column].myTeam == 1)
                        {
                            DrawSprite(10 + margin1, 10 + margin2, spritePieceWhite, Color.White, 1f);
                        }

                        //Black - Defenders
                        if (board[row, column].myTeam == 2)
                        {
                            //Normal
                            if (board[row, column].myType == 1)
                            {
                                DrawSprite(10 + margin1, 10 + margin2, spritePieceBlack, Color.White, 1f);
                            }
                            //King
                            else
                            {
                                DrawSprite(10 + margin1, 10 + margin2, spritePieceBlackKing, Color.White, 1f);
                            }
                        }

                        //Refuges
                        if (board[row, column].myTeam == 3)
                        {
                            DrawSprite(10 + margin1, 10 + margin2, spriteRefuge, Color.White, 1f);
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
                        DrawSprite(30 + (column * boardSquareSize), 30 + (row * boardSquareSize), spriteLegalMove, Color.White, 1f);
                    }

                    margin1 = margin1 + 40;
                }

                margin2 = margin2 + 40;
            }
        }

    }
}
