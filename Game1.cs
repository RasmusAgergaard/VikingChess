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
        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Sprites
        Texture2D spritePiece;

        //Pieces
        private BoardPiece[,] board;
        private List<BoardPiece> boardPieces;

        //Piece
        private BoardPiece piece1;
        private BoardPiece piece2;
        private BoardPiece piece3;

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            //Window.IsBorderless = true;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Initialize
            board = new BoardPiece[11, 11];
            piece1 = new BoardPiece();
            piece2 = new BoardPiece();
            piece3 = new BoardPiece();

            //Add pieces to board
            board[0, 0] = piece1;
            board[2, 2] = piece2;
            board[4, 4] = piece3;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spritePiece = Content.Load<Texture2D>("piece");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
                

            //Update

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            //Draw


            base.Draw(gameTime);
        }
    }
}
