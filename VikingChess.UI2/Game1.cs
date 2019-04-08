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

        int resolutionDevider = 1;

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
            graphics.PreferredBackBufferWidth = 1920 / resolutionDevider;
            graphics.PreferredBackBufferHeight = 1080 / resolutionDevider;
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var windowWidth = GraphicsDevice.Viewport.Width;
            var windowHeight = GraphicsDevice.Viewport.Height;
            var spriteSize = 128 / resolutionDevider;
            var squareWidth = 128 / resolutionDevider;
            var squareHeight = 64 / resolutionDevider;
            var drawStartX = windowWidth / 2 - (spriteSize / 2);
            var drawStartY = 150 / resolutionDevider;

            var pieceWidth = 48 / resolutionDevider;
            var pieceHeight = 88 / resolutionDevider;
            var pieceOffsetX = 40 / resolutionDevider;
            var pieceOffsetY = 12 / resolutionDevider;

            //Tiles
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
                        if (board.LegalMoves[x,y] != null)
                        {
                            spriteBatch.Draw(spriteLegalMove, drawTileRect, Color.White);
                        }
                    }
                }
            }

            //Pieces
            for (int x = 0; x < board.Columns; x++)
            {
                for (int y = 0; y < board.Rows; y++)
                {
                    if (board.Board[x,y] != null)
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

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
