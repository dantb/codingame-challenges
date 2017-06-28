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

            int legalActions = int.Parse(Console.ReadLine());
            for (int i = 0; i < legalActions; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string atype = inputs[0];
                int index = int.Parse(inputs[1]);
                string dir1 = inputs[2];
                string dir2 = inputs[3];
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine("MOVE&BUILD 0 N S");
        }
    }

    public static void DebugWriteLine(string message = "")
    {
        Console.Error.WriteLine(message);
    }

    public static void DebugWrite(string message)
    {
        Console.Error.Write(message);
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

    //public class Action
}