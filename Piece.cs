using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingChess
{
    class Piece
    {
        Random random = new Random();

        public int myTeam;
        public int myType;
        public int drawX;
        public int drawY;
        
        //Constructor
        public Piece(int team, int type)
        {
            drawX = random.Next(500);
            drawY = random.Next(500);

            //Set team
            myTeam = team;

            //Set type
            myType = type;

        }
    }
}
