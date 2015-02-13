using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    //Create a structure that will set the coordinates of each item of the snake and the snake food
    struct Position
    {
        public int col;
        public int row;
        public Position(int col, int row)
        {
            this.col = col;
            this.row = row;
        }
    }

    //some basic console parameters
    static void SetConsoleParameters()
    {
        Console.Title = "Snake";
        Console.WindowWidth = 60;
        Console.WindowHeight = 25;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;
    }

    //little helper method to print out strings on given position
    static void PrintOnCoords(int col, int row, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = color;
        Console.Write(text);
    }
    //little method to draw the divider between the playfield and the score board
    static void DrawGrid(int playfield)
    {
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            PrintOnCoords(playfield+1, i, "|", ConsoleColor.Yellow);
        }
    }

    //the game over sequence method
    static void GameOver(int score)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Game Over!");
        Console.WriteLine("Your score is: " + score);
    }

    //just a random generator
    static Random randGen = new Random();
    
    //the main game function
    static void Main()
    {
        //set the console parameters
        SetConsoleParameters();
        //set the playfield to be smaller than the console window with 20 columns, so there will be space for the scoreboard
        int playField = Console.WindowWidth - 20;
        
        //draw the border
        DrawGrid(playField);

        //initialize the score
        int score = 0;

        //initialize the sleeptimer (essentially the speed of the game)
        double sleeptime = 100;

        //some helper variables for the directions
        int right = 0;
        int left = 1;
        int up = 2;
        int down = 3;

        //the starting direction
        int direction = right;

        //little array with the possible movement directions - each direction adds or substracts a coordinate from the snake head
        //this way the snake moves or changes directions
        Position[] Directions = new Position[] 
        {
            new Position(1,0),  // move to the right
            new Position(-1,0), // move to the left
            new Position(0,-1), // move up
            new Position(0,1), // move down
        };

        //Create a queue, which will hold all of the snake elements
        Queue<Position> Snake = new Queue<Position>();

        //initialize the first 6 elements, all ligned up on the first row
        for (int i = 0; i < 6; i++)
        {
            Snake.Enqueue(new Position(i, 0));
        }

        //draw the first 6 elements
        foreach(Position snakeElement in Snake)
        {
            PrintOnCoords(snakeElement.col, snakeElement.row, "*", ConsoleColor.Green);
        }

        //Create a food element on a random position on the playfield and then draw it
        Position Food = new Position(randGen.Next(0, playField), randGen.Next(0, Console.WindowHeight));
        PrintOnCoords(Food.col, Food.row, "@", ConsoleColor.Red);

        //the main game loop, which will run until something forses it to stop
        while(true)
        {
            //check to see if a key is pressed
            if(Console.KeyAvailable)
            {
                //assign this key to a variable
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                
                //set the direction of movement, based on the keyboard input
                switch(pressedKey.Key)
                {
                        //the if statement ensures that the snake will not turn on itself
                    case ConsoleKey.RightArrow: if (direction != left) direction = right; break;
                    case ConsoleKey.LeftArrow: if (direction != right) direction = left; break;
                    case ConsoleKey.UpArrow: if (direction != down) direction = up; break;
                    case ConsoleKey.DownArrow: if (direction != up) direction = down; break;
                }
            }

            //assign the last added element in the que as the old head, so it will be easy to be manipulated
            Position snakeHead = Snake.Last();

            //get the direction in a variable
            Position nextDirection = Directions[direction];

            //Create a new snake element, which represents the head - the first element of the movement
            Position newSnakeHead = new Position(snakeHead.col + nextDirection.col, snakeHead.row + nextDirection.row);

            //if the head element is out of bounds, this will reset it on the opposite side of the playfield
            if (newSnakeHead.col < 0) newSnakeHead.col = playField;
            if (newSnakeHead.col > playField) newSnakeHead.col = 0;
            if (newSnakeHead.row >= Console.WindowHeight) newSnakeHead.row = 0;
            if (newSnakeHead.row < 0) newSnakeHead.row = Console.WindowHeight - 1;

            //check to see if the existing snake contains the new head element, because if the new head element is
            //in the que, this means that the snake is overlaping, which means - game over
            if (Snake.Contains(newSnakeHead))
            {
                GameOver(score);
                return;
            }

            //if the new snake head position is valid - add it to the que
            Snake.Enqueue(newSnakeHead);

            //check to see if the snake is eating or not
            if (newSnakeHead.col == Food.col && newSnakeHead.row == Food.row)
            {
                //this will try to create a new food element until the food is not 
                //over the existing snake
                do
                {
                    Food = new Position(randGen.Next(0, playField), randGen.Next(0, Console.WindowHeight));
                } while (Snake.Contains(Food));
                //print the new food
                PrintOnCoords(Food.col, Food.row, "@", ConsoleColor.Red);
                //update the score
                score++;
                //speed up the game by reducing the sleep time
                sleeptime -= 0.1;
            }
                //the snake has not reached food
            else
            {   //remove the last element and save it in a variable
                Position lastHead = Snake.Dequeue();
                //print empty space on it's place to avoid console.clear
                PrintOnCoords(lastHead.col, lastHead.row, " ");
            }
            //redraw the snake
            foreach(Position element in Snake)
            {
                PrintOnCoords(element.col, element.row, "*", ConsoleColor.Green);
            }

            //print the score
            PrintOnCoords(playField + 3, 10, "Score: " + score, ConsoleColor.DarkCyan);
            //sleep
            Thread.Sleep((int)sleeptime);
        }
    }
}
