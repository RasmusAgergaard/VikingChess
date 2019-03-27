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
                    PieceControl(CurrentMouseState, MousePoint, column, row, gameSetup);

                    KillCheck(column, row);
                }
            }

            //WinCoditionCheck

            //Return the updated version of the board
            return Board;
        }

        //Piece control
        public void PieceControl(MouseState mouseState, Point mousePoint, int column, int row, GameSetup gameSetup)
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
                    SelectPiece(column, row, gameSetup);
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
        private void SelectPiece(int column, int row, GameSetup gameSetup)
        {
            if (Board.Board[column, row] != null)
            {
                //Only select if the pieces of the team that has the turn
                if (Board.Board[column, row].Team == Piece.teams.attackers && Board.State == PlayBoard.gameState.attackerTurn)
                {
                    SelectedPiece = Board.Board[column, row];
                    SelectedPieceColumn = column;
                    SelectedPieceRow = row;
                }

                if (Board.Board[column, row].Team == Piece.teams.defenders && Board.State == PlayBoard.gameState.defenderTurn)
                {
                    SelectedPiece = Board.Board[column, row];
                    SelectedPieceColumn = column;
                    SelectedPieceRow = row;
                }
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
                    Board.Board[column, row].MovedInTurn = Board.Turn;
                    SelectedPiece = null;

                    Board.ChangeTurn();
                }
            }
        }

        private void KillCheck(int column, int row)
        {
            if (Board.Board[column, row] != null)
            {
                Piece.teams myTeam;
                Piece.teams enemyTeam;

                if (Board.State == PlayBoard.gameState.attackerFighting)
                {
                    myTeam = Piece.teams.attackers;
                    enemyTeam = Piece.teams.defenders;
                }
                else
                {
                    myTeam = Piece.teams.defenders;
                    enemyTeam = Piece.teams.attackers;
                }

                //If the piece is a enemy
                if (Board.Board[column, row].Team == enemyTeam)
                {
                    bool kingTakenColumn = false;
                    bool kingTakenRow = false;

                    //Column
                    if (column > 0 && column < Board.Columns - 1)
                    {
                        var checkColumn1 = Board.Board[column + 1, row];
                        var checkColumn2 = Board.Board[column - 1, row];

                        //Null check
                        if (checkColumn1 != null && checkColumn2 != null)
                        {
                            //Is there a piece on both sides
                            if (checkColumn1.Team == myTeam && checkColumn2.Team == myTeam)
                            {
                                //Is one of them was moved in the same turn
                                if (checkColumn1.MovedInTurn == Board.Turn || checkColumn2.MovedInTurn == Board.Turn)
                                {
                                    //Is the piece a king, then set varible
                                    if (Board.Board[column, row].Type == Piece.types.king)
                                    {
                                        kingTakenColumn = true;
                                    }
                                    //If not - kill it
                                    else
                                    {
                                        KillPiece(column, row);
                                    }
                                }
                            }
                        }
                    }

                    //Row
                    if (row > 0 && row < Board.Rows - 1)
                    {
                        var checkRow1 = Board.Board[column, row + 1];
                        var checkRow2 = Board.Board[column, row - 1];

                        //Null check
                        if (checkRow1 != null && checkRow2 != null)
                        {
                            //Is there a piece on both sides
                            if (checkRow1.Team == myTeam && checkRow2.Team == myTeam)
                            {
                                //Is one of them the last moved piece
                                if (checkRow1.MovedInTurn == Board.Turn || checkRow2.MovedInTurn == Board.Turn)
                                {
                                    //Is the piece a king, then set varible
                                    if (Board.Board[column, row].Type == Piece.types.king)
                                    {
                                        kingTakenRow = true;
                                    }
                                    //If not - kill it
                                    else
                                    {
                                        KillPiece(column, row);
                                    }
                                }
                            }
                        }
                    }
                }
            } 
        }

        private void KillPiece(int column, int row)
        {
            Board.Board[column, row] = null;
        }
    }
}
