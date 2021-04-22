using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class TwoPlayerConsoleUI
    {
        Dictionary<ConsoleKey, PlayerControlEnumeration> oControls = new Dictionary<ConsoleKey, PlayerControlEnumeration>();
        Dictionary<ConsoleKey, PlayerControlEnumeration> xControls = new Dictionary<ConsoleKey, PlayerControlEnumeration>();

        Dictionary<ConsoleKey, Tuple<Player, PlayerControlEnumeration>> Controls = new Dictionary<ConsoleKey, Tuple<Player, PlayerControlEnumeration>>();

        private TwoPlayerGame thisGame;

        public void startGame(bool xHuman, bool oHuman)
        {
            thisGame = new TwoPlayerGame(xHuman, oHuman);
            thisGame.DrawField += OnDrawField;
            thisGame.o.GainedLife += OnOGainedLife;
            thisGame.x.GainedLife += OnXGainedLife;
            thisGame.o.LostLife += OnOLostLife;
            thisGame.x.LostLife += OnXLostLife;
            thisGame.o.TrapDeployed += OnODeplyedTrap;
            thisGame.x.TrapDeployed += OnXDeplyedTrap;
            thisGame.GameOver += OnGameOver;
            thisGame.StepPlayers += OnStepPlayers;
            thisGame.field.SkippeeCells.Add(new FieldCell(79, 24));
            foreach (ObjectOnField o in thisGame.field)
            {
                o.Draw += OnDrawObjectOnField;
                o.Clear += OnClearObjectOnField;
            }
            defineControls();
            thisGame.startGame();
        }

        private void defineControls()
        {
            if (thisGame.o is HumanPlayer)
            {
                Controls.Add(ConsoleKey.A, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepLeft));
                Controls.Add(ConsoleKey.S, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepDown));
                Controls.Add(ConsoleKey.D, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepRight));
                Controls.Add(ConsoleKey.W, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepUp));
                Controls.Add(ConsoleKey.Q, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.DeployTrap));
            }

            if (thisGame.x is HumanPlayer)
            {
                Controls.Add(ConsoleKey.LeftArrow, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepLeft));
                Controls.Add(ConsoleKey.DownArrow, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepDown));
                Controls.Add(ConsoleKey.RightArrow, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepRight));
                Controls.Add(ConsoleKey.UpArrow, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepUp));
                Controls.Add(ConsoleKey.P, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.DeployTrap));
            }
        }

        private void OnDrawObjectOnField(ObjectOnField o)
        {
            Console.ForegroundColor = o.Color;
            Console.SetCursorPosition(o.X, o.Y);
            Console.Write(o.Character);
            Console.ResetColor();
        }

        private void OnClearObjectOnField(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }

        public void OnDrawField()
        {
            Console.Clear();
            drawBorderLine();
            drawObjectsOnField();
            drawStatusBar();
        }

        private void drawBorderLine()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, 1);
            for (int i = Field.LeftBorder; i <= Field.RightBorder; i++)
            {
                Console.Write("─");
            }
        }

        private void drawObjectsOnField()
        {
            foreach (ObjectOnField o in thisGame.field)
            {
                o.Draw(o);
            }
        }

        private void drawStatusBar()
        {
            clearStatusBar();
            drawOtrapsOnStatusBar();
            drawXtrapsOnStatusBar();
            drawOLivesOnStatusBar();
            drawXLivesOnStatusBar();
        }

        private void clearStatusBar()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = Field.LeftBorder; i <= Field.RightBorder; i++)
            {
                Console.Write(" ");
            }
        }

        //------------------
            private void drawOtrapsOnStatusBar()
            {
                Console.ForegroundColor = thisGame.o.EvilThingColor;
                for (int i = 0; i < thisGame.o.NumberOfTraps; i++)
                {
                    Console.SetCursorPosition(38 - i, 0);
                    Console.Write((char)15);
                }
                Console.ResetColor();
            }

            private void OnODeplyedTrap(EvilThing trap)
            {
                Console.ForegroundColor = thisGame.o.EvilThingColor;
                Console.SetCursorPosition(38 - thisGame.o.NumberOfTraps, 0);
                Console.Write(' ');
                Console.ResetColor();
                trap.Draw += OnDrawObjectOnField;
                trap.Draw(trap);
            }
        //------------------

        //------------------
        private void drawXtrapsOnStatusBar()
        {
            Console.ForegroundColor = thisGame.x.EvilThingColor;
            for (int i = 0; i < thisGame.x.NumberOfTraps; i++)
            {
                Console.SetCursorPosition(41 + i, 0);
                Console.Write((char)15);
            }
            Console.ResetColor();
        }

        private void OnXDeplyedTrap(EvilThing trap)
        {
            Console.ForegroundColor = thisGame.x.EvilThingColor;
            Console.SetCursorPosition(41 + thisGame.x.NumberOfTraps, 0);
            Console.Write(' ');
            Console.ResetColor();
            trap.Draw += OnDrawObjectOnField;
            trap.Draw(trap);
        }
        //------------------

        private void drawOLivesOnStatusBar()
        {
            for (int i = 0; i < thisGame.o.NumberOfLives; i++)
            {
                Console.ForegroundColor = thisGame.o.Color;
                Console.SetCursorPosition(i, 0);
                Console.Write(thisGame.o.Character);
                Console.ResetColor();
            }
        }

        private void drawXLivesOnStatusBar()
        {
            for (int i = 0; i < thisGame.x.NumberOfLives; i++)
            {
                Console.ForegroundColor = thisGame.x.Color;
                Console.SetCursorPosition(79 - i, 0);
                Console.Write(thisGame.x.Character);
                Console.ResetColor();
            }
        }

        //------------------
            private void OnOGainedLife()
            {
                Console.ForegroundColor = thisGame.o.Color;
                Console.SetCursorPosition(thisGame.o.NumberOfLives - 1, 0);
                Console.Write(thisGame.o.Character);
                Console.ResetColor();
            }

            private void OnXGainedLife()
            {
                Console.ForegroundColor = thisGame.x.Color;
                Console.SetCursorPosition(80 - thisGame.x.NumberOfLives, 0);
                Console.Write(thisGame.x.Character);
                Console.ResetColor();
            }
        //------------------

        //------------------

            private void OnOLostLife()
            {
                Console.ForegroundColor = thisGame.o.Color;
                Console.SetCursorPosition(thisGame.o.NumberOfLives, 0);
                Console.Write(' ');
                Console.ResetColor();
            }

            private void OnXLostLife()
            {
                Console.ForegroundColor = thisGame.x.Color;
                Console.SetCursorPosition(80 - thisGame.x.NumberOfLives - 1, 0);
                Console.Write(' ');
                Console.ResetColor();
            }
        //------------------

        public void OnGameOver(Player winner)
        {
            Console.Clear();
            Console.SetCursorPosition(16, 12);
            Console.Write("Nyertél, kedves {0}, gratulálok!", winner.Character);
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            { }
        }

        public void OnStepPlayers()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                Tuple<Player, PlayerControlEnumeration> pressedControl;
                Controls.TryGetValue(pressedKey.Key, out pressedControl);
                Player controlled = pressedControl.Item1;
                PlayerControlEnumeration control = pressedControl.Item2;

                controlled.executeControl(control, thisGame.field);
            }

            if (thisGame.x is AIPlayer)
            {
                AIPlayer xAsAi = thisGame.x as AIPlayer;
                if (xAsAi != null)
                {
                    xAsAi.doSomething(thisGame.field);
                }
            }

            if (thisGame.o is AIPlayer)
            {
                AIPlayer oAsAi = thisGame.o as AIPlayer;
                if (oAsAi != null)
                {
                    oAsAi.doSomething(thisGame.field);
                }
            }
        }
    }
}
