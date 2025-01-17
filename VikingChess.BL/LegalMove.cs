﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingChessBL
{
    public class LegalMove
    {
        public PlayBoard FindLegalMoves(PlayBoard board, Piece selectedPiece, int selectedPieceColumn, int selectedPieceRow)
        {
            ResetMoves(board);
            AddRowsAndColumns(board, selectedPiece, selectedPieceColumn, selectedPieceRow);
            RemoveSelectedPiece(board, selectedPieceColumn, selectedPieceRow);
            RemoveRefuges(board);
            RemovePieces(board, selectedPieceColumn, selectedPieceRow);

            return board;
        }

        public void ResetMoves(PlayBoard board)
        {
            board.LegalMoves = new Legal[board.Columns, board.Rows];
        }

        private void AddRowsAndColumns(PlayBoard board, Piece selectedPiece, int selectedPieceColumn, int selectedPieceRow)
        {
            for (int i = 0; i < board.Rows; i++)
            {
                board.LegalMoves[selectedPieceColumn, i] = new Legal();
            }

            for (int i = 0; i < board.Columns; i++)
            {
                board.LegalMoves[i, selectedPieceRow] = new Legal();
            }

        }

        private void RemoveSelectedPiece(PlayBoard board, int selectedPieceColumn, int selectedPieceRow)
        {
            board.LegalMoves[selectedPieceColumn, selectedPieceRow] = null;
        }

        private void RemoveRefuges(PlayBoard board)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                for (int row = 0; row < board.Rows; row++)
                {
                    if (board.Board[column, row] != null && board.Board[column, row].Team == Piece.teams.refuge)
                    {
                        board.LegalMoves[column, row] = null;
                    }
                }
            }
        }

        private void RemovePieces(PlayBoard board, int selectedPieceColumn, int selectedPieceRow)
        {
            //Remove pieces - Rows
            for (int row = 0; row < board.Rows; row++)
            {
                //Column
                for (int column = 0; column < board.Columns; column++)
                {
                    //A piece is found
                    if (board.Board[column, row] != null && board.Board[column, row].Team != Piece.teams.refuge)
                    {
                        //Above
                        if (column < selectedPieceColumn)
                        {
                            for (int i = column; i >= 0; i--)
                            {
                                board.LegalMoves[column - i, row] = null;
                            }
                        }

                        //Below
                        if (column > selectedPieceColumn)
                        {
                            for (int i = column; i < board.Columns; i++)
                            {
                                board.LegalMoves[i, row] = null;
                            }
                        }

                        //Right
                        if (row > selectedPieceRow)
                        {
                            for (int i = row; i < board.Rows; i++)
                            {
                                board.LegalMoves[column, i] = null;
                            }
                        }

                        //Left
                        if (row < selectedPieceRow)
                        {
                            for (int i = row; i >= 0; i--)
                            {
                                board.LegalMoves[column, row - i] = null;
                            }
                        }
                    }
                }
            }
        }
    }
}
