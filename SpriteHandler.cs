using ClassLibrary;
using Microsoft.Xna.Framework;
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
        public SpriteHandler()
        {

        }

        public SpriteHandler(SpriteBatch spriteBatch)
        {
            Batch = spriteBatch;
        }

        public SpriteBatch Batch { get; set; }

        public void DrawSprite(Texture2D sprite, Vector2 vector2)
        {
            Batch.Draw(sprite, vector2, Color.White);
        }

        public void DrawLegalMoves(PlayBoard board, Texture2D sprite, Piece selectedPiece)
        {
            if (selectedPiece != null)
            {
                for (int column = 0; column < board.Columns; column++)
                {
                    for (int row = 0; row < board.Rows; row++)
                    {
                        if (board.LegalMoves[column, row] != null)
                        {
                            DrawSprite(sprite, board.BoardPositions[column, row]);
                        }
                    }
                }
            }
        }

        public void DrawPieces(PlayBoard board, Piece selectedPiece, Texture2D spritePieceBlack, Texture2D spritePieceBlackKing, Texture2D spritePieceWhite, Texture2D spriteSelectedPiece)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                for (int row = 0; row < board.Rows; row++)
                {
                    if (board.Board[column, row] != null)
                    {
                        //Selected ring
                        if (board.Board[column, row] == selectedPiece && selectedPiece != null)
                        {
                            DrawSprite(spriteSelectedPiece, board.BoardPositions[column, row]);
                        }

                        //Normal pieces
                        if (board.Board[column, row].Type == Piece.types.normal)
                        {
                            if (board.Board[column, row].Team == Piece.teams.attackers)
                            {
                                DrawSprite(spritePieceWhite, board.BoardPositions[column, row]);
                            }

                            if (board.Board[column, row].Team == Piece.teams.defenders)
                            {
                                DrawSprite(spritePieceBlack, board.BoardPositions[column, row]);
                            }
                        }

                        //King pieces
                        if (board.Board[column, row].Type == Piece.types.king)
                        {
                            if (board.Board[column, row].Team == Piece.teams.defenders)
                            {
                                DrawSprite(spritePieceBlackKing, board.BoardPositions[column, row]);
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
