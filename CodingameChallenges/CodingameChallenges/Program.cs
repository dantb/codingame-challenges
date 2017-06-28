using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private static Grid TheGrid;
    private static List<Unit> MyUnits;
    private static Dictionary<string, Direction> Directions = new Dictionary<string, Direction>()
    {
        { "N", Direction.North },
        { "NE", Direction.NorthEast },
        { "E", Direction.East },
        { "SE", Direction.SouthEast },
        { "S", Direction.South },
        { "SW", Direction.SouthWest },
        { "W", Direction.West },
        { "NW", Direction.NorthWest }
    };

    static void Main(string[] args)
    {
        string[] inputs;
        int size = int.Parse(Console.ReadLine());
        int unitsPerPlayer = int.Parse(Console.ReadLine());

        // game loop
        while (true)
        {
            List<string> rows = new List<string>();
            for (int i = 0; i < size; i++)
            {
                string row = Console.ReadLine();
                rows.Add(row);
            }
            TheGrid = new Grid(rows);

            MyUnits = new List<Unit>();
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int unitX = int.Parse(inputs[0]);
                int unitY = int.Parse(inputs[1]);
                MyUnits.Add(new Unit(TheGrid, unitX, unitY));
                Console.Error.WriteLine($"coords are ({unitX}, {unitY})\n");
            }

            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int otherX = int.Parse(inputs[0]);
                int otherY = int.Parse(inputs[1]);
                Console.Error.WriteLine($"coords are ({otherX}, {otherY})\n");
            }

            List<Action> actions = new List<Action>();
            int legalActions = int.Parse(Console.ReadLine());
            for (int i = 0; i < legalActions; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string atype = inputs[0];
                int index = int.Parse(inputs[1]);
                string dir1 = inputs[2];
                string dir2 = inputs[3];
                Action action = new Action(atype, index, dir1, dir2);
                actions.Add(action);
                DebugWriteLine($"Action is:  {action}");
            }

            Action actionToTake = DecideActionToTake(actions); 

            Console.WriteLine(actionToTake);
        }
    }

    private static Action DecideActionToTake(List<Action> actions)
    {
        Action bestAction = null;
        int maxMoveHeight = -1;
        int maxBuildHeight = -1;

        foreach (Action action in actions)
        {
            //the unit that can perform this action
            Unit theUnit = MyUnits[action.Index];
            Coordinates unitCo = new Coordinates(theUnit.X, theUnit.Y);
            Coordinates moveCo = unitCo.GetCoordinateInDirection(action.MoveDirection);
            int heightOfMoveSquare = TheGrid.GetHeightAt(moveCo.X, moveCo.Y);
            if (heightOfMoveSquare == 3)
            {
                //we're done, move to that square
                bestAction = action;
                break;
            }
            else
            {
                Coordinates buildCo = moveCo.GetCoordinateInDirection(action.BuildDirection);
                bool moveSquareIsMoveUp = heightOfMoveSquare == theUnit.Height + 1;
                int heightOfBuildSquareAfterBuild = TheGrid.GetHeightAt(buildCo.X, buildCo.Y) + 1;

                if (moveSquareIsMoveUp)
                {
                    if (maxMoveHeight < heightOfMoveSquare)
                    {
                        maxMoveHeight = heightOfMoveSquare;
                        maxBuildHeight = heightOfBuildSquareAfterBuild;
                        bestAction = action;
                    }
                }
                else
                {
                    if (maxMoveHeight < heightOfMoveSquare)
                    {
                        maxMoveHeight = heightOfMoveSquare;
                        maxBuildHeight = heightOfBuildSquareAfterBuild;
                        bestAction = action;
                    }
                }
            }
        }

        return bestAction;
    }

    public static void DebugWriteLine(string message = "")
    {
        Console.Error.WriteLine(message);
    }

    public static void DebugWrite(string message)
    {
        Console.Error.Write(message);
    }

    public enum Direction
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    }

    public class Grid
    {
        private const char Hole = '.';
        private const int HoleInt = -1;

        private int[,] _grid;
        private int _size;

        /// <summary>
        /// Note grid input is given from top row to bottom row so 0th row is top
        /// </summary>
        /// <param name="rows"></param>
        public Grid(List<string> rows)
        {
            _grid = new int[rows.Count, rows.Count];
            _size = rows.Count;
            for (int i = 0; i < rows.Count; i++)
            {
                string row = rows[i];
                for (int j = 0; j < row.Length; j++)
                {
                    char ch = row[j];
                    if (ch == Hole)
                    {
                        _grid[i, j] = HoleInt;
                    }
                    else
                    {
                        _grid[i, j] = int.Parse(ch.ToString());
                    }
                }
            }

            DebugWriteLine("The grid is: ");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    DebugWrite($" {_grid[i, j]}");
                }
                DebugWriteLine();
            }
        }

        public int GetHeightAt(int x, int y)
        {
            return _grid[y, x];
        }
    }

    public class Unit
    {
        private Grid _grid;

        public int X;
        public int Y;
        public int Height { get { return _grid.GetHeightAt(X, Y); } }

        public Unit(Grid grid, int x, int y)
        {
            X = x;
            Y = y;
            _grid = grid;
        }
    }

    public class Action
    {
        private string _dir1;
        private string _dir2;

        public string Type;
        public int Index;
        public Direction MoveDirection;
        public Direction BuildDirection;

        public Action(string type, int index, string dir1, string dir2)
        {
            Type = type;
            Index = index;
            MoveDirection = Directions[dir1];
            BuildDirection = Directions[dir2];
            _dir1 = dir1;
            _dir2 = dir2;
        }

        public override string ToString()
        {
            return $"{Type} {Index} {_dir1} {_dir2}";
        }
    }

    public class Coordinates
    {
        public int X;
        public int Y;
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinates GetCoordinateInDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.North:
                    return new Coordinates(X, Y - 1);
                case Direction.NorthEast:
                    return new Coordinates(X + 1, Y - 1);
                case Direction.East:
                    return new Coordinates(X + 1, Y);
                case Direction.SouthEast:
                    return new Coordinates(X + 1, Y + 1);
                case Direction.South:
                    return new Coordinates(X, Y + 1);
                case Direction.SouthWest:
                    return new Coordinates(X - 1, Y + 1);
                case Direction.West:
                    return new Coordinates(X - 1, Y);
                case Direction.NorthWest:
                    return new Coordinates(X - 1, Y - 1);
                default:
                    throw new Exception($"Invalid direction {dir}");
            }
        }
    }
}