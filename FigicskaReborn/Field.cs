using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    /// <summary>
    /// This class represents a gaming field. Defines borders.
    /// </summary>
    class Field : List<ObjectOnField>
    {
        /// <summary>
        /// X coordinate of the 
        /// </summary>
        public static int LeftBorder { get; set; }
        public static int RightBorder { get; set; }
        public static int TopBorder { get; set; }
        public static int BottomBorder { get; set; }
        public static int Width
        {
            get
            {
                return RightBorder - LeftBorder;
            }
        }
        public static int Height
        {
            get
            {
                return BottomBorder - TopBorder;
            }
        }

        public List<FieldCell> ForbiddenCells { get; set; } = new List<FieldCell>();
        public List<FieldCell> SkippeeCells { get; set; } = new List<FieldCell>();

        public List<ObjectOnField> objectsOnCoordinates(int x, int y)
        {
            List<ObjectOnField> steppedOnObjects = new List<ObjectOnField>();

            foreach (ObjectOnField objectOnField in this)
            {
                if (objectOnField.X == x && objectOnField.Y == y)
                {
                    steppedOnObjects.Add(objectOnField);
                }
            }
            return steppedOnObjects;
        }

        public void drawXY(int x, int y)
        {
            List<ObjectOnField> objectsOnXY = objectsOnCoordinates(x, y);
            foreach (ObjectOnField o in objectsOnXY)
            {
                o.Draw(o);
            }
        }

        public bool isCellOccupied(int x, int y)
        {
            return objectsOnCoordinates(x, y).Count != 0;
        }

        public bool isCellForbidden(int x, int y)
        {
            foreach (FieldCell cell in ForbiddenCells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCellSkippee(int x, int y)
        {
            foreach (FieldCell cell in SkippeeCells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public FieldCell getRandomEmptyCell()
        {
            Random random = new Random();

            bool occupied = true;
            int xCoord = 0, yCoord = 0;

            while (occupied)
            {
                xCoord = random.Next(Field.LeftBorder, Field.RightBorder);
                yCoord = random.Next(Field.TopBorder, Field.BottomBorder);
                occupied = isCellOccupied(xCoord, yCoord);
            }

            return new FieldCell(xCoord, yCoord);
        }
    }
}
