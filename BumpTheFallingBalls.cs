using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;


class Program
{
    static void PlayTune()
    {
        Console.Beep(658, 125);
        Console.Beep(1320, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(1320, 125);
        Console.Beep(1188, 125);
        Console.Beep(1056, 250);
        Console.Beep(990, 250);
        Console.Beep(880, 500);
        Console.Beep(880, 250);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 750);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        Thread.Sleep(250);
        Console.Beep(1188, 500);
        Console.Beep(1408, 250);
        Console.Beep(1760, 500);
        Console.Beep(1584, 250);
        Console.Beep(1408, 250);
        Console.Beep(1320, 750);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        Thread.Sleep(500);
        Console.Beep(1320, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 250);
        Console.Beep(1320, 125);
        Console.Beep(1188, 125);
        Console.Beep(1056, 250);
        Console.Beep(990, 250);
        Console.Beep(880, 500);
        Console.Beep(880, 250);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 750);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        Thread.Sleep(250);
        Console.Beep(1188, 500);
        Console.Beep(1408, 250);
        Console.Beep(1760, 500);
        Console.Beep(1584, 250);
        Console.Beep(1408, 250);
        Console.Beep(1320, 750);
        Console.Beep(1056, 250);
        Console.Beep(1320, 500);
        Console.Beep(1188, 250);
        Console.Beep(1056, 250);
        Console.Beep(990, 500);
        Console.Beep(990, 250);
        Console.Beep(1056, 250);
        Console.Beep(1188, 500);
        Console.Beep(1320, 500);
        Console.Beep(1056, 500);
        Console.Beep(880, 500);
        Console.Beep(880, 500);
        Thread.Sleep(500);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 1000);
        Console.Beep(440, 1000);
        Console.Beep(419, 1000);
        Console.Beep(495, 1000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 500);
        Console.Beep(660, 500);
        Console.Beep(880, 1000);
        Console.Beep(838, 2000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 1000);
        Console.Beep(440, 1000);
        Console.Beep(419, 1000);
        Console.Beep(495, 1000);
        Console.Beep(660, 1000);
        Console.Beep(528, 1000);
        Console.Beep(594, 1000);
        Console.Beep(495, 1000);
        Console.Beep(528, 500);
        Console.Beep(660, 500);
        Console.Beep(880, 1000);
        Console.Beep(838, 2000);
    }

    struct Ball
    {
        public int col;
        public int row;
        public ConsoleColor color;
        public string ballChar;
    }

    struct Paddle
    {
        public int col;
        public int row;
        public string form;
        public char formChar;
        public int size;
        public ConsoleColor color;
    }

    static Random randGen = new Random();

    static void SetConsoleParameters()
    {
        Console.WindowHeight = 25;
        Console.WindowWidth = 45;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.Title = "Table tennis";
        Console.CursorVisible = false;
    }

    static Ball CreateBall(int size)
    {
        Ball firstBall = new Ball();
        firstBall.col = randGen.Next(0, size);
        firstBall.row = 0;
        firstBall.ballChar = "@";
        firstBall.color = ConsoleColor.Red;
        return firstBall;
    }

    static void PrintOnPos(int col, int row, string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    static void DrawFrame(int pos)
    {
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            PrintOnPos(pos, i, "|", ConsoleColor.Yellow);
        }
    }

    static void GameOver(int scorePoints)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Game Over!");
        Console.WriteLine("Points: "+scorePoints);
    }

    static void Main()
    {
        SetConsoleParameters();
        //Task.Run(() => { PlayTune(); });
        int playFieldWidth = Console.WindowWidth - 20;
        int lives = 5;
        int score = 0;
        Paddle player = new Paddle();
        player.formChar = '=';
        player.size = 5;
        player.form = new string(player.formChar, player.size);
        player.row = Console.WindowHeight - 1;
        player.col = playFieldWidth / 2 - player.form.Length / 2;
        player.color = ConsoleColor.Green;

        List<Ball> fallingBalls = new List<Ball>();
        List<Ball> flyingBalls = new List<Ball>();
        int pause = 10;
        Ball firstBall = CreateBall(playFieldWidth);
        fallingBalls.Add(firstBall);
        while (true)
        {
            if(pause == 0)
            { 
                firstBall = CreateBall(playFieldWidth);
                fallingBalls.Add(firstBall);
                pause = 11;
            }
            pause--;
            List<Ball> tempList = new List<Ball>();

            for (int i = 0; i < fallingBalls.Count; i++)
            {
                Ball oldBall = fallingBalls[i];
                Ball newBall = new Ball();
                newBall.col = oldBall.col;
                newBall.color = oldBall.color;
                newBall.ballChar = oldBall.ballChar;
                newBall.row = oldBall.row + 1;

                if (newBall.row < Console.WindowHeight)
                {
                    tempList.Add(newBall);
                }

                if (newBall.row == Console.WindowHeight -1 && newBall.col>=player.col && newBall.col < player.col+player.size)
                {
                    score++;
                    flyingBalls.Add(newBall);
                }
                else if (newBall.row == Console.WindowHeight -1 && (newBall.col <= player.col || newBall.col >= player.col+player.size))
                {
                    lives--;
                    if (lives == 0)
                    {
                        GameOver(score);
                        return;
                    }
                }
            }

            List<Ball> tempflyingList = new List<Ball>();
            for (int i = 0; i < flyingBalls.Count; i++)
            {
                Ball oldBall = flyingBalls[i];
                Ball newBall = new Ball();
                newBall.col = oldBall.col;
                newBall.color = oldBall.color;
                newBall.ballChar = oldBall.ballChar;
                newBall.row = oldBall.row - 1;

                if (newBall.row > 0)
                {
                    tempflyingList.Add(newBall);
                }

                if (newBall.row == 0)
                {
                    score++;
                    flyingBalls.Remove(newBall);
                }
            }

            flyingBalls = tempflyingList;
            fallingBalls = tempList;
            Console.Clear();
            DrawFrame(playFieldWidth);
            PrintOnPos(player.col, player.row, player.form, player.color);
            PrintOnPos(playFieldWidth + 5, 10, "Score: " + score, ConsoleColor.Cyan);
            PrintOnPos(playFieldWidth + 5, 11, "Lives: " + lives, ConsoleColor.Cyan);
            foreach (Ball ball in fallingBalls)
            {
                PrintOnPos(ball.col, ball.row, ball.ballChar, ball.color);
            }
            foreach (Ball ball in flyingBalls)
            {
                PrintOnPos(ball.col, ball.row, ball.ballChar, ball.color);
            }
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (player.col - 1 >= 0)
                    {
                        player.col--;
                    }
                }
                if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (player.col + player.form.Length < playFieldWidth)
                    {
                        player.col++;
                    }
                }
            }
            Thread.Sleep(150);
        }
    }
}
