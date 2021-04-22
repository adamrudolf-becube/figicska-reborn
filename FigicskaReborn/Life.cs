using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class Life : ObjectOnField
    {
        public event LifePickedUp PickedUp;

        public Life(int _X, int _Y) : base(_X, _Y, '♥', ConsoleColor.Red)
        {

        }

        public void eliminate()
        {
            if (PickedUp != null)
            {
                PickedUp(this);
            }
        }
    }
}
