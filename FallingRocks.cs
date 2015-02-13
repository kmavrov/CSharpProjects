using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FallingRocksNew
{
    class Program
    {
        static void SetConsoleParams()
        {
            Console.Title = "Falling Rocks";
            Console.WindowWidth = 45;
            Console.WindowHeight = 25;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.CursorVisible = false;
        }

        static void PrintOnCoords(int x, int y, string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }
        static void ClearRock(Rock someRock)
        {
            Console.SetCursorPosition(someRock.X, someRock.Y);
            string spaces = new string(' ', someRock.size);
            Console.Write(spaces);
        }

        static void ClearDwarf(Dwarf theDwarf)
        {
            Console.SetCursorPosition(theDwarf.X, theDwarf.Y);
            string spaces = new string(' ', theDwarf.form.Length);
            Console.Write(spaces);
        }

        static bool CheckForCollision(Rock tempRock, Dwarf dwarfPlayer)
        {
            for (int i = 0; i < tempRock.size; i++)
            {
                if (tempRock.X + i >= dwarfPlayer.X && tempRock.X + i < dwarfPlayer.X + dwarfPlayer.form.Length)
                {
                    return true;
                }
            }
            return false;
        }


        struct Rock
        {
            public int X;
            public int Y;
            public ConsoleColor color;
            public int size;
            public string form;
            public char symbol;
        }

        struct Dwarf
        {
            public int X;
            public int Y;
            public ConsoleColor color;
            public string form;
            public int lives;
        }

        static void DrawFrame(int playfield)
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnCoords(playfield, i, "|", ConsoleColor.White);
            }
        }
        static Random randGen = new Random();
        static char[] allowedRockSymbols = new char[] { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';', '-' };
        static ConsoleColor allowedColors(int n)
        {
            ConsoleColor color = ConsoleColor.White;
            switch (n)
            {
                case 1: color = ConsoleColor.Blue; break;
                case 2: color = ConsoleColor.Cyan; break;
                case 3: color = ConsoleColor.DarkBlue; break;
                case 4: color = ConsoleColor.DarkCyan; break;
                case 5: color = ConsoleColor.DarkGray; break;
                case 6: color = ConsoleColor.DarkGreen; break;
                case 7: color = ConsoleColor.DarkMagenta; break;
                case 8: color = ConsoleColor.DarkRed; break;
                case 9: color = ConsoleColor.DarkYellow; break;
                case 10: color = ConsoleColor.Gray; break;
                case 11: color = ConsoleColor.Green; break;
                case 12: color = ConsoleColor.Magenta; break;
                case 13: color = ConsoleColor.Red; break;
                case 14: color = ConsoleColor.White; break;
                case 15: color = ConsoleColor.Yellow; break;
            }
            return color;
        }
        static void Main(string[] args)
        {
            //Set the console parameters
            SetConsoleParams();
            int playFieldWidth = Console.WindowWidth - 15;
            //Create the dwarf and initialize some params
            Dwarf player = new Dwarf();
            player.form = "(o)";
            player.X = playFieldWidth / 2 - player.form.Length / 2;
            player.Y = Console.WindowHeight - 1;
            player.lives = 5;
            player.color = ConsoleColor.Green;
            PrintOnCoords(player.X, player.Y, player.form, player.color);

            bool isAlive = true;

            int score = 0;
            //Create list of rocks

            List<Rock> Rocks = new List<Rock>();

            while (isAlive == true)
            {

                foreach (Rock tmpRock in Rocks)
                {
                    ClearRock(tmpRock);
                }

                Rock newRock = new Rock();
                newRock.size = randGen.Next(0, 5);
                newRock.Y = 0;
                newRock.X = randGen.Next(0, playFieldWidth - newRock.size);
                newRock.symbol = allowedRockSymbols[randGen.Next(1, allowedRockSymbols.Length)];
                newRock.form = new string(newRock.symbol, newRock.size);
                newRock.color = allowedColors(randGen.Next(1, 16));
                Rocks.Add(newRock);

                List<Rock> newRocksList = new List<Rock>();

                for (int i = 0; i < Rocks.Count; i++)
                {
                    Rock oldRock = Rocks[i];
                    Rock newTempRock = new Rock();
                    newTempRock.X = oldRock.X;
                    newTempRock.form = oldRock.form;
                    newTempRock.color = oldRock.color;
                    newTempRock.size = oldRock.size;
                    newTempRock.Y = oldRock.Y + 1;
                    if (newTempRock.Y < Console.WindowHeight)
                    {
                        newRocksList.Add(newTempRock);
                    }

                    if (newTempRock.Y == Console.WindowHeight && (newTempRock.X < player.X || newTempRock.X > player.X + player.form.Length))
                    {
                        score = score + newTempRock.size;
                    }

                    if (newTempRock.Y == Console.WindowHeight)
                    {
                        if (CheckForCollision(newTempRock, player))
                        {
                            //if the above conditions are true, mark the dwarf as dead
                            player.lives--;
                            PrintOnCoords(player.X, player.Y, player.form, player.color);
                            Rocks.Clear();
                            if (player.lives == 0)
                            {
                                isAlive = false;
                            }
                        }
                    }

                }

                Rocks = newRocksList;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (player.X - 1 >= 0)
                        {
                            ClearDwarf(player);
                            player.X--;
                            PrintOnCoords(player.X, player.Y, player.form, player.color);
                        }
                    }
                    if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (player.X + 1 - player.form.Length <= playFieldWidth)
                        {
                            ClearDwarf(player);
                            player.X++;
                            PrintOnCoords(player.X, player.Y, player.form, player.color);
                        }
                    }
                }

                DrawFrame(playFieldWidth);
                foreach (Rock theRock in Rocks)
                {
                    PrintOnCoords(theRock.X, theRock.Y, theRock.form, theRock.color);
                }
                PrintOnCoords(playFieldWidth + 2, 10, "Lives: " + player.lives, ConsoleColor.White);
                PrintOnCoords(playFieldWidth + 2, 11, "Score: " + score, ConsoleColor.White);
                Thread.Sleep(150);
            }

            Console.Clear();
            PrintOnCoords(15, 10, "Game over!", ConsoleColor.Red);
            //Display the reached score
            PrintOnCoords(12, 11, "Your score: " + score, ConsoleColor.White);
            //Set the cursor position for the "Press any key to continue..." so it will be in the middle of the game screen
            Console.SetCursorPosition(7, 12);
            //exit the game
            return;
            //See if hit
            //add score/take lives
        }
    }
}
