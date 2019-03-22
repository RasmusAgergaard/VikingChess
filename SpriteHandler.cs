using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingChess
{
    public class SpriteHandler
    {

        public void DrawGameBoard()
        {
            SpriteBatch.Draw(spriteBoard, board.Position, Color.White);
        }

        public void DrawPieces()
        {
            for (int column = 0; column < board.Columns; column++)
            {
                for (int row = 0; row < board.Rows; row++)
                {
                    if (board.Board[column, row] != null)
                    {
                        //Normal pieces
                        if (board.Board[column, row].Type == Piece.types.normal)
                        {
                            if (board.Board[column, row].Team == Piece.teams.attackers)
                            {
                                spriteBatch.Draw(spritePieceWhite, board.BoardPositions[column, row], Color.White);
                            }

                            if (board.Board[column, row].Team == Piece.teams.defenders)
                            {
                                spriteBatch.Draw(spritePieceBlack, board.BoardPositions[column, row], Color.White);
                            }
                        }

                        //King pieces
                        if (board.Board[column, row].Type == Piece.types.king)
                        {
                            if (board.Board[column, row].Team == Piece.teams.defenders)
                            {
                                spriteBatch.Draw(spritePieceBlackKing, board.BoardPositions[column, row], Color.White);
                            }

                            if (board.Board[column, row].Team == Piece.teams.attackers)
                            {
                                //Draw white king
                            }
                        }
                    }
                }
            }
        }
    }

    
}
