using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class Trap : EvilThing
    {
        public Trap(int _X, int _Y, ConsoleColor color, Player owner) : base(_X, _Y, (char)15, color, owner)
        {

        }
    }
}
