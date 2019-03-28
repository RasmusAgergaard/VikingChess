using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PlayBoard
    {
        public enum gameState { gameStart, attackerTurn, attackerMoveing, attackerFighting, defenderTurn, defenderMoveing, defenderFighting, attackerWin, defenderWin };


        //Constructor
        public PlayBoard()
        {

        }

        //Overloaded Constructor
        public PlayBoard(int columns, int rows, Vector2 position)
        {
            Columns = columns;
            Rows = rows;
            Position = position;
            Board = new Piece[Columns, Rows];
            LegalMoves = new Legal[Columns, Rows];
            BoardPositions = new Vector2[Columns, Rows];
            SpriteName = "board_360x360";
            BoardWidth = 360;
            BoardHeight = 360;
            FieldWidth = (float)BoardWidth / Rows;
            FieldHeight = (float)BoardHeight / Columns;
            State = gameState.attackerTurn;
            Turn = 0;

            AddPiecesToBoard();
            CalculateBoardPositions();
        }

        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public Vector2 Position { get; private set; }
        public Piece[,] Board { get; set; }
        public Legal[,] LegalMoves { get; set; }
        public Vector2[,] BoardPositions { get; set; }
        public String SpriteName { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public float FieldWidth { get; set; }
        public float FieldHeight { get; set; }
        public gameState State { get; set; }
        public int Turn { get; set; }

        public void AddPiecesToBoard()
        {
            var startPosition = new Vector2(0f);

            //Add to board - Attackers
            Board[0, 3] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[0, 4] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[0, 5] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[0, 6] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[0, 7] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[1, 5] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[3, 0] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[4, 0] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[5, 0] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[5, 1] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[6, 0] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[7, 0] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[3, 10] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[4, 10] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[5, 10] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[5, 9] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[6, 10] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[7, 10] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[9, 5] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[10, 3] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[10, 4] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[10, 5] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[10, 6] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);
            Board[10, 7] = new Piece(Piece.teams.attackers, Piece.types.normal, startPosition);

            //Add to board - Defenders
            Board[3, 5] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[4, 4] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[4, 5] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[4, 6] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[5, 3] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[5, 4] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[5, 5] = new Piece(Piece.teams.defenders, Piece.types.king, startPosition); //King
            Board[5, 6] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[5, 7] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[6, 4] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[6, 5] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[6, 6] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);
            Board[7, 5] = new Piece(Piece.teams.defenders, Piece.types.normal, startPosition);

            //Add to board - Refuges
            Board[0, 0] = new Piece(Piece.teams.refuge, Piece.types.normal, startPosition);
            Board[0, 10] = new Piece(Piece.teams.refuge, Piece.types.normal, startPosition);
            Board[10, 0] = new Piece(Piece.teams.refuge, Piece.types.normal, startPosition);
            Board[10, 10] = new Piece(Piece.teams.refuge, Piece.types.normal, startPosition);
            //Board[5, 5] aka the middle refuge haft to be added when the king moves away
        }

        private void CalculateBoardPositions()
        {
            var fieldSize = FieldWidth;
            var columnPos = 0f;
            var rowPos = 0f;

            for (int column = 0; column < Columns; column++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    BoardPositions[column, row] = new Vector2(columnPos, rowPos);
                    rowPos += fieldSize;
                }

                columnPos += fieldSize;
                rowPos = 0f;
            }
        }

        public void SetTurn(gameState gameState)
        {
            State = gameState;
        }

        public void ChangeTurn()
        {
            switch (State)
            {
                case gameState.gameStart:
                    State = gameState.attackerTurn;
                    break;

                case gameState.attackerTurn:
                    State = gameState.attackerMoveing;
                    Turn += 1;
                    break;

                case gameState.attackerMoveing:
                    State = gameState.attackerFighting;
                    break;

                case gameState.attackerFighting:
                    State = gameState.defenderTurn;
                    break;

                case gameState.defenderTurn:
                    State = gameState.defenderMoveing;
                    Turn += 1;
                    break;

                case gameState.defenderMoveing:
                    State = gameState.defenderFighting;
                    break;

                case gameState.defenderFighting:
                    State = gameState.attackerTurn;
                    break;

                case gameState.attackerWin:
                    break;

                case gameState.defenderWin:
                    break;
            }
        }
    }
}
