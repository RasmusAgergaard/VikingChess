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
        }

        public PlayBoard Board { get; set; }
        public Piece SelectedPiece { get; set; }
        public int SelectedPieceColumn { get; set; }
        public int SelectedPieceRow { get; set; }
        public MouseState CurrentMouseState { get; set; }
        public MouseState OldMouseState { get; set; }
        public Point MousePoint { get; set; }

        //Update
        public PlayBoard Update(GameSetup gameSetup)
        {
            CurrentMouseState = Mouse.GetState();
            MousePoint = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            //Main update loop
            for (int column = 0; column < Board.Columns; column++)
            {
                for (int row = 0; row < Board.Rows; row++)
                {
                    PieceControl(CurrentMouseState, MousePoint, column, row);
                }
            }

            return Board;
            
        }

        //Piece control
        public void PieceControl(MouseState mouseState, Point mousePoint, int column, int row)
        {
            if (collisionHandler.PointColisionWithBox(mousePoint.X, mousePoint.Y, Board.Position.X + (Board.FieldWidth * column), Board.Position.Y + (Board.FieldHeight * row), Board.FieldWidth, Board.FieldHeight))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
                {
                    
                    SelectPiece(column, row);
                    MovePiece(column, row);
                    //DeselectPiece(column, row);
                }

                OldMouseState = mouseState;
            }
        }

        //Select piece
        private void SelectPiece(int column, int row)
        {
            if (Board.Board[column, row] != null)
            {
                SelectedPiece = Board.Board[column, row];
                SelectedPieceColumn = column;
                SelectedPieceRow = row;
            }
        }

        //Deselect piece
        private void DeselectPiece(int column, int row)
        {
            if (Board.Board[column, row] == null && SelectedPiece != null)
            {
                SelectedPiece = null;
            } 
        }

        //Move piece
        private void MovePiece(int column, int row)
        {
            if (Board.Board[column, row] == null)
            {
                if (SelectedPiece != null)
                {
                    Board.Board[column, row] = SelectedPiece;
                    Board.Board[SelectedPieceColumn, SelectedPieceRow] = null;
                    SelectedPiece = null;
                }
            }
        }
    }
}
