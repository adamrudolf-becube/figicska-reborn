using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FigicskaReborn
{
    class Program
    {
        static void Main(string[] args)
        {
            TwoPlayerConsoleUI game1 = new TwoPlayerConsoleUI();

            game1.startGame(false, false);
        }


    }
}
