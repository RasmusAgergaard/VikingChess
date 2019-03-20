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
        //System
        GameSetup gameSetup = new GameSetup();

        PlayBoard board = new PlayBoard(11, 11, new Vector2(0));


        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera
        private ResolutionIndependentRenderer _resolutionIndependence;
        private Camera2D _camera;
        private SpriteFont _debugFont;
        private InputHelper _inputHelper;
        private Vector2 _screenMousePos;
        private Vector2 _mouseDrawPos;
        private float _rotationDiff = 0.02f;
        private float _zoomDiff = 0.02f;
        private Vector2 _instructionsDrawPos;
        private string _instructions = "Use (+/-) to Zoom  And (Shift +/-) to Rotate. (R to reset)";

        //Game


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
        Texture2D fullWindowSprite;

        //Fonts
        private SpriteFont font;


        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            _resolutionIndependence = new ResolutionIndependentRenderer(this, gameSetup.WindowWidth, gameSetup.WindowHeight);
            _inputHelper = new InputHelper();
            graphics.PreferredBackBufferWidth = gameSetup.WindowWidth;
            graphics.PreferredBackBufferHeight = gameSetup.WindowHeight;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //System
            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Camera
            _camera = new Camera2D(_resolutionIndependence);
            _camera.Zoom = gameSetup.Zoom;
            _camera.Position = new Vector2(_resolutionIndependence.VirtualWidth / 2, _resolutionIndependence.VirtualHeight / 2);
            //_camera.Position = new Vector2(0);

            InitializeResolutionIndependence(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            _mouseDrawPos.X = 700;
            _mouseDrawPos.Y = 40f;

            _instructionsDrawPos.X = 600;
            _instructionsDrawPos.Y = 10f;

            _debugFont = Content.Load<SpriteFont>(@"font");
            board.Sprite = Content.Load<Texture2D>(@"board");

            fullWindowSprite = Content.Load<Texture2D>(@"full_window");


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
            _resolutionIndependence.VirtualWidth = gameSetup.WindowWidth;
            _resolutionIndependence.VirtualHeight = gameSetup.WindowHeight;
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
            //Camera
            _resolutionIndependence.BeginDraw();

            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, _camera.GetViewTransformationMatrix());
            spriteBatch.Draw(fullWindowSprite, board.Position, Color.White);
            spriteBatch.DrawString(_debugFont, string.Format("Translated Mouse Pos: x:{0:0}  y:{1:0}", _screenMousePos.X, _screenMousePos.Y), _mouseDrawPos, Color.Black);
            //spriteBatch.DrawString(_debugFont, string.Format("Real Mouse Pos: x:{0:0}  y:{1:0}", _inputHelper.MousePosition.X, _inputHelper.MousePosition.Y), _mouseDrawPos, Color.Black);
            spriteBatch.DrawString(_debugFont, _instructions, _instructionsDrawPos, Color.Yellow);
            
            //Draw board
            //DrawSprite(boardX, boardY, spriteBoard, Color.White, new Rectangle(0, 0, 200, 200));
            //spriteBatch.Draw(spriteBoard, Vector2.Zero, new Rectangle(0, 0, 200, 200), Color.White);


            //Draw text
            //DrawText();
            spriteBatch.DrawString(font, "Game state: " + gameSetup.State.ToString(), new Vector2(30, 480), Color.Black);
            spriteBatch.DrawString(font, "Turn: " + gameSetup.Turn.ToString(), new Vector2(30, 510), Color.Black);
            spriteBatch.DrawString(font, "AI log: " + AiLog, new Vector2(30, 540), Color.Black);

            //Base draw
            base.Draw(gameTime);

            spriteBatch.End();
        }

    }
}
