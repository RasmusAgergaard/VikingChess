using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingChessBL
{
    public class GameplayHandler
    {
        CollisionHandler collisionHandler = new CollisionHandler();
        LegalMove legalMove = new LegalMove();

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

        public PlayBoard Update(GameSetup gameSetup)
        {
            CurrentMouseState = Mouse.GetState();
            MousePoint = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            //Main update loop
            for (int column = 0; column < Board.Columns; column++)
            {
                for (int row = 0; row < Board.Rows; row++)
                {
                    if (Board.State == PlayBoard.gameState.attackerTurn || Board.State == PlayBoard.gameState.defenderTurn)
                    {
                        PieceControl(CurrentMouseState, MousePoint, column, row, gameSetup);
                    }

                    if (Board.Board[column, row] != null && (Board.State == PlayBoard.gameState.attackerFighting || Board.State == PlayBoard.gameState.defenderFighting))
                    {
                        KillCheck(column, row); 
                    }
                }
            }

            WinConditionCheck();

            return Board;
        }

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
                if (MousePress(mouseState))
                {
                    SelectPiece(column, row, gameSetup);
                    MovePiece(column, row);

                    //Find legal moves, and return the updated board
                    Board = legalMove.FindLegalMoves(Board, SelectedPiece, SelectedPieceColumn, SelectedPieceRow);
                }
            }
        }

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

        private void DeselectPiece(int column, int row)
        {
            if (Board.Board[column, row] == null && SelectedPiece != null)
            {
                SelectedPiece = null;
            } 
        }

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

                    Board.ChangeTurn();
                    Board.Board[column, row].MovedInTurn = Board.Turn;
                }
            }
        }

        private void KillCheck(int column, int row)
        {
            //Set teams
            Piece.teams myTeam = Piece.teams.attackers;
            Piece.teams enemyTeam = Piece.teams.defenders;

            if (Board.State == PlayBoard.gameState.attackerFighting)
            {
                myTeam = Piece.teams.attackers;
                enemyTeam = Piece.teams.defenders;
            }
            if (Board.State == PlayBoard.gameState.defenderFighting)
            {
                myTeam = Piece.teams.defenders;
                enemyTeam = Piece.teams.attackers;
            }

            //If the piece is an enemy (opposite team)
            if (Board.Board[column, row].Team == enemyTeam)
            {
                if (Board.Board[column, row] != null && Board.Board[column, row].Type == Piece.types.normal)
                {
                    KillCheckNormalPiece(column, row, myTeam);
                }

                if (Board.Board[column, row] != null && Board.Board[column, row].Type == Piece.types.king)
                {
                    KillCheckKingPiece(column, row, myTeam);
                } 
            }
        }

        private void KillCheckNormalPiece(int column, int row, Piece.teams myTeam)
        {
            if (Board.Board[column, row].Type == Piece.types.normal)
            {
                //Column check
                if (column > 0 && column < Board.Columns - 1)
                {
                    if (IsThereAPieceOnEachSide(Board.Board[column + 1, row], Board.Board[column - 1, row], myTeam))
                    {
                        KillPiece(column, row);
                    }
                }

                //Row check
                if (row > 0 && row < Board.Rows - 1)
                {
                    if (IsThereAPieceOnEachSide(Board.Board[column, row + 1], Board.Board[column, row - 1], myTeam))
                    {
                        KillPiece(column, row);
                    }
                }
            }
        }

        private void KillCheckKingPiece(int column, int row, Piece.teams myTeam)
        {
            var killKingColumn = false;
            var killKingRow = false;

            //Column check
            if (column > 0 && column < Board.Columns - 1)
            {
                if (IsThereAPieceOnEachSideWithoutTurnCheck(Board.Board[column + 1, row], Board.Board[column - 1, row], myTeam))
                {
                    killKingColumn = true;
                }
            }

            //Row check
            if (row > 0 && row < Board.Rows - 1)
            {
                if (IsThereAPieceOnEachSideWithoutTurnCheck(Board.Board[column, row + 1], Board.Board[column, row - 1], myTeam))
                {
                    killKingRow = true;
                }
            }

            if (killKingColumn && killKingRow)
            {
                KillPiece(column, row);
            }  
        }

        private bool IsThereAPieceOnEachSide(Piece pieceToCheck1, Piece pieceToCheck2, Piece.teams myTeam)
        {
            if (pieceToCheck1 != null && pieceToCheck2 != null)
            {
                //Is there a piece on both sides
                if (pieceToCheck1.Team == myTeam && pieceToCheck2.Team == myTeam)
                {
                    //Is one of them was moved in the same turn
                    if (pieceToCheck1.MovedInTurn == Board.Turn || pieceToCheck2.MovedInTurn == Board.Turn)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsThereAPieceOnEachSideWithoutTurnCheck(Piece pieceToCheck1, Piece pieceToCheck2, Piece.teams myTeam)
        {
            if (pieceToCheck1 != null && pieceToCheck2 != null)
            {
                //Is there a piece on both sides
                if (pieceToCheck1.Team == myTeam && pieceToCheck2.Team == myTeam)
                {
                    return true;
                }
            }

            return false;
        }

        private void KillPiece(int column, int row)
        {
            Board.Board[column, row] = null;
        }

        private void WinConditionCheck()
        {
            if (Board.State == PlayBoard.gameState.attackerTurn || Board.State == PlayBoard.gameState.defenderTurn)
            {
                var doesAttackerKingExist = false;
                var doesDefenderKingExist = false;

                for (int column = 0; column < Board.Columns; column++)
                {
                    for (int row = 0; row < Board.Rows; row++)
                    {
                        if (Board.Board[column, row] != null && Board.Board[column, row].Type == Piece.types.king)
                        {
                            if (Board.Board[column, row].Team == Piece.teams.attackers)
                            {
                                doesAttackerKingExist = true;
                            }

                            if (Board.Board[column, row].Team == Piece.teams.defenders)
                            {
                                doesDefenderKingExist = true;
                            }
                        }
                    }
                }

                if (Board.DoesAttackersHaveKing && !doesAttackerKingExist)
                {
                    Board.State = PlayBoard.gameState.defenderWin;
                }

                if (Board.DoesDefendersHaveKing && !doesDefenderKingExist)
                {
                    Board.State = PlayBoard.gameState.attackerWin;
                }
            }
        }

        //TODO: Make sure if this should be used
        private bool DoesKingStillExist(int column, int row, Piece.teams team)
        {
            var doesKingExist = false;

            if (Board.Board[column, row] != null
                && Board.Board[column, row].Team == team
                && Board.Board[column, row].Type == Piece.types.king)
            {
                
            }

            return doesKingExist;
        }
    }
}
