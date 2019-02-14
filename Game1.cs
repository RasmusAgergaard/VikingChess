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
        private int pieceMoveSpeed = 1;
        private bool isPiecesMoving = true;
        private int turn = 1;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Game
        private enum gameState {gameStart, whiteTurn, whiteMoveing, whiteFighting, blackTurn, blackMoveing, blackFighting, whiteWin, blackWin };
        private gameState currentGameState;

        //Sprites
        Texture2D spritePieceBlack;
        Texture2D spritePieceBlackKing;
        Texture2D spritePieceWhite;
        Texture2D spriteBoard;
        Texture2D spriteSelectedRing;
        Texture2D spriteLegalMove;
        Texture2D spriteRefuge;
        Texture2D spriteKillSplash;

        //Kill splash
        private float KillSplashAlpha = 0;
        private int KillSplashX = -100;
        private int KillSplashY = -100;

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
        private int selectedPieceColumn;
        private int selectedPieceRow;

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
        private Piece pieceBlackKing;
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
            currentGameState = gameState.gameStart;
            //lastTurn = gameState.blackTurn;

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
            pieceBlackKing = new Piece(2, 2); //KING
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
            board[5, 5] = pieceBlackKing;
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
            //board[5, 5] aka the middle refuge is added another place - Good luck finding it!

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
            spriteKillSplash = Content.Load<Texture2D>("kill_splash");

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

            //Move the drawn positions, if there is any to move
            isPiecesMoving = MoveDraw();

            //STATE - GameStart
            if (currentGameState == gameState.gameStart)
            {
                if (isPiecesMoving == false)
                {
                    currentGameState = gameState.whiteTurn;
                }
            }

            //STATE - White turn
            if (currentGameState == gameState.whiteTurn)
            {
                AiRandomAllan(AiTeam: 1);
                //AiAdamTheKiller();
            }

            //STATE - White moving
            if (currentGameState == gameState.whiteMoveing)
            {
                if (isPiecesMoving == false)
                {
                    currentGameState = gameState.whiteFighting;
                }
            }

            //STATE - White fighting
            if (currentGameState == gameState.whiteFighting)
            {
                KillCheck(2, 1);
                currentGameState = gameState.blackTurn;

                //Change turn
                turn = turn + 1;
            }

            //STATE - Black turn
            if (currentGameState == gameState.blackTurn)
            {
                SelectAndMove(mouseState, mousePoint);
            }

            //STATE - Black moving
            if (currentGameState == gameState.blackMoveing)
            {
                if (isPiecesMoving == false)
                {
                    currentGameState = gameState.blackFighting;
                }
            }

            //STATE - Black fighting
            if (currentGameState == gameState.blackFighting)
            {
                KillCheck(1, 2);
                currentGameState = gameState.whiteTurn;

                //Change turn
                turn = turn + 1;
            }

            //Check win or loose conditions
            WinCoditionCheck();

            //Base update
            base.Update(gameTime);
        }


        /*********************************** Draw ***********************************/
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Clear
            GraphicsDevice.Clear(Color.White);

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

            //Draw kill splash
            DrawKillSplash();

            //Draw text
            //DrawText();
            spriteBatch.DrawString(font, "Game state: " + currentGameState.ToString(), new Vector2(30, 480), Color.Black);
            spriteBatch.DrawString(font, "Turn: " + turn.ToString(), new Vector2(30, 510), Color.Black);

            //Base draw
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

        private void SelectAndMove(MouseState mouseState, Point mousePoint)
        {
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
                                    board[column, row].movedInTurn = turn;

                                    isPiecesMoving = true;
                                    currentGameState = gameState.blackMoveing;
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
                                                FindLegalMoves();
                                            }
                                        }
                                        if (currentGameState == gameState.blackTurn)
                                        {
                                            if (board[column, row].myTeam == 2)
                                            {
                                                selectedPiece = board[column, row];
                                                selectedPieceRow = row;
                                                selectedPieceColumn = column;
                                                FindLegalMoves();
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
        }

        //AI - Random Allan
        private void AiRandomAllan(int AiTeam)
        {
            //Find and select a random piece
            FindAndSelectRandomPiece(1);

            //Move selected piece to a random, but legal place
            //RandomMoveSelectedPiece();
           
            //Change turn
            //ChangeTurn();

            //Deselect piece
            //DeselectPiece();
        }

        //AI - Random Allan
        private void FindAndSelectRandomPiece(int AiTeam)
        {
            //Find all the pieces, and add them to the list
            List<int> piecesColumn = new List<int>();
            List<int> piecesRow = new List<int>();
            Random random = new Random();

            int numberOfPiecesThatCanMove = 0;

            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Select the piece
                    selectedPiece = board[column, row];
                    selectedPieceColumn = column;
                    selectedPieceRow = row;

                    //Find the legal moves
                    FindLegalMoves();

                    //Run though legal moves
                    bool isThereAlegalMove = false;

                    //Row
                    for (int legalRow = 0; legalRow < boardRows; legalRow++)
                    {
                        //Columns
                        for (int legalColumn = 0; legalColumn < boardColumns; legalColumn++)
                        {
                            if (legalMoves[legalColumn, legalRow] != null)
                            {
                                isThereAlegalMove = true;
                            }
                        }
                    }

                    //If the selected piece can move
                    if (isThereAlegalMove == true)
                    {
                        //If it is not null, and is the right team - Add to list
                        if (board[column, row] != null && board[column, row].myTeam == AiTeam)
                        {
                            //Add to list
                            piecesColumn.Add(column);
                            piecesRow.Add(row);

                            //Add to the count
                            numberOfPiecesThatCanMove = numberOfPiecesThatCanMove + 1;
                        }
                    }
                }
            }

            //If there is no pieces that can move, the other team wins
            if (numberOfPiecesThatCanMove == 0)
            {
                if (AiTeam == 1)
                {
                    currentGameState = gameState.blackWin;
                }
                if (AiTeam == 2)
                {
                    currentGameState = gameState.whiteWin;
                }
            }
            //If there is a piece that can move, pick one and select it
            else
            {
                //Pick a random place in list, and extract the column and row from it
                int randomPlaceInList = random.Next(piecesColumn.Count);
                int choosenColumn = piecesColumn[randomPlaceInList];
                int choosenRow = piecesRow[randomPlaceInList];

                //Select the choosen piece
                selectedPiece = board[choosenColumn, choosenRow];
                selectedPieceColumn = choosenColumn;
                selectedPieceRow = choosenRow;

                //Move the selected piece
                RandomMoveSelectedPiece();

                //And deselect the piece
                DeselectPiece();
            }
        }

        //AI - Random Allan
        private void RandomMoveSelectedPiece()
        {
            FindLegalMoves();

            //Find all the legal moves, and add them to the list
            List<int> legalColumn = new List<int>();
            List<int> legalRow = new List<int>();
            Random random = new Random();

            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //If it is not null in legal moves, add to list
                    if (legalMoves[column, row] != null)
                    {
                        legalColumn.Add(column);
                        legalRow.Add(row);
                    }
                }
            }

            //Pick a random place in list, and extract the column and row from it
            int randomPlaceInList = random.Next(legalColumn.Count);
            int choosenColumn = legalColumn[randomPlaceInList];
            int choosenRow = legalRow[randomPlaceInList];

            //Move the choosen piece
            board[choosenColumn, choosenRow] = selectedPiece;
            board[selectedPieceColumn, selectedPieceRow] = null;
            board[choosenColumn, choosenRow].movedInTurn = turn;

            //Change turn
            isPiecesMoving = true;
            currentGameState = gameState.whiteMoveing;
        }

        //AI - Adam the Killer
        private void AiAdamTheKiller()
        {
            FindPiecesAndLegalMoves(AiTeam: 1);
        }

        //AI - Adam the Killer
        private void FindPiecesAndLegalMoves(int AiTeam)
        {
            //Find all the pieces, and add them to the list
            List<int> piecesThatHaveLegalMovesColumn = new List<int>();
            List<int> piecesThatHaveLegalMovesRow = new List<int>();
            Random random = new Random();

            int numberOfPiecesThatCanMove = 0;

            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Select the piece
                    selectedPiece = board[column, row];
                    selectedPieceColumn = column;
                    selectedPieceRow = row;

                    //Find the legal moves
                    FindLegalMoves();

                    //Run though legal moves
                    bool isThereAlegalMove = false;

                    //Row
                    for (int legalRow = 0; legalRow < boardRows; legalRow++)
                    {
                        //Columns
                        for (int legalColumn = 0; legalColumn < boardColumns; legalColumn++)
                        {
                            if (legalMoves[legalColumn, legalRow] != null)
                            {
                                isThereAlegalMove = true;
                            }
                        }
                    }

                    //If the selected piece can move
                    if (isThereAlegalMove == true)
                    {
                        //If it is not null, and is the right team - Add to list
                        if (board[column, row] != null && board[column, row].myTeam == AiTeam)
                        {
                            //Add to list
                            piecesThatHaveLegalMovesColumn.Add(column);
                            piecesThatHaveLegalMovesRow.Add(row);

                            //Add to the count
                            numberOfPiecesThatCanMove = numberOfPiecesThatCanMove + 1;
                        }
                    }
                }
            }

            //If there is no pieces that can move, the other team wins
            if (numberOfPiecesThatCanMove == 0)
            {
                if (AiTeam == 1)
                {
                    currentGameState = gameState.blackWin;
                }
                if (AiTeam == 2)
                {
                    currentGameState = gameState.whiteWin;
                }
            }
            //If there is a piece that can move, pick one and select it
            else
            {
                //Pick a random place in list, and extract the column and row from it
                int randomPlaceInList = random.Next(piecesThatHaveLegalMovesColumn.Count);
                int choosenColumn = piecesThatHaveLegalMovesColumn[randomPlaceInList];
                int choosenRow = piecesThatHaveLegalMovesRow[randomPlaceInList];

                //Select the choosen piece
                selectedPiece = board[choosenColumn, choosenRow];
                selectedPieceColumn = choosenColumn;
                selectedPieceRow = choosenRow;

                //Move the selected piece
                RandomMoveSelectedPiece();

                //And deselect the piece
                DeselectPiece();
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

        private void KillCheck(int oppositeTeam, int myTeam)
        {
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    if (board[column, row] != null)
                    {
                        //If the piece is from the opposite team
                        if (board[column, row].myTeam == oppositeTeam)
                        {
                            bool kingTakenColumn = false;
                            bool kingTakenRow = false;

                            //Column
                            if (column > 0 && column < boardColumns - 1)
                            {
                                var checkColumn1 = board[column + 1, row];
                                var checkColumn2 = board[column - 1, row];

                                //Null check
                                if (checkColumn1 != null && checkColumn2 != null)
                                {
                                    //Is there a piece on both sides
                                    if ((checkColumn1.myTeam == myTeam || checkColumn1.myTeam == 3) && (checkColumn2.myTeam == myTeam || checkColumn2.myTeam == 3))
                                    {
                                        //Is one of them was moved in the same turn
                                        if (checkColumn1.movedInTurn == turn || checkColumn2.movedInTurn == turn)
                                        {
                                            //Is the piece a king, then set varible
                                            if (board[column, row].myType == 2)
                                            {
                                                kingTakenColumn = true;
                                            }
                                            //If not - kill it
                                            else
                                            {
                                                KillPiece(column, row);
                                            }
                                        }
                                    }
                                }
                            }

                            //Row
                            if (row > 0 && row < boardRows - 1)
                            {
                                var checkRow1 = board[column, row + 1];
                                var checkRow2 = board[column, row - 1];

                                //Null check
                                if (checkRow1 != null && checkRow2 != null)
                                {
                                    //Is there a piece on both sides
                                    if ((checkRow1.myTeam == myTeam || checkRow1.myTeam == 3) && (checkRow2.myTeam == myTeam || checkRow2.myTeam == 3))
                                    {
                                        //Is one of them the last moved piece
                                        if (checkRow1.movedInTurn == turn || checkRow2.movedInTurn == turn)
                                        {                                            
                                            //Is the piece a king, then set varible
                                            if (board[column, row].myType == 2)
                                            {
                                                kingTakenRow = true;
                                            }
                                            //If not - kill it
                                            else
                                            {
                                                KillPiece(column, row);
                                            }
                                        }
                                    }
                                }
                            }

                            if (kingTakenColumn == true && kingTakenRow == true)
                            {
                                KillKing();
                            }
                        }
                    }
                }
            }
            
        }

        private void KillPiece(int column, int row)
        {
            KillSplashX = board[column, row].posX;
            KillSplashY = board[column, row].posY;
            board[column, row] = null;
        }

        //NOTE: This will remove all king types from the game...
        private void KillKing()
        {
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    if (board[column, row] != null)
                    {
                        //If there is a king, remove it
                        if (board[column, row].myType == 2)
                        {
                            board[column, row] = null;
                        }
                    }
                }
            }
        }

        private void WinCoditionCheck()
        {
            if (isPiecesMoving == false)
            {
                WhiteWinCheck();
                BlackWinCheck();
            }
        }

        private void WhiteWinCheck()
        {
            bool doesKingExist = false;

            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Check if there is a king on the defenders team
                    if (board[column, row] != null && board[column, row].myTeam == 2 && board[column, row].myType == 2)
                    {
                        doesKingExist = true;
                    }
                }
            }

            //If there is not king, the defenders loose, and the attackers win
            if (doesKingExist == false)
            {
                currentGameState = gameState.whiteWin;
            }
        }

        private void BlackWinCheck()
        {
            bool kingEscaped = false;
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //If it is the defenders king
                    if (board[column, row] != null && board[column, row].myTeam == 2 && board[column, row].myType == 2)
                    {
                        //Check to see if he is next to a refuge
                        //Top left
                        if (board[column, row] == board[0, 1] || board[column, row] == board[1, 0])
                        {
                            kingEscaped = true;
                        }

                        //Top right
                        if (board[column, row] == board[0, 9] || board[column, row] == board[1, 10])
                        {
                            kingEscaped = true;
                        }

                        //Bottom left
                        if (board[column, row] == board[9, 0] || board[column, row] == board[10, 1])
                        {
                            kingEscaped = true;
                        }

                        //Bottom right
                        if (board[column, row] == board[9, 10] || board[column, row] == board[10, 9])
                        {
                            kingEscaped = true;
                        }
                    }

                }
            }

            //This king is next to one of the refuges, so the defenders win
            if (kingEscaped == true)
            {
                currentGameState = gameState.blackWin;
            }
        }

        //Move the drawn position towards the real position
        private bool MoveDraw()
        {
            bool aPiceIsMoving = false;

            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    if (board[row, column] != null)
                    {
                        //X
                        if (board[row, column].drawX != board[row, column].posX)
                        {
                            //Set bool
                            aPiceIsMoving = true;

                            //Move left
                            if (board[row, column].drawX < board[row, column].posX)
                            {
                                board[row, column].drawX = board[row, column].drawX + pieceMoveSpeed;
                            }

                            //Move right
                            if (board[row, column].drawX > board[row, column].posX)
                            {
                                board[row, column].drawX = board[row, column].drawX - pieceMoveSpeed;
                            }
                        }

                        //Y
                        if (board[row, column].drawY != board[row, column].posY)
                        {
                            //Set bool
                            aPiceIsMoving = true;

                            //Move down
                            if (board[row, column].drawY < board[row, column].posY)
                            {
                                board[row, column].drawY = board[row, column].drawY + pieceMoveSpeed;
                            }

                            //Move up
                            if (board[row, column].drawY > board[row, column].posY)
                            {
                                board[row, column].drawY = board[row, column].drawY - pieceMoveSpeed;
                            }
                        }
                    }
                }
            }

            return aPiceIsMoving;
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
                        int posX = 10 + margin1;
                        int posY = 10 + margin2;

                        //Set piece position. Note, this is just to have something to check
                        board[row, column].posX = posX;
                        board[row, column].posY = posY;

                        //White - Attackers
                        if (board[row, column].myTeam == 1)
                        {
                            //Draw at the draw position
                            DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceWhite, Color.White, 1f);
                        }

                        //Black - Defenders
                        if (board[row, column].myTeam == 2)
                        {

                            //Normal
                            if (board[row, column].myType == 1)
                            {
                                //Draw at the draw position
                                DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceBlack, Color.White, 1f);
                            }
                            //King
                            else
                            {
                                //Draw at the draw position
                                DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceBlackKing, Color.White, 1f);
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

        //Draw text
        private void DrawText()
        {
            if (currentGameState == gameState.whiteTurn)
            {
                spriteBatch.DrawString(font, "Attackers turn (White)", new Vector2(30, 480), Color.Black);
            }
            if (currentGameState == gameState.blackTurn)
            {
                spriteBatch.DrawString(font, "Defenders turn (Black)", new Vector2(30, 480), Color.Black);
            }
            if (currentGameState == gameState.whiteWin)
            {
                spriteBatch.DrawString(font, "Attackers Win!", new Vector2(30, 480), Color.Black);
            }
            if (currentGameState == gameState.blackWin)
            {
                spriteBatch.DrawString(font, "Defenders Win!", new Vector2(30, 480), Color.Black);
            }
        }

        private void DrawKillSplash()
        {
            DrawSprite(KillSplashX, KillSplashY, spriteKillSplash, Color.White, 1f);
        }

    }
}
