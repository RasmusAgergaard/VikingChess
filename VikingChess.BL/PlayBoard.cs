﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VikingChessBL
{
    public class PlayBoard
    {
        public enum gameState { gameStart,
                                attackerTurn, attackerMoveing, attackerFighting, attackerWinCheck,
                                defenderTurn, defenderMoveing, defenderFighting, defenderWinCheck,
                                attackerWin, defenderWin, draw};

        public PlayBoard(int columns, int rows, Vector2 position)
        {
            Columns = columns;
            Rows = rows;
            Position = position;
            Board = new Piece[Columns, Rows];
            LegalMoves = new Legal[Columns, Rows];
            BoardPositions = new Vector2[Columns, Rows];
            FieldWidth = 40;
            FieldHeight = 40;
            State = gameState.attackerTurn;
            Turn = 0;
            TurnLog = "";

            AddPiecesToBoard();
            AddRefugesToBoard();
            CalculateBoardPositions();
        }

        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public Vector2 Position { get; private set; }
        public Piece[,] Board { get; set; }
        public Legal[,] LegalMoves { get; set; }
        public Vector2[,] BoardPositions { get; set; }
        public float FieldWidth { get; set; }
        public float FieldHeight { get; set; }
        public gameState State { get; set; }
        public int Turn { get; set; }
        public string TurnLog { get; set; }
        public bool DoesAttackersHaveKing { get; set; }
        public bool DoesDefendersHaveKing { get; set; }
        public bool DoesAttackerKingWantsToFlee { get; set; }
        public bool DoesDefenderKingWantsToFlee { get; set; }

        public void SetRules(bool doesAttackersHaveKing, bool doesDefendersHaveKing, bool doesAttackerKingWantsToFlee, bool doesDefenderKingWantsToFlee)
        {
            DoesAttackersHaveKing = doesAttackersHaveKing;
            DoesDefendersHaveKing = doesDefendersHaveKing;
            DoesAttackerKingWantsToFlee = doesAttackerKingWantsToFlee;
            DoesDefenderKingWantsToFlee = doesDefenderKingWantsToFlee;
        }

        private void AddPiecesToBoard()
        {
            var startPosition = new Vector2(0f);

            //Add to board - Attackers
            Board[0, 3] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[0, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[0, 5] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[0, 6] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[0, 7] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[1, 5] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[3, 0] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[4, 0] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[5, 0] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[5, 1] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[6, 0] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[7, 0] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[3, 10] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[4, 10] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[5, 10] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[5, 9] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[6, 10] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[7, 10] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[9, 5] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[10, 3] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[10, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[10, 5] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[10, 6] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);
            Board[10, 7] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, startPosition);

            //Add to board - Defenders
            Board[3, 5] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[4, 4] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[4, 5] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[4, 6] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[5, 3] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[5, 4] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[5, 5] = new Piece(Piece.teams.defenders, Piece.types.kingPiece, startPosition); //King
            Board[5, 6] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[5, 7] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[6, 4] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[6, 5] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[6, 6] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);
            Board[7, 5] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, startPosition);

            //Add to board - Refuges
            Board[0, 0] = new Piece(Piece.teams.refuge, Piece.types.normalPiece, startPosition);
            Board[0, 10] = new Piece(Piece.teams.refuge, Piece.types.normalPiece, startPosition);
            Board[10, 0] = new Piece(Piece.teams.refuge, Piece.types.normalPiece, startPosition);
            Board[10, 10] = new Piece(Piece.teams.refuge, Piece.types.normalPiece, startPosition);
            //Board[5, 5] aka the middle refuge haft to be added when the king moves away
        }

        public void AddRefugesToBoard()
        {
            var startPosition = new Vector2(0f);

            Board[0, 0] = new Piece(Piece.teams.refuge, Piece.types.cornerRefuge, startPosition);
            Board[0, 10] = new Piece(Piece.teams.refuge, Piece.types.cornerRefuge, startPosition);
            if (Board[5, 5] == null){ Board[5, 5] = new Piece(Piece.teams.refuge, Piece.types.centerRefuge, startPosition);}
            Board[10, 0] = new Piece(Piece.teams.refuge, Piece.types.cornerRefuge, startPosition);
            Board[10, 10] = new Piece(Piece.teams.refuge, Piece.types.cornerRefuge, startPosition);
        }

        private void CalculateBoardPositions()
        {
            var drawStartX = 960 / 2; //TODO: this should not be magic numbers
            var drawStartY = 100;
            var tileWidth = 64;
            var tileHeight = 32;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    var posX = ((x - y) * tileWidth / 2) - (tileWidth / 2) + drawStartX;
                    var posY = ((x + y) * tileHeight / 2) + drawStartY;

                    BoardPositions[x, y] = new Vector2(posX,posY);
                }

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
                    State = gameState.attackerWinCheck;
                    break;

                case gameState.attackerWinCheck:
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
                    State = gameState.defenderWinCheck;
                    break;

                case gameState.defenderWinCheck:
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
