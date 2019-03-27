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
        LegalMove legalMove = new LegalMove();

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

        //Main update method
        public PlayBoard Update(GameSetup gameSetup)
        {
            //Update mouse
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

            //Kill check

            //WinCoditionCheck

            //Return the updated version of the board
            return Board;
        }

        //Piece control
        public void PieceControl(MouseState mouseState, Point mousePoint, int column, int row)
        {
            var mouseX = mousePoint.X;
            var mouseY = mousePoint.Y;
            var posX = Board.Position.X + (Board.FieldWidth * column);
            var posY = Board.Position.Y + (Board.FieldHeight * row);
            var width = Board.FieldWidth;
            var height = Board.FieldHeight;

            if (collisionHandler.PointColisionWithBox(mouseX, mouseY, posX, posY, width, height))
            {
                if (MousePress(mouseState) == true)
                {
                    SelectPiece(column, row);
                    MovePiece(column, row);

                    //Find legal moves, and return the updated board
                    Board = legalMove.FindLegalMoves(Board, SelectedPiece, SelectedPieceColumn, SelectedPieceRow);
                }
            }
        }

        //Mouse press
        private bool MousePress(MouseState mouseState)
        {
            var mousePressed = false;

            if (mouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
            {
                mousePressed = true;
            }

            OldMouseState = mouseState;
            return mousePressed;
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
            if (SelectedPiece != null)
            {
                if (Board.Board[column, row] == null && Board.LegalMoves[column, row] != null)
                {
                    //Move piece
                    Board.Board[column, row] = SelectedPiece;
                    Board.Board[SelectedPieceColumn, SelectedPieceRow] = null;
                    SelectedPiece = null;
                }
            }
        }
    }
}
