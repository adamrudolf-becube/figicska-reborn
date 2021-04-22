using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class Wall : EvilThing
    {
        public Wall(int _X, int _Y, ConsoleColor color, Player owner) : base(_X, _Y, '█', color, owner)
        {

        }
    }
}
