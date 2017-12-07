using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMazeGenerator
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            int height = 7;
            int width = 7;

            int[,] maze = new int[height, width]; // 0 for empties, 1 for nodes and 2 for edges

            List<int> verticesInMaze = new List<int>();

            maze.Fill(0);

            for (int i = 0; i < height; i += 2)
            {
                for (int j = 0; j < width; j += 2)
                {
                    maze[i, j] = 1;
                }
            }

            verticesInMaze.Add(maze[0, 0]);

            for (int i = 0; i < height; i += 2)
            {
                for (int j = 0; j < width; j += 2)
                {
                    List<int> neighbours = GetNeighbours(i, j, maze);

                    if (verticesInMaze.Contains(maze[i, j]) && !verticesInMaze.Contains(maze[i, j]))
                    {
                        
                    }
                }
            }

            for (int i = 0; i < height; i ++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (maze[i, j] == 0) Console.Write("█");
                    else Console.Write("░");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static List<int> GetNeighbours(int i, int j, int[,] maze)
        {
            List<int> neighbours = new List<int>();

            if(i > 1)
            {
                neighbours.Add(maze[i - 2, j]);
            }

            if (i < maze.GetLength(0) - 1)
            {
                neighbours.Add(maze[i + 2, j]);
            }

            if (j > 1)
            {
                neighbours.Add(maze[i, j - 2]);
            }

            if (j < maze.GetLength(1) - 1)
            {
                neighbours.Add(maze[i, j + 2]);
            }

            return neighbours;
        }
    }

    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        public static void Fill<T>(this T[,] originalArray, T with)
        {
            for (int i = 0; i < originalArray.GetLength(0); i++)
            {
                for (int j = 0; j < originalArray.GetLength(1); j++)
                {
                    originalArray[i, j] = with;
                }
            }
        }
    }
}

