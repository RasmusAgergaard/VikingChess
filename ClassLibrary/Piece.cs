using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Piece
    {
        //Init
        public enum teams { attackers, defenders, refuge};
        public enum types { normal, king};
        
        //Constructor
        public Piece()
        {

        }

        //Overloaded constructor
        public Piece(teams team, types type, Vector2 position)
        {
            Team = team;
            Type = type;
            Position = position;
            DrawPosition = position;
            MovedInTurn = 0;
        }

        //Properties
        public teams Team { get; private set; }
        public types Type { get; private set; }
        public Vector2 Position { get; set; } 
        public Vector2 DrawPosition { get; set; }
        public int MovedInTurn { get; set; }
    }
}
