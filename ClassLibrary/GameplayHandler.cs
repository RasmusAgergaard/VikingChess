using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class GameplayHandler
    {
        CollisionHandler collisionHandler = new CollisionHandler();

        public GameplayHandler()
        {

        }

        public GameplayHandler(PlayBoard board)
        {
            Board = board;
            UpdateMouseStateAndPoint();
        }

        public PlayBoard Board { get; set; }
        public Piece SelectedPiece { get; set; }
        public MouseState CurrentMouseState { get; set; }
        public MouseState OldMouseState { get; set; }
        public Point MousePoint { get; set; }

        public void Update(GameSetup gameSetup)
        {
            UpdateMouseStateAndPoint();

            //Main update loop
            for (int column = 0; column < Board.Columns; column++)
            {
                for (int row = 0; row < Board.Rows; row++)
                {
                    PieceControl(CurrentMouseState, MousePoint, column, row);
                }
            }
            
        }

        public void UpdateMouseStateAndPoint()
        {
            CurrentMouseState = Mouse.GetState();
            MousePoint = new Point(CurrentMouseState.X, CurrentMouseState.Y);
        }

        public void PieceControl(MouseState mouseState, Point mousePoint, int column, int row)
        {
            if (collisionHandler.PointColisionWithBox(mousePoint.X, mousePoint.Y, 
                                                      Board.Position.X + (Board.FieldWidth * column), 
                                                      Board.Position.Y + (Board.FieldHeight * row), 
                                                      Board.FieldWidth, Board.FieldHeight))
            {
                SelectPiece(mouseState, column, row);
                DeselectPiece(mouseState, column, row);
                //Move piece
            }
        }

        private void SelectPiece(MouseState mouseState, int column, int row)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
            {
                if (Board.Board[column, row] != null)
                {
                    SelectedPiece = Board.Board[column, row];
                }
            }

            //Reassigns the old state so that it is ready for next time
            OldMouseState = mouseState;
        }

        private void DeselectPiece(MouseState mouseState, int column, int row)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
            {
                if (Board.Board[column, row] == null)
                {
                    SelectedPiece = null;
                }
            }

            //Reassigns the old state so that it is ready for next time
            OldMouseState = mouseState;
        }
    }
}
