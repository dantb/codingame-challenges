using System;

namespace CodingameChallenges
{
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
    }
}
