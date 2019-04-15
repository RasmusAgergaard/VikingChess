using System;
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
        int windowWidth = 960;
        int windowHeight = 540;


        //Sprites
        Texture2D spriteTileGrass;
        Texture2D spritePieceDefenderNormal;
        Texture2D spritePieceDefenderKing;
        Texture2D spritePieceAttackerNormal;
        Texture2D spriteRefugeCorner;
        Texture2D spriteRefugeCenter;
        Texture2D spriteSelectedPiece;
        Texture2D spriteLegalMove;

        //Fonts
        SpriteFont normalFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
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

            gameplayHandler = new GameplayHandler(board, windowWidth, windowHeight);
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
            spriteRefugeCorner = Content.Load<Texture2D>(@"Refuges\tile_refuge_corner");
            spriteRefugeCenter = Content.Load<Texture2D>(@"Refuges\tile_refuge_center");
            spriteSelectedPiece = Content.Load<Texture2D>(@"Layers\selected_piece");
            spriteLegalMove = Content.Load<Texture2D>(@"Layers\legal_move");

            //Fonts
            normalFont = Content.Load<SpriteFont>(@"Fonts\normal");
        }

        protected override void UnloadContent()
        {
           
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawBoard();
            DrawGUI();

            base.Draw(gameTime);
            spriteBatch.End();
        }

        private void DrawBoard()
        {
            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    DrawTiles(x, y);
                    DrawMouseHover(x, y);
                    DrawSelectedPiece(x ,y);
                    DrawPieces(x, y);
                }
            }
        }

        private void DrawTiles(int x, int y)
        {
            var spriteWidth = 40;
            var spriteHeight = 40;
            var boardWidth = spriteWidth * board.Columns;
            var boardHeight = spriteHeight * board.Rows;
            var drawStartX = (windowWidth / 2) - (boardWidth / 2);
            var drawStartY = (windowHeight / 2) - (boardHeight / 2);

            var drawX = (x * spriteWidth) + drawStartX;
            var drawY = (y * spriteHeight) + drawStartY;
            var drawRect = new Rectangle(drawX, drawY, spriteWidth, spriteHeight);

            //Grass tile
            spriteBatch.Draw(spriteTileGrass, drawRect, Color.White);

            //Refuge
            if (board.Board[x, y] != null && board.Board[x, y].Team == Piece.teams.refuge)
            {
                //Center
                if (board.Board[x, y].Type == Piece.types.centerRefuge)
                {
                    spriteBatch.Draw(spriteRefugeCenter, drawRect, Color.White);
                }

                //Center
                if (board.Board[x, y].Type == Piece.types.cornerRefuge)
                {
                    spriteBatch.Draw(spriteRefugeCorner, drawRect, Color.White);
                }
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

        private void DrawMouseHover(int x, int y)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            var spriteWidth = 40;
            var spriteHeight = 40;
            var boardWidth = spriteWidth * board.Columns;
            var boardHeight = spriteHeight * board.Rows;
            var drawStartX = (windowWidth / 2) - (boardWidth / 2);
            var drawStartY = (windowHeight / 2) - (boardHeight / 2);

            var drawX = (x * spriteWidth) + drawStartX;
            var drawY = (y * spriteHeight) + drawStartY;

            if (collisionHandler.PointColisionWithBox(mousePoint.X, mousePoint.Y, drawX, drawY, spriteWidth, spriteHeight))
            {
                if (board.Board[x, y] != null && board.Board[x, y].Team == Piece.teams.attackers && board.State == PlayBoard.gameState.attackerTurn)
                {
                    var drawRect = new Rectangle(drawX, drawY, spriteWidth, spriteHeight);
                    spriteBatch.Draw(spriteSelectedPiece, drawRect, Color.White);
                }

                if (board.Board[x, y] != null && board.Board[x, y].Team == Piece.teams.defenders && board.State == PlayBoard.gameState.defenderTurn)
                {
                    var drawRect = new Rectangle(drawX, drawY, spriteWidth, spriteHeight);
                    spriteBatch.Draw(spriteSelectedPiece, drawRect, Color.White);
                }
            }
        }

        private void DrawSelectedPiece(int x, int y)
        {
            var spriteWidth = 40;
            var spriteHeight = 40;
            var boardWidth = spriteWidth * board.Columns;
            var boardHeight = spriteHeight * board.Rows;
            var drawStartX = (windowWidth / 2) - (boardWidth / 2);
            var drawStartY = (windowHeight / 2) - (boardHeight / 2);

            var drawX = (x * spriteWidth) + drawStartX;
            var drawY = (y * spriteHeight) + drawStartY;

            if (board.Board[x, y] != null && gameplayHandler.SelectedPiece != null)
            {
                if (board.Board[x, y] == gameplayHandler.SelectedPiece)
                {
                    var drawRect = new Rectangle(drawX, drawY, spriteWidth, spriteHeight);
                    spriteBatch.Draw(spriteSelectedPiece, drawRect, Color.White);
                }
            }
        }

        private void DrawPieces(int x, int y)
        {
            var spriteWidth = 40;
            var spriteHeight = 40;
            var boardWidth = spriteWidth * board.Columns;
            var boardHeight = spriteHeight * board.Rows;
            var drawStartX = (windowWidth / 2) - (boardWidth / 2);
            var drawStartY = (windowHeight / 2) - (boardHeight / 2);
            var pieceWidth = 24;
            var pieceHeight = 44;
            var pieceOffsetX = 8;
            var pieceOffsetY = -16;

            if (board.Board[x, y] != null)
            {
                var drawX = (x * spriteWidth) + drawStartX + pieceOffsetX;
                var drawY = (y * spriteHeight) + drawStartY + pieceOffsetY;
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

        private void DrawGUI()
        {
            spriteBatch.DrawString(normalFont, "Game state: " + board.State.ToString(), new Vector2(20, 20), Color.Black);
            spriteBatch.DrawString(normalFont, "Turn: " + board.Turn.ToString(), new Vector2(20, 40), Color.Black);

            var logSize = normalFont.MeasureString(board.TurnLog);
            var logPos = new Vector2(720,470 - logSize.Y);
            spriteBatch.DrawString(normalFont, "Log:\n" + board.TurnLog, logPos, Color.Black);
        }
    }
}
