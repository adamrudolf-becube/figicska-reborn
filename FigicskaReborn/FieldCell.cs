using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    public class FieldCell
    {
        private int _x;
        public int X
        {
            get
            {
                return _x;
            }
            protected set
            {
                _x = value;
                cyclize();
            }
        }

        private int _y;
        public int Y
        {
            get
            {
                return _y;
            }
            protected set
            {
                _y = value;
                cyclize();
            }
        }

        public FieldCell(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        private void cyclize()
        {
            _x = cyclizeNumber(_x, Field.LeftBorder, Field.RightBorder);
            _y = cyclizeNumber(_y, Field.TopBorder, Field.BottomBorder);
        }

        private static int cyclizeNumber(int number, int lowerBoundary, int upperBoundary)
        {
            int range = upperBoundary - lowerBoundary;
            while (number < lowerBoundary)
            {
                number += range + 1;
            }
            while (number > upperBoundary)
            {
                number -= range + 1;
            }
            return number;
        }
    }
}
