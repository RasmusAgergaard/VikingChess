using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CollisionHandler
    {
        public bool PointColisionWithBox(float pointX, float pointY, float boxX, float boxY, float boxWidth, float boxHeight)
        {
            if (pointX > boxX && pointX < boxX + boxWidth && pointY > boxY && pointY < boxY + boxHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
