using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class TwoPlayerGame
    {
        public event DrawDelegate DrawField;
        public event GameOverDelegate GameOver;
        public event GameStepPlayers StepPlayers;

        public bool GameIsOver { get; set; } = false;

        public Field field { get; set; } = new Field();
        public List<Player> playerList { get; set; } = new List<Player>();

        public Player x;
        public Player o;

        public int NumberOfNeutralTraps { get; set; } = 5;
        public int NumberOfLivesToDeploy { get; set; } = 5;
        public static int NumberOfLives { get; set; }

        public TwoPlayerGame(bool xHuman, bool oHuman)
        {
            // Set the borders of the field
            Field.LeftBorder = 0;
            Field.RightBorder = 79;
            Field.TopBorder = 2;
            Field.BottomBorder = 24;

            // Set maximum numbers of walls and traps
            Player.MaxNumberOfWalls = 0;
            Player.MaxNumberOfTraps = 5;
            NumberOfLives = NumberOfLivesToDeploy;

            if (xHuman)
                x = new HumanPlayer(60, 13, 'x', ConsoleColor.Blue);
            else
                x = new AIPlayer(60, 13, 'x', ConsoleColor.Blue);
            if (oHuman)
                o = new HumanPlayer(20, 13, 'o', ConsoleColor.Green);
            else
                o = new AIPlayer(20, 13, 'o', ConsoleColor.Green);
            
            o.GainedLife += OnGainedLife;
            x.GainedLife += OnGainedLife;

            playerList.Add(x);
            playerList.Add(o);

            foreach (Player player in playerList)
            {
                field.Add(player);
            }

            deployLives(NumberOfLivesToDeploy);

            deployNeutralTraps(NumberOfNeutralTraps);
        }

        private void OnGainedLife()
        {
            if (NumberOfLives > 0)
            {
                NumberOfLives--;
            }
        }

        private void initialize()
        {
            Console.WriteLine(x == null);

            o.Chaser = null;
            o.Chased = x;
            x.Chaser = o;
            x.Chased = null;

            Player.Died += OnPlayerDies;

            o.TrapDeployed += OnPlayerDeplyedTrap;
            x.TrapDeployed += OnPlayerDeplyedTrap;

            // Draw everything
            if (DrawField != null)
            {
                DrawField();
            }
        }

        public void startGame()
        {
            initialize();

            while (!GameIsOver)
            {
                StepPlayers();
            }
        }

        private void deployLives(int numberOfLivesToDeploy)
        {
            for (int i = 0; i < numberOfLivesToDeploy; i++)
            {
                deployOneLife();
            }
        }

        private void deployOneLife()
        {
            FieldCell randomEmptyCell = field.getRandomEmptyCell();

            Life newLife = new Life(randomEmptyCell.X, randomEmptyCell.Y);
            newLife.PickedUp += OnLifePickedUp;
            field.Add(newLife);
        }

        private void deployNeutralTraps(int numberOfTrapsToDeploy)
        {
            for (int i = 0; i < numberOfTrapsToDeploy; i++)
            {
                deployOneNeutralTrap();
            }
        }

        private void deployOneNeutralTrap()
        {
            FieldCell randomEmptyCell = field.getRandomEmptyCell();

            Trap newTrap = new Trap(randomEmptyCell.X, randomEmptyCell.Y, ConsoleColor.DarkMagenta, null);
            field.Add(newTrap);
        }

        public void OnPlayerDies(Player whoDied)
        {
            Player winner;
            if (whoDied.Chaser != null)
            {
                winner = whoDied.Chaser;
            }
            else
            {
                winner = whoDied.Chased;
            }
            GameIsOver = true;
            if (GameOver != null)
            {
                GameOver(winner);
            }
        }

        public void OnPlayerDeplyedTrap(EvilThing trap)
        {
            field.Add(trap);
        }

        public void OnLifePickedUp(Life life)
        {
            field.Remove(life);
            life.Clear(life.X, life.Y);
        }
    }
}
