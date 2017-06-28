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
    static void Main(string[] args)
    {
        string[] inputs;
        int size = int.Parse(Console.ReadLine());
        int unitsPerPlayer = int.Parse(Console.ReadLine());

        // game loop
        while (true)
        {
            for (int i = 0; i < size; i++)
            {
                string row = Console.ReadLine();
                Console.Error.WriteLine($"row is {row}\n");
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int unitX = int.Parse(inputs[0]);
                int unitY = int.Parse(inputs[1]);
            }
            for (int i = 0; i < unitsPerPlayer; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int otherX = int.Parse(inputs[0]);
                int otherY = int.Parse(inputs[1]);
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

    public class Grid
    {
        private const char Hole = '.';
        private const int HoleInt = -1;

        private int[,] _grid; 

        /// <summary>
        /// Note grid input is given from top row to bottom row so 0th row is top
        /// </summary>
        /// <param name="rows"></param>
        public Grid(string[] rows)
        {
            _grid = new int[rows.Length, rows.Length];
            for (int i = 0; i < rows.Length; i++)
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
        }
    }
}