using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldCode
{
    public class Game1
    {
        //System
        private int windowHeight = 360;
        private int windowWidth = 640;
        private MouseState oldState;
        private int pieceMoveSpeed = 1;
        private bool isPiecesMoving = true;
        private int turn = 1;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera
        private ResolutionIndependentRenderer _resolutionIndependence;
        private Camera2D _camera;
        private SpriteFont _debugFont;
        private Texture2D _bkg;
        private Vector2 _bkgPos;
        private InputHelper _inputHelper;
        private Vector2 _screenMousePos;
        private Vector2 _mouseDrawPos;
        private float _rotationDiff = 0.02f;
        private float _zoomDiff = 0.02f;
        private Vector2 _instructionsDrawPos;
        private string _instructions = "Use (+/-) to Zoom  And (Shift +/-) to Rotate. (R to reset)";

        //Game
        private enum gameState { gameStart, whiteTurn, whiteMoveing, whiteFighting, blackTurn, blackMoveing, blackFighting, whiteWin, blackWin };
        private gameState currentGameState;

        //AI
        string AiLog = "";

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

        private int selectedPieceColumn;
        private int selectedPieceRow;


        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            _resolutionIndependence = new ResolutionIndependentRenderer(this);
            _inputHelper = new InputHelper();
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //System
            this.IsMouseVisible = true;

            //Game
            currentGameState = gameState.gameStart;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Camera
            _camera = new Camera2D(_resolutionIndependence);
            _camera.Zoom = 0.5f;
            _camera.Position = new Vector2(_resolutionIndependence.VirtualWidth / 2, _resolutionIndependence.VirtualHeight / 2);

            InitializeResolutionIndependence(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            _mouseDrawPos.X = 700;
            _mouseDrawPos.Y = 40f;

            _instructionsDrawPos.X = 600;
            _instructionsDrawPos.Y = 10f;

            _debugFont = Content.Load<SpriteFont>(@"font");
            _bkg = Content.Load<Texture2D>(@"board");
            _bkgPos = new Vector2(0, 0);


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

        private void InitializeResolutionIndependence(int realScreenWidth, int realScreenHeight)
        {
            _resolutionIndependence.VirtualWidth = 960;
            _resolutionIndependence.VirtualHeight = 540;
            _resolutionIndependence.ScreenWidth = realScreenWidth;
            _resolutionIndependence.ScreenHeight = realScreenHeight;
            _resolutionIndependence.Initialize();

            _camera.RecalculateTransformationMatrices();
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

            //Camera control
            _inputHelper.Update();

            _screenMousePos = _resolutionIndependence.ScaleMouseToScreenCoordinates(_inputHelper.MousePosition);

            if (_inputHelper.IsCurPress(Keys.Add))
            {
                if (_inputHelper.IsCurPress(Keys.LeftShift))
                    _camera.Rotation += _rotationDiff;
                else
                    _camera.Zoom += _zoomDiff;
            }
            else if (_inputHelper.IsCurPress(Keys.Subtract))
            {
                if (_inputHelper.IsCurPress(Keys.LeftShift))
                    _camera.Rotation -= _rotationDiff;
                else
                    _camera.Zoom -= _zoomDiff;
            }
            else if (_inputHelper.IsNewPress(Keys.R))
            {
                _camera.Zoom = 1;
                _camera.Rotation = 0;
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
                //AiRandomAllan(AiTeam: 1);
                AiAdamTheKiller();
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
            //spriteBatch.Begin();

            //Clear
            //GraphicsDevice.Clear(Color.White);



            //Camera
            _resolutionIndependence.BeginDraw();

            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, _camera.GetViewTransformationMatrix());
            spriteBatch.Draw(_bkg, _bkgPos, Color.White);
            spriteBatch.DrawString(_debugFont, string.Format("Translated Mouse Pos: x:{0:0}  y:{1:0}", _screenMousePos.X, _screenMousePos.Y), _mouseDrawPos, Color.Black);
            spriteBatch.DrawString(_debugFont, _instructions, _instructionsDrawPos, Color.Yellow);

            //Draw board
            //DrawSprite(boardX, boardY, spriteBoard, Color.White, new Rectangle(0, 0, 200, 200));
            //spriteBatch.Draw(spriteBoard, Vector2.Zero, new Rectangle(0, 0, 200, 200), Color.White);

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
            spriteBatch.DrawString(font, "AI log: " + AiLog, new Vector2(30, 540), Color.Black);


            //Base draw
            base.Draw(gameTime);

            spriteBatch.End();
        }




        /*********************************** Methods ***********************************/

        /******************** Update ********************/


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

        private void MovePiece(int legalColumn, int legalRow)
        {
            board[legalColumn, legalRow] = selectedPiece;
            board[selectedPieceColumn, selectedPieceRow] = null;
            board[legalColumn, legalRow].movedInTurn = turn;
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
                        if (board[column, row] != null)
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

        private void KillKing()
        {
            //NOTE: This will remove all king types from the game...

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

        private bool MoveDraw()
        {
            //Move the drawn position towards the real position
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

        /******************** AI ********************/

        /********** Allan the Idiot **********/
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

        /********** Adam the Killer **********/
        private void AiAdamTheKiller()
        {
            //Kill normal piece
            if (MoveToKillPosition(myTeam: 1, enemyTeam: 2, enemyType: 1)) { }

            //Move next to king
            else if (MoveNextToPiece(myTeam: 1, enemyTeam: 2, enemyType: 2)) { }

            //Block kings movement towards refuges

            //Block kings movement
            else if (BlockKingMovement(myTeam: 1, enemyTeam: 2, enemyType: 2)) { }

            //Move next to normal piece
            else if (MoveNextToPiece(myTeam: 1, enemyTeam: 2, enemyType: 1)) { }

            //Random move

            //None of the above worked - So change the turn
            isPiecesMoving = true;
            currentGameState = gameState.whiteMoveing;
        }

        private bool MoveNextToPiece(int myTeam, int enemyTeam, int enemyType)
        {
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Make sure there is a piece and check team
                    if (board[column, row] != null && board[column, row].myTeam == myTeam)
                    {
                        //Select the piece
                        selectedPiece = board[column, row];
                        selectedPieceColumn = column;
                        selectedPieceRow = row;

                        //Find the legal moves
                        FindLegalMoves();

                        if (selectedPiece != null)
                        {
                            //Move next to piece
                            for (int legalRow = 0; legalRow < boardRows; legalRow++)
                            {
                                //Columns
                                for (int legalColumn = 0; legalColumn < boardColumns; legalColumn++)
                                {
                                    //If there is a legal move
                                    if (legalMoves[legalColumn, legalRow] != null)
                                    {
                                        if (EnemyIsNextToSquare(legalRow, legalColumn, enemyTeam, enemyType))
                                        {
                                            //Move the choosen piece
                                            MovePiece(legalColumn, legalRow);

                                            //Change turn
                                            isPiecesMoving = true;
                                            currentGameState = gameState.whiteMoveing;
                                            DeselectPiece();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool MoveToKillPosition(int myTeam, int enemyTeam, int enemyType)
        {
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Make sure there is a piece and check team
                    if (board[column, row] != null && board[column, row].myTeam == myTeam)
                    {
                        //Select the piece
                        selectedPiece = board[column, row];
                        selectedPieceColumn = column;
                        selectedPieceRow = row;

                        //Find the legal moves
                        FindLegalMoves();

                        //Run though legal moves
                        if (selectedPiece != null)
                        {
                            //Row
                            for (int legalRow = 0; legalRow < boardRows; legalRow++)
                            {
                                //Columns
                                for (int legalColumn = 0; legalColumn < boardColumns; legalColumn++)
                                {
                                    //If there is a legal move
                                    if (legalMoves[legalColumn, legalRow] != null)
                                    {
                                        if (EnemyCanBeKilledFromSquare(legalRow, legalColumn, myTeam, enemyTeam, enemyType))
                                        {
                                            //Move the choosen piece
                                            MovePiece(legalColumn, legalRow);

                                            //Change turn
                                            isPiecesMoving = true;
                                            currentGameState = gameState.whiteMoveing;
                                            DeselectPiece();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool BlockKingMovement(int myTeam, int enemyTeam, int enemyType)
        {
            Piece[,] kingLegalMoves = new Piece[boardRows, boardColumns];

            //Find and select the king
            FindAndSelectPiece(enemyTeam, enemyType);
            //Find his legal moves
            FindLegalMoves();
            //Save them
            kingLegalMoves = legalMoves;

            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Make sure there is a piece and check team
                    if (board[column, row] != null && board[column, row].myTeam == myTeam)
                    {
                        //Select the piece
                        selectedPiece = board[column, row];
                        selectedPieceColumn = column;
                        selectedPieceRow = row;

                        //Find the legal moves
                        FindLegalMoves();

                        //Row
                        for (int legalRow = 0; legalRow < boardRows; legalRow++)
                        {
                            //Columns
                            for (int legalColumn = 0; legalColumn < boardColumns; legalColumn++)
                            {
                                //If there is a legal move, and the king have a legal move on the same spot - move there
                                if (legalMoves[legalColumn, legalRow] != null && kingLegalMoves[legalColumn, legalRow] != null)
                                {
                                    MovePiece(legalColumn, legalRow);
                                    AiLog = "Block king move";

                                    //Change turn
                                    isPiecesMoving = true;
                                    currentGameState = gameState.whiteMoveing;
                                    DeselectPiece();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void FindAndSelectPiece(int team, int type)
        {
            //Row
            for (int row = 0; row < boardRows; row++)
            {
                //Columns
                for (int column = 0; column < boardColumns; column++)
                {
                    //Not null, and the right team and type
                    if (board[column, row] != null && board[column, row].myTeam == team && board[column, row].myType == type)
                    {
                        //Select the piece
                        selectedPiece = board[column, row];
                        selectedPieceColumn = column;
                        selectedPieceRow = row;
                    }
                }
            }
        }

        private bool EnemyIsNextToSquare(int legalRow, int legalColumn, int enemyTeam, int enemyType)
        {
            if (legalColumn > 0 && legalColumn < boardColumns - 1)
            {
                var checkSquare1 = board[legalColumn - 1, legalRow];
                var checkSquare4 = board[legalColumn + 1, legalRow];

                if (checkSquare1 != null && checkSquare1.myTeam == enemyTeam && checkSquare1.myType == enemyType)
                {
                    AiLog = "Move next to square - Type: " + enemyType.ToString();
                    return true;
                }

                if (checkSquare4 != null && checkSquare4.myTeam == enemyTeam && checkSquare4.myType == enemyType)
                {
                    AiLog = "Move next to square - Type: " + enemyType.ToString();
                    return true;
                }
            }

            if (legalRow > 0 && legalRow < boardRows - 1)
            {
                var checkSquare2 = board[legalColumn, legalRow - 1];
                var checkSquare3 = board[legalColumn, legalRow + 1];

                if (checkSquare2 != null && checkSquare2.myTeam == enemyTeam && checkSquare2.myType == enemyType)
                {
                    AiLog = "Move next to square - Type: " + enemyType.ToString();
                    return true;
                }

                if (checkSquare3 != null && checkSquare3.myTeam == enemyTeam && checkSquare3.myType == enemyType)
                {
                    AiLog = "Move next to square - Type: " + enemyType.ToString();
                    return true;
                }
            }

            return false;
        }

        private bool EnemyCanBeKilledFromSquare(int legalMoveRow, int legalMoveColumn, int myTeam, int enemyTeam, int enemyType)
        {
            if (legalMoveColumn > 1 && legalMoveColumn < boardColumns - 2)
            {
                //Set the squares to check
                var checkSquare1 = board[legalMoveColumn - 2, legalMoveRow];
                var checkSquare2 = board[legalMoveColumn - 1, legalMoveRow];
                var checkSquare7 = board[legalMoveColumn + 1, legalMoveRow];
                var checkSquare8 = board[legalMoveColumn + 2, legalMoveRow];

                //If square right above is an enemy
                if (checkSquare2 != null && checkSquare2.myTeam == enemyTeam && checkSquare2.myType == enemyType)
                {
                    //And the one above that is a friend
                    if (checkSquare1 != null && checkSquare1.myTeam == myTeam)
                    {
                        AiLog = "Enemy can be killed - Column";
                        return true;
                    }
                }

                if (checkSquare7 != null && checkSquare7.myTeam == enemyTeam && checkSquare7.myType == enemyType)
                {
                    if (checkSquare8 != null && checkSquare8.myTeam == myTeam)
                    {
                        AiLog = "Enemy can be killed - Column";
                        return true;
                    }
                }
            }

            if (legalMoveRow > 1 && legalMoveRow < boardRows - 2)
            {
                //Set the squares to check
                var checkSquare3 = board[legalMoveColumn, legalMoveRow - 2];
                var checkSquare4 = board[legalMoveColumn, legalMoveRow - 1];
                var checkSquare5 = board[legalMoveColumn, legalMoveRow + 1];
                var checkSquare6 = board[legalMoveColumn, legalMoveRow + 2];

                //If square next to it, is a enemy
                if (checkSquare4 != null && checkSquare4.myTeam == enemyTeam && checkSquare4.myType == enemyType)
                {
                    //And the one above that is a friend
                    if (checkSquare3 != null && checkSquare3.myTeam == myTeam)
                    {
                        AiLog = "Enemy can be killed - Row";
                        return true;
                    }
                }

                if (checkSquare5 != null && checkSquare5.myTeam == enemyTeam && checkSquare5.myType == enemyType)
                {
                    if (checkSquare6 != null && checkSquare6.myTeam == myTeam)
                    {
                        AiLog = "Enemy can be killed - Row";
                        return true;
                    }
                }
            }


            return false;
        }


        /******************** Draw ********************/

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
            float boardHeight = 838;
            //List<float> squareSizeColumnPercent = new List<float> {0f, 7.16f, 7.40f, 7.76f, 8.11f, 8.47f, 8.95f, 9.31f, 9.90f, 10.50f, 10.86f, 11.58f};
            //List<float> squareSizeRowPercent = new List<float> { 0f, 7.16f, 7.40f, 7.76f, 8.11f, 8.47f, 8.95f, 9.31f, 9.90f, 10.50f, 10.86f, 11.58f };

            //Maybe use this alos
            //squareStartPosX = squareStartPosX + (squareSizeRowPercent[row] * boardHeight / 100);

            List<float> squareTotalSizeColumnPercent = new List<float> { 0f, 7.16f, 14.56f, 22.32f, 30.43f, 38.9f, 47.85f, 57.16f, 67.06f, 77.56f, 88.42f };

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
                        float temp = squareTotalSizeColumnPercent[row] * boardHeight / 100;

                        int posX = 10 + margin1;
                        int posY = (int)temp;

                        //Set piece position. Note, this is just to have something to check
                        board[row, column].posX = posX;
                        board[row, column].posY = posY;

                        //White - Attackers
                        if (board[row, column].myTeam == 1)
                        {
                            //Draw at the draw position
                            DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceWhite, Color.White, 0.8f);
                        }

                        //Black - Defenders
                        if (board[row, column].myTeam == 2)
                        {

                            //Normal
                            if (board[row, column].myType == 1)
                            {
                                //Draw at the draw position
                                DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceBlack, Color.White, 0.8f);
                            }
                            //King
                            else
                            {
                                //Draw at the draw position
                                DrawSprite(board[row, column].drawX, board[row, column].drawY, spritePieceBlackKing, Color.White, 0.9f);
                            }
                        }

                        //Refuges
                        if (board[row, column].myTeam == 3)
                        {
                            DrawSprite(10 + margin1, 10 + margin2, spriteRefuge, Color.White, 1f);
                        }

                    }

                    margin1 = margin1 + 90;
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
}
