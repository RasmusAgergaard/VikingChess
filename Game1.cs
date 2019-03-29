using ClassLibrary;
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
        //Init
        GameSetup gameSetup = new GameSetup();
        PlayBoard board;
        CollisionHandler collisionHandler;
        GameplayHandler gameplayHandler;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprites
        Texture2D spriteBoard;
        Texture2D spritePieceBlack;
        Texture2D spritePieceBlackKing;
        Texture2D spritePieceWhite;
        Texture2D spriteSelectedPiece;
        Texture2D spriteLegalMove;

        //Fonts
        SpriteFont normalFont;

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = gameSetup.WindowWidth;
            graphics.PreferredBackBufferHeight = gameSetup.WindowHeight;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            board = new PlayBoard(11, 11, new Vector2(0), doesAttackersHaveKing: false, doesDefendersHaveKing: true);
            gameplayHandler = new GameplayHandler(board);
            collisionHandler = new CollisionHandler();

            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Sprites
            spriteBoard = Content.Load<Texture2D>(board.SpriteName);
            spritePieceBlack = Content.Load<Texture2D>(@"Pieces\piece_black");
            spritePieceBlackKing = Content.Load<Texture2D>(@"Pieces\piece_black_king");
            spritePieceWhite = Content.Load<Texture2D>(@"Pieces\piece_white");
            spriteSelectedPiece = Content.Load<Texture2D>(@"Pieces\selected_ring");
            spriteLegalMove = Content.Load<Texture2D>(@"Board objects\legal_move");

            //Fonts
            normalFont = Content.Load<SpriteFont>(@"Fonts\normalFont");
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

                case PlayBoard.gameState.attackerWin:
                    break;

                case PlayBoard.gameState.defenderWin:
                    break;
            }



            //Base update
            base.Update(gameTime);
        }

        /*********************************** Draw ***********************************/
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.White);

            var spriteHandler = new SpriteHandler(spriteBatch);

            //Draw sprite
            spriteHandler.DrawSprite(spriteBoard, board.Position);
            spriteHandler.DrawLegalMoves(board, spriteLegalMove, gameplayHandler.SelectedPiece);
            spriteHandler.DrawPieces(board, gameplayHandler.SelectedPiece, spritePieceBlack, spritePieceBlackKing, spritePieceWhite, spriteSelectedPiece);

            //Draw text
            spriteBatch.DrawString(normalFont, "Game state: " + board.State.ToString(), new Vector2(380, 20), Color.Black);
            spriteBatch.DrawString(normalFont, "Turn: " + board.Turn.ToString(), new Vector2(380, 40), Color.Black);
            spriteBatch.DrawString(normalFont, "Fields: ", new Vector2(380, 60), Color.Black);

            //Base draw
            base.Draw(gameTime);

            spriteBatch.End();
        }

        
    }
}
