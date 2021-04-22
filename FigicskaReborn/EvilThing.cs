using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class EvilThing : ObjectOnField
    {
        public Player Owner { get; protected set; }

        public EvilThing(int _X, int _Y, char _character, ConsoleColor _color, Player owner) : base(_X, _Y, _character, _color)
        {
            Owner = owner;
        }
    }
}
