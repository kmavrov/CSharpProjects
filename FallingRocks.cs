using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    //Set some game restraints and console parameters
    static void SetConsoleParams()
    {
        Console.Title = "Falling Rocks";
        Console.WindowWidth = 45;
        Console.WindowHeight = 25;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;
    }

    //Just a little helper method to write on each coordinate we need to
    static void PrintOnCoords(int x, int y, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    //Helper method to write blanks where the rocks used to be, so the console does not have to refresh each time something moves
    static void ClearRock(Rock someRock)
    {
        Console.SetCursorPosition(someRock.X, someRock.Y);
        string spaces = new string(' ', someRock.size);
        Console.Write(spaces);
    }
    //Helper method to write blanks where the dwarf used to be, so the console does not have to refresh each time something moves
    static void ClearDwarf(Dwarf theDwarf)
    {
        Console.SetCursorPosition(theDwarf.X, theDwarf.Y);
        string spaces = new string(' ', theDwarf.form.Length);
        Console.Write(spaces);
    }

    //Method to check if each of the elements of the rock is between the borders of the dwarf and then returns the result
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

    //Structure to keep the rocks in a List<> array
    struct Rock
    {
        public int X;
        public int Y;
        public ConsoleColor color;
        public int size;
        public string form;
        public char symbol;
    }

    //Helper structure to create and manipulate the dwarf
    struct Dwarf
    {
        public int X;
        public int Y;
        public ConsoleColor color;
        public string form;
        public int lives;
    }

    //Just a simple method to draw the border between the playfield and the scoreboard
    static void DrawFrame(int playfield)
    {
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            PrintOnCoords(playfield, i, "|", ConsoleColor.White);
        }
    }

    //Simple random generator that is going to be used in the code
    static Random randGen = new Random();

    //An array of the permitted characters, which will later form the rocks
    static char[] allowedRockSymbols = new char[] { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';', '-' };

    //A method to return a color, based on random number n. The color will then be used for the rocks
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

    //The main program
    static void Main()
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

        //To keep track the player is still alive
        bool isAlive = true;
        
        //Initialize the score
        int score = 0;

        //Create list of rocks. It will later be filled with rocks, that follow certain rules
        List<Rock> Rocks = new List<Rock>();

        //While the player is alive and has lives the game will run
        while (isAlive == true)
        {
            //This clears the old rocks from the console each time the loop starts
            foreach (Rock tmpRock in Rocks)
            {
                ClearRock(tmpRock);
            }

            //Each time the loop starts we create a new rock and later add it to the list of rocks
            Rock newRock = new Rock();
            newRock.size = randGen.Next(0, 5);
            newRock.Y = 0;
            newRock.X = randGen.Next(0, playFieldWidth - newRock.size);
            newRock.symbol = allowedRockSymbols[randGen.Next(1, allowedRockSymbols.Length)];
            newRock.form = new string(newRock.symbol, newRock.size);
            newRock.color = allowedColors(randGen.Next(1, 16));
            Rocks.Add(newRock);

            //This is temporary rock list which will keep only the rocks that meet all the necessary criteria:
            //1) they will be kept only if they are within the play field (which will keep the buffer from overflowing)
            //2) this method will help us move the rocks down while the rocks are "valid" as objects
            //3) also with each rock from this list several checks will be made:
            //   a) if the rock hits the player - if yes - substract lives
            //   b) if no, but it is still on the bottom row - add points
            List<Rock> newRocksList = new List<Rock>();

            //a loop to move all the rocks from the list 
            for (int i = 0; i < Rocks.Count; i++)
            {
                //store the current rock as temporary value, so it can be manipulated
                Rock oldRock = Rocks[i];
                //create a new temporary rock, which will be moved and evaluated
                Rock newTempRock = new Rock();
                //keep the same X coordinate and other propereties
                newTempRock.X = oldRock.X;
                newTempRock.form = oldRock.form;
                newTempRock.color = oldRock.color;
                newTempRock.size = oldRock.size;

                //This is the most important row - move the rock down with one line
                newTempRock.Y = oldRock.Y + 1;
                //Check if the new Y coordinate is within the boundries of the playfield
                if (newTempRock.Y < Console.WindowHeight)
                {
                    //if the Y coordinate is valid - add the rock to the new temporary list
                    newRocksList.Add(newTempRock);
                }

                //Save all the temporary valid rocks to the global list of rocks
                //this means that some of the old rocks in the global list will no longer exist,
                //because they have reached the bottom. This ensures that only the valid rocks
                //will be stored and later displayed.
                Rocks = newRocksList;

                //This checks if the rock is on the last line (the same line as the dwarf)
                if (newTempRock.Y == Console.WindowHeight)
                {
                    //if the rock hits the dwarf
                    if (CheckForCollision(newTempRock, player))
                    {
                        //Substract one life, for each collision
                        player.lives--;
                        //Reprint the dwarf since, parts of it may be deleted by the rock clearing method
                        //this ensures no bugs and missing parts, which will reappear after movement
                        PrintOnCoords(player.X, player.Y, player.form, player.color);
                        //this method clears the list of rocks, so the game could start from the beginning, with no rocks available
                        Rocks.Clear();
                        //this checks if the player still has some lives
                        if (player.lives == 0)
                        {   //if not - game over :) the main while loop will exit and display the game over stuff
                            isAlive = false;
                        }
                    }
                    //if the rocks are on the bottom line, but do not hit the dwarf, then add some points
                    score = score + newTempRock.size;
                }

            }

            //This checks to see if there are some keys that are being pressed
            if (Console.KeyAvailable)
            {
                //if there are - then store the value of the key
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                //this simple loop clears the buffer so the game runs smoother
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                //check to see if the pressed key is the left arrow
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    //check to see if the new possition is possible. this ensures that after the movement to the left
                    //the dwarf will alwais be inside the playfield, so no errors will be thrown
                    if (player.X - 1 >= 0)
                    {
                        //Clear the old possition of the dwarf
                        ClearDwarf(player);
                        //Modify the X coordinate
                        player.X--;
                        //Draw the dwarf
                        PrintOnCoords(player.X, player.Y, player.form, player.color);
                    }
                }
                if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    //this ensures that after the movement the right most symbol of the dwarf will be inside the 
                    //playfield and will not hide inside the score board;
                    if (player.X + 1 - player.form.Length <= playFieldWidth)
                    {
                        //Clear the old possition
                        ClearDwarf(player);
                        //Modify the X Coordinate
                        player.X++;
                        //Draw the dwarf
                        PrintOnCoords(player.X, player.Y, player.form, player.color);
                    }
                }
            }

            //Draws the vertical line between the playfield and the scoreboard
            DrawFrame(playFieldWidth);

            //Draws each valid rock from the list
            foreach (Rock theRock in Rocks)
            {
                PrintOnCoords(theRock.X, theRock.Y, theRock.form, theRock.color);
            }

            //prints the scoreboard
            PrintOnCoords(playFieldWidth + 2, 10, "Lives: " + player.lives, ConsoleColor.White);
            PrintOnCoords(playFieldWidth + 2, 11, "Score: " + score, ConsoleColor.White);

            //Slows down the while loop so the game will be possible to play. It is necessary to have such pause, 
            //because if it is not pressent the game will run as fast as the processor permits it
            //which could be hundreds or thousands moves per second
            Thread.Sleep(150);
        }  //<--- this is the end of teh main while loop. When the player runs out of lives and the isAlive variable equals to "false"
           //the loop will end and the game over sequence will be displayed.
        //The game over sequence
        Console.Clear();
        PrintOnCoords(15, 10, "Game over!", ConsoleColor.Red);
        //Display the reached score
        PrintOnCoords(12, 11, "Your score: " + score, ConsoleColor.White);
        //Set the cursor position for the "Press any key to continue..." so it will be in the middle of the game screen
        Console.SetCursorPosition(7, 12);
        //exit the game
        return;
    }
}
