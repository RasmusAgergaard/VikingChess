using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class GameSetup
    {

        public GameSetup()
        {
            WindowWidth = 640;
            WindowHeight = 360;
            Zoom = 1f; //0.35f
        }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public float Zoom { get; set; }
    }
}
