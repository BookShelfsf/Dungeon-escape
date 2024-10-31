using System;

namespace DungeonEscape
{
    class Program
    {
        static char[,] dungeon = new char[6, 6];
        static bool[,] discoveredRooms = new bool[6, 6]; // Keeps track of discovered rooms
        static int playerX = 0;
        static int playerY = 0;
        static int exitX;
        static int exitY;
        static bool hasKey = false;
        static int lives = 3;
        static Random random = new Random();

        static void Main(string[] args)
        {
            InitializeDungeon();

            Console.WriteLine("Welcome to Dungeon Escape!");
            Console.WriteLine("Find the key and avoid traps to escape.");
            Console.WriteLine("To move, type: up, down, left, right.");
            Console.WriteLine("You start with 3 lives and lose one life each time you hit a trap.");

            while (lives > 0)
            {
                PrintDungeon();
                Console.Write("\nEnter your move: ");
                string move = Console.ReadLine().ToLower();

                int previousX = playerX;
                int previousY = playerY;

                // Update player's position based on move
                if (move == "up") playerX--;
                else if (move == "down") playerX++;
                else if (move == "left") playerY--;
                else if (move == "right") playerY++;
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    continue;
                }

                // Check if player is outside the dungeon
                if (playerX < 0 || playerX >= dungeon.GetLength(0) || playerY < 0 || playerY >= dungeon.GetLength(1))
                {
                    Console.WriteLine("You hit a wall! Try another direction.");
                    playerX = previousX;
                    playerY = previousY;
                }
                else
                {
                    discoveredRooms[playerX, playerY] = true; // Mark room as discovered
                    char currentTile = dungeon[playerX, playerY];

                    if (currentTile == 'T')
                    {
                        lives--;
                        Console.WriteLine($"You fell into a trap! You have {lives} lives left.");
                        if (lives == 0)
                        {
                            Console.WriteLine("You lost all your lives! Game over.");
                            break;
                        }
                    }
                    else if (currentTile == 'K')
                    {
                        hasKey = true;
                        Console.WriteLine("You found the key!");
                        dungeon[playerX, playerY] = ' '; // Remove key from room
                    }
                    else if (currentTile == 'E')
                    {
                        if (hasKey)
                        {
                            Console.WriteLine("You used the key and escaped! You win!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You need the key to escape!");
                        }
                    }
                }
            }
        }

        static void InitializeDungeon()
        {
            // Initialize dungeon with empty rooms
            for (int i = 0; i < dungeon.GetLength(0); i++)
            {
                for (int j = 0; j < dungeon.GetLength(1); j++)
                {
                    dungeon[i, j] = ' ';
                    discoveredRooms[i, j] = false; // All rooms start as undiscovered
                }
            }

            // Set start position
            dungeon[0, 0] = 'S';
            discoveredRooms[0, 0] = true;

            // Place exit (E) randomly, avoiding the start position (0,0)
            do
            {
                exitX = random.Next(0, 6);
                exitY = random.Next(0, 6);
            } while (exitX == 0 && exitY == 0);
            dungeon[exitX, exitY] = 'E';

            // Add random traps (T) and the key (K)
            int traps = 5; // Increased number of traps
            while (traps > 0)
            {
                int trapX = random.Next(0, 6);
                int trapY = random.Next(0, 6);

                if (dungeon[trapX, trapY] == ' ' && !(trapX == 0 && trapY == 0))
                {
                    dungeon[trapX, trapY] = 'T';
                    traps--;
                }
            }

            // Add the key (K) at a random position
            while (true)
            {
                int keyX = random.Next(0, 6);
                int keyY = random.Next(0, 6);

                if (dungeon[keyX, keyY] == ' ' && !(keyX == 0 && keyY == 0) && !(keyX == exitX && keyY == exitY))
                {
                    dungeon[keyX, keyY] = 'K';
                    break;
                }
            }
        }

        static void PrintDungeon()
        {
            for (int i = 0; i < dungeon.GetLength(0); i++)
            {
                for (int j = 0; j < dungeon.GetLength(1); j++)
                {
                    if (i == playerX && j == playerY)
                        Console.Write("P ");
                    else if (discoveredRooms[i, j])
                        Console.Write(dungeon[i, j] + " ");
                    else
                        Console.Write("? "); // Hidden room
                }
                Console.WriteLine();
            }
        }
    }
}
