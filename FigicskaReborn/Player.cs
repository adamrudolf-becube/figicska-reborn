using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    /// <summary>
    /// This class represents a player of the game. Meant to be the parent of human and AI players.
    /// </summary>
    class Player : ObjectOnField
    {
        public int NumberOfLives { get; protected set; }

        public static int MaxNumberOfWalls { get; set; }
        public int NumberOfWalls { get; protected set; }

        public static int MaxNumberOfTraps { get; set; }
        public int NumberOfTraps { get; protected set; }

        public static readonly PlayerControlEnumeration StepLeft = PlayerControlEnumeration.StepLeft;
        public static readonly PlayerControlEnumeration StepRight = PlayerControlEnumeration.StepRight;
        public static readonly PlayerControlEnumeration StepUp = PlayerControlEnumeration.StepUp;
        public static readonly PlayerControlEnumeration StepDown = PlayerControlEnumeration.StepDown;
        public static readonly PlayerControlEnumeration DeployTrap = PlayerControlEnumeration.DeployTrap;
        public static readonly PlayerControlEnumeration DeployWall = PlayerControlEnumeration.DeployWall;

        public Player Chaser { get; set; }
        public Player Chased { get; set; }

        public List<ObjectOnField> isOnObjects { get; set; }

        public ConsoleColor EvilThingColor { get; set; }
        
        public List<Trap> WallList { get; set; }

        public event DeployedEvilThingDelegate TrapDeployed;
        public event DeployedEvilThingDelegate WallDeployed;

        public event PlayerGainedLife GainedLife;
        public event PlayerLostLife LostLife;
        public static event PlayerDied Died;
        public event PlayerMoved MovedFrom;
        public event PlayerMoved MovedTo;
        public event PlayerDecision Decide;

        public Player(int _X, int _Y) : base(_X, _Y)
        { }

        public Player(int _X, int _Y, char _character, ConsoleColor _color) : base(_X, _Y, _character, _color)
        {
            initValues(_character, _color);
        }

        public Player(int _X, int _Y, char _character, ConsoleColor _color, Player _chased, Player _chaser) : base(_X, _Y, _character, _color)
        {
            initValues(_character, _color);
            Chased = _chased;
            Chaser = _chaser;
        }

        private void initValues(char _character, ConsoleColor _color)
        {
            NumberOfLives = 1;
            NumberOfTraps = MaxNumberOfTraps;
            NumberOfWalls = MaxNumberOfWalls;
            EvilThingColor = getEvilThingColor();
            isOnObjects = new List<ObjectOnField>();
        }
        
        public void executeControl(PlayerControlEnumeration control, Field field)
        {
            switch (control)
            {
                case PlayerControlEnumeration.StepLeft:
                    step(field, Direction.Left);
                    break;
                case PlayerControlEnumeration.StepRight:
                    step(field, Direction.Right);
                    break;
                case PlayerControlEnumeration.StepUp:
                    step(field, Direction.Up);
                    break;
                case PlayerControlEnumeration.StepDown:
                    step(field, Direction.Down);
                    break;
                case PlayerControlEnumeration.DeployTrap:
                    deployTrap();
                    break;
                case PlayerControlEnumeration.DeployWall:
                    deployWall();
                    break;
                case PlayerControlEnumeration.DoNothing:
                    break;
                default:
                    break;
            }
        }

        protected void step(Field field, Direction direction)
        {
            int oldX = X;
            int oldY = Y;
            stepCoordinates(field, direction);
            move(oldX, oldY, field);
        }

        private void stepCoordinates(Field field, Direction direction)
        {
            Player newCell = new Player(X, Y);
            FieldCell originalCell = new FieldCell(X, Y);

            newCell.relocate(direction);

            if (field.isCellForbidden(newCell.X, newCell.Y))
            {
                return;
            }
            if (field.isCellSkippee(newCell.X, newCell.Y))
            {
                newCell.stepCoordinates(field, direction);
            }
            X = newCell.X;
            Y = newCell.Y;
        }

        private void move(int oldX, int oldY, Field field)
        {
            movedFrom(oldX, oldY, field);
            movedTo(field);
        }

        private void movedFrom(int oldX, int oldY, Field field)
        {
            clearIfNotNull(oldX, oldY);
            isOnObjects.Clear();
            if (MovedFrom != null)
            {
                MovedFrom(oldX, oldY);
            }
            field.drawXY(oldX, oldY);
        }

        private void movedTo(Field field)
        {
            drawIfNotNull();
            isOnObjects.Clear();
            isOnObjects = field.objectsOnCoordinates(X, Y);
            
            if (MovedTo != null)
            {
                MovedTo(X, Y);
            }

            steppedOnObjects(isOnObjects);
        }

        public void steppedOnObjects(List<ObjectOnField> objectsSteppedOn)
        {
            foreach (ObjectOnField objectSteppedOn in objectsSteppedOn)
            {
                if (objectSteppedOn == Chaser)
                {
                    gotOnChaser();
                }
                if (objectSteppedOn == Chased)
                {
                    gotOnChased();
                }
                if (objectSteppedOn is Trap)
                {
                    Trap trap = objectSteppedOn as Trap;
                    if (trap != null)
                    {
                        if (trap.Owner != this)
                        {
                            gotOnEnemyTrap();
                        }
                    }
                }
                if (objectSteppedOn is Life)
                {
                    Life life = objectSteppedOn as Life;
                    if (life != null)
                    {
                        gotOnLife(life);
                    }
                }
            }
        }

        public void gotOnChaser()
        {
            looseLife();
        }

        private void gotOnChased()
        {
            Chased.looseLife();
        }

        private void gotOnEnemyTrap()
        {
            looseLife();
        }

        private void gotOnLife(Life life)
        {
            gainLife();
            life.eliminate();
            if (Draw != null)
            {
                Draw(this);
            }
        }

        private void clearIfNotNull(int oldX, int oldY)
        {
            if (Clear != null)
            {
                Clear(oldX, oldY);
            }
        }

        private void drawIfNotNull()
        {
            if (Draw != null)
            {
                Draw(this);
            }
        }

        public void deployTrap()
        {
            if (NumberOfTraps > 0)
            {
                Trap deployedTrap = new Trap(X, Y, EvilThingColor, this);

                NumberOfTraps--;

                if (TrapDeployed != null)
                {
                    TrapDeployed(deployedTrap);
                }
            }
        }

        public void deployWall()
        {
            if (NumberOfWalls > 0)
            {
                Trap deployedWall = new Trap(X, Y, EvilThingColor, this);
                WallList.Add(deployedWall);

                if (WallDeployed != null)
                {
                    WallDeployed(deployedWall);
                }
            }
        }

        public ConsoleColor getEvilThingColor()
        {
            switch (Color)
            {
                case ConsoleColor.White:
                    return ConsoleColor.Gray;
                case ConsoleColor.Blue:
                    return ConsoleColor.DarkBlue;
                case ConsoleColor.Cyan:
                    return ConsoleColor.DarkCyan;
                case ConsoleColor.Gray:
                    return ConsoleColor.DarkGray;
                case ConsoleColor.Green:
                    return ConsoleColor.DarkGreen;
                case ConsoleColor.Magenta:
                    return ConsoleColor.DarkMagenta;
                case ConsoleColor.Red:
                    return ConsoleColor.DarkRed;
                case ConsoleColor.Yellow:
                    return ConsoleColor.DarkYellow;
                default:
                    throw new ArgumentException("Unsupported Player color.");
            }
        }

        public void gainLife()
        {
            NumberOfLives++;
            if (GainedLife != null)
            {
                GainedLife();
            }
        }

        public void looseLife()
        {
            NumberOfLives--;
            if (LostLife != null)
            {
                LostLife();
            }
            if (NumberOfLives == 0)
            {
                if (Died != null)
                {
                    Died(this);
                }
            }
        }
    }
}
