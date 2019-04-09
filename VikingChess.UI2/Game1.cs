using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingChessBL;

namespace VikingChess.UI2
{
    public class Game1 : Game
    {
        //Init
        GameSetup gameSetup = new GameSetup();
        PlayBoard board;
        CollisionHandler collisionHandler;
        GameplayHandler gameplayHandler;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprites
        Texture2D spriteTileGrass;
        Texture2D spritePieceDefenderNormal;
        Texture2D spritePieceDefenderKing;
        Texture2D spritePieceAttackerNormal;
        Texture2D spriteRefuge;
        Texture2D spriteSelectedPiece;
        Texture2D spriteLegalMove;
        Texture2D spriteTestSquare;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 540;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            board = new PlayBoard(11, 11, new Vector2(0));
            board.SetRules(doesAttackersHaveKing: false,
                           doesDefendersHaveKing: true,
                           doesAttackerKingWantsToFlee: false,
                           doesDefenderKingWantsToFlee: true);

            gameplayHandler = new GameplayHandler(board);
            collisionHandler = new CollisionHandler();

            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteTileGrass = Content.Load<Texture2D>(@"Tiles\tile_grass");
            spritePieceDefenderNormal = Content.Load<Texture2D>(@"Pieces\piece_defender_normal");
            spritePieceDefenderKing = Content.Load<Texture2D>(@"Pieces\piece_defender_king");
            spritePieceAttackerNormal = Content.Load<Texture2D>(@"Pieces\piece_attacker_normal");
            spriteRefuge = Content.Load<Texture2D>(@"Refuges\tile_refuge");
            spriteSelectedPiece = Content.Load<Texture2D>(@"Layers\selected_piece");
            spriteLegalMove = Content.Load<Texture2D>(@"Layers\legal_move");
            spriteTestSquare = Content.Load<Texture2D>(@"Test\square");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (board.State)
            {
                case PlayBoard.gameState.gameStart:
                    break;

                case PlayBoard.gameState.attackerTurn:
                    board = gameplayHandler.Update(gameSetup);
                    break;

                case PlayBoard.gameState.attackerMoveing:
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.attackerFighting:
                    board = gameplayHandler.Update(gameSetup);
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.attackerWinCheck:
                    board = gameplayHandler.Update(gameSetup);
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.defenderTurn:
                    board = gameplayHandler.Update(gameSetup);
                    break;

                case PlayBoard.gameState.defenderMoveing:
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.defenderFighting:
                    board = gameplayHandler.Update(gameSetup);
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.defenderWinCheck:
                    board = gameplayHandler.Update(gameSetup);
                    board.ChangeTurn();
                    break;

                case PlayBoard.gameState.attackerWin:
                    break;

                case PlayBoard.gameState.defenderWin:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawBoard();



            var spriteSize = 64;
            var drawStartX = 960 / 2;
            var drawStartY = 120;

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var mouseX = mousePoint.X - drawStartX;
            var mouseY = mousePoint.Y - drawStartY;



            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    var xx = (mouseX / 32 + mouseY / 16) / 2;
                    var yy = (mouseY / 16 - (mouseX / 32)) / 2;

                    var posX = 0;
                    var posY = 0;

                    if (xx >= 0 && xx < 11)
                    {
                        if (yy >= 0 && yy < 11)
                        {
                            posX = (int)board.BoardPositions[xx, yy].X;
                            posY = (int)board.BoardPositions[xx, yy].Y;
                        }
                    }

                    var width = 64;
                    var height = 64;

                    var drawRect = new Rectangle(posX, posY, width, height);
                    spriteBatch.Draw(spriteLegalMove, drawRect, Color.White);

                    //if (collisionHandler.PointColisionWithBox(mouseX, mouseY, posX, posY, width, height))
                    //{
                    //    var drawRect = new Rectangle(posX, posY, width, height);
                    //    spriteBatch.Draw(spriteTestSquare, drawRect, Color.White);
                    //}
                }
            }



            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void DrawBoard()
        {
            DrawTiles();
            DrawPieces();
        }

        private void DrawTiles()
        {
            var spriteSize = 64;

            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {

                    var drawX = (int)board.BoardPositions[x, y].X;
                    var drawY = (int)board.BoardPositions[x, y].Y;
                    var drawRect = new Rectangle(drawX, drawY, spriteSize, spriteSize);

                    //Grass tile
                    spriteBatch.Draw(spriteTileGrass, drawRect, Color.White);

                    //Refuge
                    if (board.Board[x, y] != null && board.Board[x, y].Team == Piece.teams.refuge)
                    {
                        spriteBatch.Draw(spriteRefuge, drawRect, Color.White);
                    }

                    //Legal moves
                    if (gameplayHandler.SelectedPiece != null)
                    {
                        if (board.LegalMoves[x, y] != null)
                        {
                            spriteBatch.Draw(spriteLegalMove, drawRect, Color.White);
                        }
                    }
                }
            }
        }

        private void DrawPieces()
        {
            var pieceWidth = 24;
            var pieceHeight = 44;
            var pieceOffsetX = 20;
            var pieceOffsetY = -6;

            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    if (board.Board[x, y] != null)
                    {
                        var drawX = (int)board.BoardPositions[x, y].X + pieceOffsetX;
                        var drawY = (int)board.BoardPositions[x, y].Y + pieceOffsetY;
                        var drawRect = new Rectangle(drawX, drawY, pieceWidth, pieceHeight);

                        //Defenders
                        if (board.Board[x, y].Team == Piece.teams.defenders)
                        {
                            //Normal
                            if (board.Board[x, y].Type == Piece.types.normalPiece)
                            {
                                spriteBatch.Draw(spritePieceDefenderNormal, drawRect, Color.White);
                            }

                            //King
                            if (board.Board[x, y].Type == Piece.types.kingPiece)
                            {
                                spriteBatch.Draw(spritePieceDefenderKing, drawRect, Color.White);
                            }
                        }

                        //Attackers
                        if (board.Board[x, y].Team == Piece.teams.attackers)
                        {
                            //Normal
                            if (board.Board[x, y].Type == Piece.types.normalPiece)
                            {
                                spriteBatch.Draw(spritePieceAttackerNormal, drawRect, Color.White);
                            }
                        }
                    }
                }
            }
        }

        
    }
}
