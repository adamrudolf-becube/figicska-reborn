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
            /*
            TwoPlayerOpenTKUI game1 = new TwoPlayerOpenTKUI();

            SimplifiedGame window = new SimplifiedGame();

            window.Run();
            */
            SimplifiedGame game = new SimplifiedGame();

            game.Run();

            //game1.startGame(true, true);
        }


    }
}
