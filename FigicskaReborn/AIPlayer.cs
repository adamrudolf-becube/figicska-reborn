using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigicskaReborn
{
    class AIPlayer : Player
    {
        static Random random = new Random();
        double stepProbability = 0.00004;
        double mistakeProbability = 0.1;

        public AIPlayer(int _X, int _Y) : base(_X, _Y)
        { }

        public AIPlayer(int _X, int _Y, char _character, ConsoleColor _color) : base(_X, _Y, _character, _color)
        {

        }

        private Life getClosestLife(Field field)
        {
            Life closestLife = null;
            int distanceOfClosestLife = Field.Width + Field.Height + 1;

            foreach (ObjectOnField currentObject in field)
            {
                if (currentObject is Life)
                {
                    int currentDistance = this.distance(currentObject);
                    if (currentDistance <= distanceOfClosestLife)
                    {
                        distanceOfClosestLife = currentDistance;
                        closestLife = currentObject as Life;
                    }
                }
            }

            return closestLife;
        }

        public void doSomething(Field field)
        {
            Random ownRandom = new Random(random.Next(10000));
            Direction stepDirection = new Direction();

            double randomDouble = ownRandom.NextDouble();

            if (randomDouble < stepProbability)
            {
                stepDirection = getIntelligentDirection(field);

                intelligentStep(field, stepDirection);
            }
        }

        private void intelligentStep(Field field, Direction direction)
        {
            AIPlayer fieldToDirection = new AIPlayer(this.X, this.Y);

            bool isCellDangerous = false;
            bool isCellImpossible = false;

            do
            {
                fieldToDirection.relocate(direction);
            } while (field.isCellSkippee(fieldToDirection.X, fieldToDirection.Y));

            List<ObjectOnField> objectsOnNewPlace = field.objectsOnCoordinates(fieldToDirection.X, fieldToDirection.Y);

            foreach(ObjectOnField currentObject in objectsOnNewPlace)
            {
                if (currentObject is Trap)
                {
                    isCellDangerous = true;
                    break;
                }

                if (currentObject == Chaser)
                {
                    isCellDangerous = true;
                    break;
                }
            }

            isCellImpossible = field.isCellForbidden(fieldToDirection.X, fieldToDirection.Y);

            if (isCellImpossible)
            {
                Direction newDirection;

                if (random.Next(2) == 0)
                {
                    newDirection = turnRight(direction);
                }
                else
                {
                    newDirection = turnLeft(direction);
                }
                
                intelligentStep(field, newDirection);
            }

            if (isCellDangerous)
            {
                Direction newDirection = direction;

                if (random.NextDouble() > mistakeProbability)
                {
                    if (random.Next(2) == 0)
                    {
                        newDirection = turnRight(direction);
                    }
                    else
                    {
                        newDirection = turnLeft(direction);
                    }
                }

                intelligentStep(field, newDirection);
            }

            step(field, direction);
        }

        private Direction directionTo(int _X, int _Y)
        {
            Direction returnDirection = Direction.Nowhere;
            Direction horizontal = Direction.Nowhere;
            Direction vertical = Direction.Nowhere;

            if (X != _X)
            {
                if (X < _X)
                {
                    if (X < _X - Field.Width / 2)
                    {
                        horizontal = Direction.Left;
                    }
                    else
                    {
                        horizontal = Direction.Right;
                    }
                }
                else
                {
                    if (X > _X + Field.Width / 2)
                    {
                        horizontal = Direction.Right;
                    }
                    else
                    {
                        horizontal = Direction.Left;
                    }
                }
            }

            if (Y != _Y)
            {
                if (Y < _Y)
                {
                    if (Y < _Y - Field.Height / 2)
                    {
                        vertical = Direction.Up;
                    }
                    else
                    {
                        vertical = Direction.Down;
                    }
                }
                else
                {
                    if (Y > _Y + Field.Height / 2)
                    {
                        vertical = Direction.Down;
                    }
                    else
                    {
                        vertical = Direction.Up;
                    }
                }
            }

            bool coinToss = random.Next(2) == 0;

            if (vertical == Direction.Up)
            {
                if (horizontal == Direction.Left)
                {
                    if (coinToss)
                    {
                        returnDirection = Direction.Left;
                    }
                    else
                    {
                        returnDirection = Direction.Up;
                    }
                }
                else if (horizontal == Direction.Nowhere)
                {
                    returnDirection = Direction.Up;
                }
                else if (horizontal == Direction.Right)
                {
                    if (coinToss)
                    {
                        returnDirection = Direction.Right;
                    }
                    else
                    {
                        returnDirection = Direction.Up;
                    }
                }
            }
            else if (vertical == Direction.Nowhere)
            {
                if (horizontal == Direction.Left)
                {
                    returnDirection = Direction.Left;
                }
                else if (horizontal == Direction.Nowhere)
                {
                    returnDirection = Direction.Nowhere;
                }
                else if (horizontal == Direction.Right)
                {
                    returnDirection = Direction.Right;
                }
            }
            else if (vertical == Direction.Down)
            {
                if (horizontal == Direction.Left)
                {
                    if (coinToss)
                    {
                        returnDirection = Direction.Left;
                    }
                    else
                    {
                        returnDirection = Direction.Down;
                    }
                }
                else if (horizontal == Direction.Nowhere)
                {
                    returnDirection = Direction.Down;
                }
                else if (horizontal == Direction.Right)
                {
                    if (coinToss)
                    {
                        returnDirection = Direction.Right;
                    }
                    else
                    {
                        returnDirection = Direction.Down;
                    }
                }
            }

            return returnDirection;
        }

        private Direction directionFrom(int _X, int _Y)
        {
            Direction returnDirection;

            returnDirection = directionTo(_X, _Y);

            if (returnDirection == Direction.Right)
            {
                returnDirection = Direction.Left;
            }
            if (returnDirection == Direction.Left)
            {
                returnDirection = Direction.Right;
            }
            if (returnDirection == Direction.Up)
            {
                returnDirection = Direction.Down;
            }
            if (returnDirection == Direction.Down)
            {
                returnDirection = Direction.Up;
            }
            return returnDirection;
        }

        // if chaser is closer than 15, flee
        // if chased is closer than 23, chase
        // else collect closes heart
        // if no heart, chase chased
        public Direction getIntelligentDirection(Field field)
        {
            int chaserDistance = this.distance(Chaser);
            int chasedDistance = this.distance(Chased);
            Life closestLife = getClosestLife(field);
            int lifeDistance = this.distance(closestLife);

            Direction stepDirection = Direction.Nowhere;
            
            if (chaserDistance < 15)
            {
                if (Chaser != null)
                {
                    stepDirection = directionFrom(Chaser.X, Chaser.Y);
                }
            }
            else if (chasedDistance < 23 || TwoPlayerGame.NumberOfLives == 0)
            {
                if (Chased != null)
                {
                    stepDirection = directionTo(Chased.X, Chased.Y);
                }
            }
            else
            {
                stepDirection = directionTo(closestLife.X, closestLife.Y);
            }

            return stepDirection;
        }
    }
}
