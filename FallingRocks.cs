using System;
using System.Collections.Generic;
using System.Threading;

class FallingRocks
{
    //Create a structrure for the rocks, so all the rocks will have the same properties and fields.
    public struct Rock
    {
        public int rockPositionX;
        public int rockPositionY;
        public ConsoleColor rockColor;
        public int rockSize;
        public char rockChar;
        public string rockString;
    }

    //Create a structure for the dwarf for easies management
    public struct Dwarf
    {
        public int dwarfPositionX;
        public int dwarfPositionY;
        public ConsoleColor dwarfColor;
        public string dwarfString;
    }
    //Allowed rock symbols 
    static char[] allowedRocks = { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';', '-' };

    //Method to return a color, when given a number
    static ConsoleColor allowedColors(int n)
    {
        ConsoleColor color = ConsoleColor.Black;
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
    
    //Just some basic console settings
    static void SetConsoleSettings()
    {
        Console.WindowWidth = 60;
        Console.WindowHeight = 25;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;
        Console.Title = "Falling Rocks";
    }

    //Method to write stuff on given coordinates with given color
    static void PrintOnCoords(int x, int y, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    //Create a global randomizer variable
    static Random randGen = new Random();

    //Display Game Over Sequence
    static void DisplayGameOver()
    {
        Console.Clear();
        PrintOnCoords(20, 10, "Game over!", ConsoleColor.Red);
    }
    static void Main()
    {
        //Display the hardness level, based on the score and the current score, set the initial speed
        int level = 1;
        int score = 0;
        int speed = 0;

        //Boolean variable to keep if the player is hit
        bool isAlive = true;

        //Set global console and play field parameters
        SetConsoleSettings();
        int playfieldWidth = Console.WindowWidth - 15;

        //Create the dwarf and initialize it's values;
        Dwarf dwarf = new Dwarf();
        dwarf.dwarfString = "(o)";
        dwarf.dwarfPositionX = (playfieldWidth / 2) - (dwarf.dwarfString.Length / 2);
        dwarf.dwarfPositionY = Console.WindowHeight - 1;
        dwarf.dwarfColor = ConsoleColor.Green;

        //Create the rocks
        List<Rock> rocks = new List<Rock>();

        //Main game loop
        while (isAlive == true)
        {
            //Create a new rock each time the loop is being run
            Rock newRock = new Rock();
            //Generate the new rock color
            newRock.rockColor = allowedColors(randGen.Next(1, 16));
            //Generate the new rock X coordinate, since the Y coordinate will be 0
            newRock.rockPositionX = randGen.Next(0, playfieldWidth - 1);
            //Set the new rock Y coordinate to 0
            newRock.rockPositionY = 0;
            //Generate the new rock size between 1 and 3 [1;4)
            newRock.rockSize = randGen.Next(1, 4);
            //Generate the new rock symbol, based on random number
            newRock.rockChar = allowedRocks[randGen.Next(1, allowedRocks.Length)];
            //Create the new rock string
            newRock.rockString = new string(newRock.rockChar, newRock.rockSize);
            //Add the new rock to the existing list 
            rocks.Add(newRock);
            
            //Check if any key is pressed
            if (Console.KeyAvailable)
            {
                //Get the new pressed key into new variable
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                //Clean the keys buffer
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                //Move the key to the left
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (dwarf.dwarfPositionX - 1 >= 0)
                    {
                        dwarf.dwarfPositionX--;
                    }
                }

                //Move the key to the right
                if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (dwarf.dwarfPositionX + dwarf.dwarfString.Length <= playfieldWidth)
                    {
                        dwarf.dwarfPositionX++;
                    }
                }
            }

            //Move the rocks
            //Create a new rock list to move the rocks down, because it can not be done with foreach loop
            List<Rock> newRocksList = new List<Rock>();

            //Run a loop for each rock element
            for (int i = 0; i < rocks.Count; i++)
            {
                //Set the last created rock into new variable
                Rock oldRock = rocks[i];
                //Create new temporary rock
                Rock newTempRock = new Rock();
                //Keep the X position the same
                newTempRock.rockPositionX = oldRock.rockPositionX;
                //Move the rock to the next coordinate
                newTempRock.rockPositionY = oldRock.rockPositionY + 1;
                //Keep the color
                newTempRock.rockColor = oldRock.rockColor;
                //Keep the form
                newTempRock.rockString = oldRock.rockString;
                //Keep the size
                newTempRock.rockSize = oldRock.rockSize;
                //If the Y coordinate of the new rock is lower than the screen size it will be added to the existing rock list
                if (newTempRock.rockPositionY < Console.BufferHeight)
                {
                    newRocksList.Add(newTempRock);
                }
                //If the rock reaches the bottom and it is not over the dwarf the score will become higher
                if (newTempRock.rockPositionY == Console.WindowHeight && (newTempRock.rockPositionX < dwarf.dwarfPositionX || newTempRock.rockPositionX > dwarf.dwarfPositionX + dwarf.dwarfString.Length))
                {
                    score = score + newTempRock.rockSize;
                }
                //Check if the rock is being hit
                //Check if the rock is on the same level as the dwarf
                if (newTempRock.rockPositionY == Console.WindowHeight
                    &&
                        (
                            (   //check if the left most char of the rock has the same X coordinate has the same left most char of the dwarf
                                (newTempRock.rockPositionX > dwarf.dwarfPositionX)
                                &&
                                //check if the left most char of the rock has lower coordinate than the right most coordinate of the dwarf
                                (newTempRock.rockPositionX < dwarf.dwarfPositionX + dwarf.dwarfString.Length)
                            )
                            ||
                            (   //check if the right most coordinate of the rock is inside the dwarf
                                ((newTempRock.rockPositionX + newTempRock.rockSize) > dwarf.dwarfPositionX)
                                &&
                                //check if the right most coordinate of the rock is inside the dwarf
                                ((newTempRock.rockPositionX + newTempRock.rockSize) < (dwarf.dwarfPositionX + dwarf.dwarfString.Length))
                            )
                            ||
                            (
                                //check if the rock is in the middle of the dwarf
                                (newTempRock.rockPositionX == dwarf.dwarfPositionX)
                                ||
                                //check if the rock is in the middle of the dwarf
                                (newTempRock.rockPositionX + newTempRock.rockSize) == (dwarf.dwarfPositionX + dwarf.dwarfString.Length)
                            )
                        )
                    )
                {
                    //if the above conditions are true, mark the dwarf as dead
                    isAlive = false;
                    //Display the game over sequence
                    DisplayGameOver();
                    //Display the reached score
                    PrintOnCoords(17, 11, "Your score: " + score, ConsoleColor.White);
                    //Set the cursor position for the "Press any key to continue..." so it will be in the middle of the game screen
                    Console.SetCursorPosition(12, 12);
                    //exit the game
                    return;
                }
            }
            //Update the existing rocks with the new valid rocks
            rocks = newRocksList;
            Console.Clear();

            //Print frame
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnCoords(playfieldWidth + 1, i, "|", ConsoleColor.White);
            }

            //Calculate the level
            level = score / 100;
            if (level - 1 > 0)
            {
                //set the speed
                speed = level - 1;
            }

            //If the speed reaches 15 the Thread.Sleep will be 0, so it is limited to 14
            if (speed > 15)
            {
                speed = 14;
            }

            //Redraw the dwarf and place it ot it's new position.
            PrintOnCoords(dwarf.dwarfPositionX, dwarf.dwarfPositionY, dwarf.dwarfString, dwarf.dwarfColor);
            //Print the score, level and speed in the frame
            PrintOnCoords(48, 10, "Score: " + score, ConsoleColor.White);
            PrintOnCoords(48, 11, "Level: " + level, ConsoleColor.White);
            PrintOnCoords(48, 12, "Speed: " + speed, ConsoleColor.White);
            //redraw each rock
            foreach (Rock rock in rocks)
            {
                PrintOnCoords(rock.rockPositionX, rock.rockPositionY, rock.rockString, rock.rockColor);
            }
            //run the pause for each iteration, depending on the speed and level
            Thread.Sleep(150 - speed * 10);
        }
    }
}
