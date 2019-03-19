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
        private int windowHeight = 360;
        private int windowWidth = 640;
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
        private enum gameState {gameStart, whiteTurn, whiteMoveing, whiteFighting, blackTurn, blackMoveing, blackFighting, whiteWin, blackWin };
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
            _bkgPos = new Vector2(0,0);


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


            //Draw text
            //DrawText();
            spriteBatch.DrawString(font, "Game state: " + currentGameState.ToString(), new Vector2(30, 480), Color.Black);
            spriteBatch.DrawString(font, "Turn: " + turn.ToString(), new Vector2(30, 510), Color.Black);
            spriteBatch.DrawString(font, "AI log: " + AiLog, new Vector2(30, 540), Color.Black);

            //Base draw
            base.Draw(gameTime);

            spriteBatch.End();
        }

    }
}
