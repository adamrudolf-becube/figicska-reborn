using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    public class ObjectOnField : FieldCell
    {
        /// <summary>
        /// The display character of the 
        /// </summary>
        public char Character { get; protected set; }

        /// <summary>
        /// Writing color of the ObjectOnField instance.
        /// </summary>
        public ConsoleColor Color { get; protected set; }

        public ObjectOnFieldClearDelegate Clear;
        public ObjectOnFieldDrawDelegate Draw;

        public ObjectOnField(int _X, int _Y) : base(_X, _Y)
        { }

        public ObjectOnField(int _X, int _Y, char _character, ConsoleColor _color) : base(_X, _Y)
        {
            Character = _character;
            Color = _color;
        }

        public bool isAtSamePlaceAs(ObjectOnField objectOnField)
        {
            if (X == objectOnField.X && Y == objectOnField.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void relocate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    Y++;
                    break;
                case Direction.Left:
                    X--;
                    break;
                case Direction.Right:
                    X++;
                    break;
                case Direction.Up:
                    Y--;
                    break;
            }
        }

        public Direction turnRight(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
            }
            return Direction.Right;
        }

        public Direction turnLeft(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
            }
            return Direction.Left;
        }

        public int distance(ObjectOnField other)
        {
            if (other == null)
            {
                return Field.Width + Field.Height + 1;
            }
            int xDist, yDist;

            xDist = Math.Abs(this.X - other.X);
            yDist = Math.Abs(this.Y - other.Y);

            if (xDist > Field.Width)
            {
                xDist = Field.Width - xDist;
            }
            if (yDist > Field.Height)
            {
                yDist = Field.Height - yDist;
            }
            return xDist + yDist;
        }
    }
}
