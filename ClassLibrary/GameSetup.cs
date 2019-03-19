using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class GameSetup
    {
        //public GameSetup()
        //{

        //}

        public GameSetup()
        {
            WindowWidth = 640;
            WindowHeight = 360;
            Turn = 0;
        }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int Turn { get; set; }
    }
}
