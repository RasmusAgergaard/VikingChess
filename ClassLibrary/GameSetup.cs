using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class GameSetup
    {
        public enum gameState { gameStart, whiteTurn, whiteMoveing, whiteFighting, blackTurn, blackMoveing, blackFighting, whiteWin, blackWin };

        public GameSetup()
        {
            WindowWidth = 640;
            WindowHeight = 360;
            Zoom = 1f; //0.35f
            Turn = 0;
            State = gameState.gameStart;
        }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public float Zoom { get; set; }
        public int Turn { get; set; }
        public gameState State { get; set; }
    }
}
