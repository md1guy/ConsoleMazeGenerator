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

            Cell[,] maze = new Cell[height, width];

            List<int> verticesInMaze = new List<int>();
            int numOfEdgesInMaze = 0;

            for (int i = 0; i < height; i ++)
            {
                for (int j = 0; j < width; j ++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        maze[i, j] = new Cell(1, i, j);
                    }
                    else
                    {
                        maze[i, j] = new Cell(0, i, j);
                    }
                }
            }

            /*verticesInMaze.Add(maze[0, 0]);

            for (int i = 0; i < height; i += 2)
            {
                for (int j = 0; j < width; j += 2)
                {
                    List<int> neighbours = GetNeighbours(i, j, maze, verticesInMaze);

                    if (verticesInMaze.Contains(maze[i, j]))
                    {
                        verticesInMaze.Add(neighbours.ElementAt(random.Next(neighbours.Count)));
                    }
                }
            }*/

            for (int i = 0; i < height; i ++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (maze[i, j].type == 0) Console.Write("█");
                    else Console.Write("░");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }

    class Cell
    {
        public int type = 0; // 0 for empties, 1 for nodes and 2 for edges
        public int i;
        public int j;

        List<int> verticesInMaze = new List<int>();

        public Cell(int type, int j, int i)
        {
            this.type = type;
            this.i = i;
            this.j = j;
        }

        List<int> GetNeighbours(int[,] maze)
        {
            List<int> neighbours = new List<int>();

            if (i > 1 && !verticesInMaze.Contains(maze[i - 2, j]))
            {
                neighbours.Add(maze[i - 2, j]);
            }

            if (i < maze.GetLength(0) - 1 && !verticesInMaze.Contains(maze[i + 2, j]))
            {
                neighbours.Add(maze[i + 2, j]);
            }

            if (j > 1 && !verticesInMaze.Contains(maze[i, j] - 2))
            {
                neighbours.Add(maze[i, j - 2]);
            }

            if (j < maze.GetLength(1) - 1 && !verticesInMaze.Contains(maze[i, j + 2]))
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

