using System;
using System.Collections.Generic;
using System.Drawing;

namespace WordSearch
{
    class Program
    {
        // Rows and columns in given grid
        static int ROWS, COLS;
        // For searching in all 8 direction
        static int[] x = { -1, -1, -1, 0, 0, 1, 1, 1 };
        static int[] y = { -1, 0, 1, -1, 1, -1, 0, 1 };
        //List for found words
        static List<string> foundWords = new List<string>();
        static void Main(string[] args)
        {
            //Reading in 2d array
            int Row2d,Col2d;
            Console.WriteLine("\n Enter the row size of 2d array?");
            Row2d = int.Parse(Console.ReadLine());
            Console.WriteLine("\n Enter the col size of 2d array?");
            Col2d = int.Parse(Console.ReadLine());
            Console.Write("Input elements in the matrix :\n");
            char[,] grid = new char[Row2d, Col2d];

            for (int i = 0; i < Row2d; i++)
            {
                for (int j = 0; j < Col2d; j++)
                {
                    Console.Write("\nelement - [{0},{1}] : ", i, j);
                    grid[i, j] = Console.ReadKey().KeyChar;
                }
            }

            
            //example grid for testing
            char[,] exgrid = new char[5, 5] { { 'r', 'i' ,'o','t','f'}, {'e', 'l','c','u','p' },
                                        { 'p', 'r','a','l','u' } ,{'l','s','e','s','o' },{'w','b','e','a','d' } };

            PrintArray(grid);

            ReadFile(grid);
        }

        public static void PrintArray(char[,] grid)
        {
            int rowLength = grid.GetLength(0);
            int colLength = grid.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("\n{0} ", grid[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        public static void ReadFile(char[,] grid)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"../../../words.txt");
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                if(line.Length != 0)
                {
                    SearchWord(grid, line);
                }
                counter++;
            }

            file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);
            Console.WriteLine("Found Words are :");
            Console.WriteLine(String.Join(", ", foundWords));
            // Suspend the screen.  
            System.Console.ReadLine();
        }

        public static void SearchWord(char[,] grid, string word)
        {
            ROWS = grid.GetLength(0);//Gets the first dimension size
            COLS = grid.GetLength(1);//Gets the second dimension size
            //Console.WriteLine("Rows:" + ROWS +"cols:"+ COLS);
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    //Console.WriteLine($"FindWord:{row}{col}");
                    if(FindWord(grid, row, col, word))
                    {
                        Console.WriteLine("pattern found at " + row + ", " + col +" for "+word);
                        //add found words to list
                        foundWords.Add(word);
                    }
                }
            }
        }

        static bool FindWord(char[,] grid, int row,int col, String word)
        {

            int lettersFound = 1;
            bool reusedLetter = false;
            Point[] points = new Point[word.Length];
            // Look for first character
            if (grid[row, col] != word[0])
            {
                return false;
            }
            //If found,Get word length and search all directions
            int wordLength = word.Length;
           // Console.WriteLine("word len:" + wordLength);
            //store co-ordinates of found letters
            points[0] = new Point(row, col);
            //Console.WriteLine("points:" + points[0]);
           

            for (int dir = 0; dir < 8; dir++)
            {
                //get row of search direction
                int rd = row + x[dir];
                //get col of search direction
                int cd = col + y[dir];
                //Console.WriteLine($"{rd} {cd}");
                //Find remaining letters to match
                while (lettersFound < wordLength)
                {
                    //check if out of bounds of grid
                    if (rd >= ROWS || rd < 0 || cd >= COLS || cd < 0)
                    {
                        //Console.WriteLine("Out of Bounds");
                        break;
                    }

                    // If letter doesnt match the current grid position,break
                    if (grid[rd, cd] != word[lettersFound])
                    {
                        break;
                    }
                    for(int p = 0; p < lettersFound; p++)
                    {   //previous letter has been found
                        if(points[p].X == rd && points[p].Y == cd)
                        {
                            reusedLetter = true;
                            break;
                        }
                    }
                    if(reusedLetter == false)
                    {
                        //Console.WriteLine("Matched Letter:" + grid[rd, cd]);
                        //Console.WriteLine("Matched:" + rd + "and:" + cd + "for:" + word[lettersFound]);
                        points[lettersFound] = new Point(rd, cd); //Save the points of found letters,so cant be reused
                        //Console.WriteLine("points:" + points[lettersFound] + "at:" + lettersFound);

                        row = rd;
                        col = cd;

                        dir = -1;
                        lettersFound++;
                        break;
                    }
                    reusedLetter = false;
                    break;


                }

                // If all letters matched, then value of lettersFound must be equal to length of word
                if (lettersFound == wordLength) 
                {
                    //Console.WriteLine("Word Found!");
                    return true;
                }
            }
            return false;
        }
    }
}
