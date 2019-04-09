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

            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void DrawBoard()
        {
            var squareWidth = 64;
            var squareHeight = 32;
            var spriteSize = 64;
            var drawStartX = 0; //windowWidth / 2 - (spriteSize / 2);
            var drawStartY = 80;

            DrawTiles(spriteSize, squareWidth, squareHeight, drawStartX, drawStartY);

            DrawPieces(squareWidth, squareHeight, drawStartX, drawStartY);


        }

        private void DrawPieces(int squareWidth, int squareHeight, int drawStartX, int drawStartY)
        {
            var pieceWidth = 24;
            var pieceHeight = 44;
            var pieceOffsetX = 20;
            var pieceOffsetY = 6;

            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    if (board.Board[x, y] != null)
                    {
                        var pieceOffset = (squareWidth / 2) + (pieceWidth / 2);
                        var screenX = ((x - y) * squareWidth / 2) + drawStartX + pieceOffsetX;
                        var screenY = ((x + y) * squareHeight / 2) + drawStartY - pieceOffsetY;

                        var drawPieceRect = new Rectangle(screenX, screenY, pieceWidth, pieceHeight);

                        //Defenders
                        if (board.Board[x, y].Team == Piece.teams.defenders)
                        {
                            //Normal
                            if (board.Board[x, y].Type == Piece.types.normalPiece)
                            {
                                spriteBatch.Draw(spritePieceDefenderNormal, drawPieceRect, Color.White);
                            }

                            //King
                            if (board.Board[x, y].Type == Piece.types.kingPiece)
                            {
                                spriteBatch.Draw(spritePieceDefenderKing, drawPieceRect, Color.White);
                            }
                        }

                        //Attackers
                        if (board.Board[x, y].Team == Piece.teams.attackers)
                        {
                            //Normal
                            if (board.Board[x, y].Type == Piece.types.normalPiece)
                            {
                                spriteBatch.Draw(spritePieceAttackerNormal, drawPieceRect, Color.White);
                            }
                        }
                    }
                }
            }
        }

        private void DrawTiles(int spriteSize, int squareWidth, int squareHeight, int drawStartX, int drawStartY)
        {
            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    var drawX = ((x - y) * squareWidth / 2) + drawStartX;
                    var drawY = ((x + y) * squareHeight / 2) + drawStartY;

                    //Grass tile
                    var drawTileRect = new Rectangle(drawX, drawY, spriteSize, spriteSize);
                    spriteBatch.Draw(spriteTileGrass, drawTileRect, Color.White);

                    //Refuges
                    if (board.Board[x, y] != null && board.Board[x, y].Team == Piece.teams.refuge)
                    {
                        spriteBatch.Draw(spriteRefuge, drawTileRect, Color.White);
                    }

                    //Legal moves
                    if (gameplayHandler.SelectedPiece != null)
                    {
                        if (board.LegalMoves[x, y] != null)
                        {
                            spriteBatch.Draw(spriteLegalMove, drawTileRect, Color.White);
                        }
                    }
                }
            }
        }
    }
}
