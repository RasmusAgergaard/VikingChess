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
        public int myTeam;
        public int myType;
        
        //Constructor
        public Piece(int team, int type)
        {
            //Set team
            myTeam = team;

            //Set type
            myType = type;

        }
    }
}
