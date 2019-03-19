using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Board
    {
        //Constructor
        public Board()
        {

        }

        //Overloaded Constructor
        public Board(int columns, int rows, Vector2 position)
        {
            Columns = columns;
            Rows = rows;
            Position = position;
            Pieces = new Piece[Columns, Rows];
            LegalMoves = new Legal[Columns, Rows];
        }


        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public Vector2 Position { get; private set; }
        public Piece[,] Pieces { get; set; }
        public Legal[,] LegalMoves { get; set; }

        public void AddPiecesToBoard()
        {

        }
    }
}
