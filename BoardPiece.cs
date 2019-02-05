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
    class BoardPiece
    {
        public Vector2 position { get; set; }

        //Constructor
        public BoardPiece()
        {
            position = new Vector2(50, 50);
        }
    }
}
