using System;
using System.Linq;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private const string MoveAndBuild = "MOVE&BUILD";
    private const string PushAndBuild = "PUSH&BUILD";
    private static Grid TheGrid;
    private static List<Unit> MyUnits;
    private static List<Unit> EnemyUnits;
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
                MyUnits.Add(new Unit(TheGrid, unitX, unitY, i));
            }

            EnemyUnits = new List<Unit>();
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int otherX = int.Parse(inputs[0]);
                int otherY = int.Parse(inputs[1]);
                if (otherX != -1)
                {
                    EnemyUnits.Add(new Unit(TheGrid, otherX, otherY, i));
                }
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
        List<Action> moveActions = actions.Where(a => a.Type == MoveAndBuild).ToList();
        List<Action> pushActions = actions.Where(a => a.Type == PushAndBuild).ToList();
        int maxMoveHeight = -1;
        int maxBuildHeight = -1;

        foreach (Action action in moveActions)
        {
            Unit theUnit = MyUnits[action.Index];
            Coordinates unitCo = new Coordinates(theUnit.X, theUnit.Y);
            Coordinates moveCo = unitCo.GetCoordinateInDirection(action.MoveDirection);
            int heightOfMoveSquare = TheGrid.GetHeightAt(moveCo.X, moveCo.Y);
            Coordinates buildCo = moveCo.GetCoordinateInDirection(action.BuildDirection);
            int heightOfBuildSquareAfterBuild = TheGrid.GetHeightAt(buildCo.X, buildCo.Y) + 1;
            //the unit that can perform this action
            if (heightOfMoveSquare == 3)
            {
                //we're done, move to that square
                if (theUnit.Height == 3 && heightOfBuildSquareAfterBuild == 4)
                {
                    //don't build on our own threes
                    continue;
                }
                if (heightOfBuildSquareAfterBuild == 3
                    && EnemyCanReachCoordinatesAtHeight(buildCo, heightOfBuildSquareAfterBuild))
                {
                    //don't give enemy a point
                    continue;
                }
                bestAction = action;
                break;
            }
            else
            {
                if (heightOfBuildSquareAfterBuild > heightOfMoveSquare + 1)
                {
                    //why build something that we can't move to?
                    continue;
                }

                if (heightOfBuildSquareAfterBuild == 3
                    && EnemyCanReachCoordinatesAtHeight(buildCo, heightOfBuildSquareAfterBuild))
                {
                    //don't give enemy a point
                    continue;
                }

                if (maxMoveHeight < heightOfMoveSquare)
                {
                    maxMoveHeight = heightOfMoveSquare;
                    maxBuildHeight = heightOfBuildSquareAfterBuild;
                    bestAction = action;
                }
                else if (maxMoveHeight == heightOfMoveSquare)
                {
                    //same move height, check build height
                    if (maxBuildHeight < heightOfBuildSquareAfterBuild)
                    {
                        maxMoveHeight = heightOfMoveSquare;
                        maxBuildHeight = heightOfBuildSquareAfterBuild;
                        bestAction = action;
                    }
                }
            }
        }

        //certain cases just push as not enough to gain by moving
        foreach (Action action in pushActions)
        {
            Unit theUnit = MyUnits[action.Index];
            Coordinates unitCo = new Coordinates(theUnit.X, theUnit.Y);
            Coordinates enemyCoords = unitCo.GetCoordinateInDirection(action.MoveDirection);
            Unit enemyUnit = EnemyUnits.First(e => e.Coords.Equals(enemyCoords));
            Coordinates enemyFinalLocation = enemyCoords.GetCoordinateInDirection(action.BuildDirection);
            if (theUnit.Height == 0 || theUnit.Height == 1)
            {
                //we're low down
                if (enemyUnit.Height >= 2 && TheGrid.GetHeightAt(enemyFinalLocation) < 2)
                {
                    //knock them off
                    if (TheGrid.GetHeightAt(enemyFinalLocation) == 0)
                    {
                        //knock them straight to ground
                        bestAction = action;
                        break;
                    }
                    else
                    {
                        Action betterAction = GetBetterPushActionIfPossible(actions, enemyFinalLocation);
                        bestAction = betterAction ?? action;
                        break;
                    }
                }
            }
            else
            {
                //how good is the best action move?
                Coordinates bestMoveCo = unitCo.GetCoordinateInDirection(bestAction.MoveDirection);
                int heightOfMoveSquare = TheGrid.GetHeightAt(bestMoveCo.X, bestMoveCo.Y);
                DebugWriteLine($"Height of move square is  {heightOfMoveSquare}");
                if (heightOfMoveSquare == 0)
                {
                    //we're high, but have to go all the way down... see if we can hurt them at least a bit
                    if (enemyUnit.Height >= 2 && TheGrid.GetHeightAt(enemyFinalLocation) < 2)
                    {
                        //knock them off
                        if (TheGrid.GetHeightAt(enemyFinalLocation) == 0)
                        {
                            //knock them straight to ground
                            bestAction = action;
                            break;
                        }
                        else
                        {
                            Action betterAction = GetBetterPushActionIfPossible(actions, enemyFinalLocation);
                            bestAction = betterAction ?? action;
                            break;
                        }
                    }
                }
            }
        }

        return bestAction;
    }

    private static Action GetBetterPushActionIfPossible(List<Action> actions, Coordinates enemyFinalLocation)
    {
        Action betterAction = null;
        //make sure none lower
        foreach (Action act in actions.Where(a => a.Type == PushAndBuild))
        {
            Unit friendly = MyUnits.First(u => u.Index == act.Index);
            Coordinates enCoords = friendly.Coords.GetCoordinateInDirection(act.MoveDirection);
            DebugWriteLine($"moveDirection {act.MoveDirection} enCoords are ({enCoords.X}, {enCoords.Y})");
            Unit enUnit = EnemyUnits.First(e => e.Coords.Equals(enCoords));
            Coordinates enFinalLocation = enCoords.GetCoordinateInDirection(act.BuildDirection);
            if (TheGrid.GetHeightAt(enemyFinalLocation) == 0)
            {
                //there is another push which does better 
                betterAction = act;
            }
        }

        return betterAction;
    }

    private static bool EnemyCanReachCoordinatesAtHeight(Coordinates buildCo, int heightOfBuildSquareAfterBuild)
    {
        foreach (Unit enemy in EnemyUnits)
        {
            if (buildCo.IsAdjacentTo(enemy.Coords))
            {
                if (heightOfBuildSquareAfterBuild <= enemy.Height + 1)
                {
                    //they can get there
                    return true;
                }
            }
        }
        return false;
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

    #region Game entities

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

        public int GetHeightAt(Coordinates coords)
        {
            return _grid[coords.Y, coords.X];
        }
    }

    public class Unit
    {
        private Grid _grid;

        public Coordinates Coords;
        public int X;
        public int Y;
        public int Index;
        public int Height { get { return _grid.GetHeightAt(X, Y); } }

        public Unit(Grid grid, int x, int y, int index)
        {
            X = x;
            Y = y;
            Coords = new Coordinates(x, y);
            _grid = grid;
            Index = index;
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

        public override bool Equals(object obj)
        {
            if (obj is Coordinates)
            {
                Coordinates co = obj as Coordinates;
                bool equal = co.X == X && co.Y == Y;
                return equal;
            }
            return base.Equals(obj);
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

        internal bool IsAdjacentTo(Coordinates coords)
        {
            if (Math.Abs(X - coords.X) <= 1 && Math.Abs(Y - coords.Y) <= 1)
            {
                return true;
            }
            return false;
        }
    }

    #endregion
}