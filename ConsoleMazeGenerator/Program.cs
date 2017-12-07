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
            int height = 49;
            int width = 49;

            Cell[,] maze = new Cell[height, width];

            List<Cell> cellsInMaze = new List<Cell>();

            int numOfEdgesInMaze = 0;
            int numOfCells = (height - 1) / 2 + (width + 1) / 2;

            for (int i = 0; i < height; i ++)
            {
                for (int j = 0; j < width; j ++)
                {/*
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        maze[i, j] = new Cell(1, i, j);
                    }
                    else
                    {
                        maze[i, j] = new Cell(0, i, j);
                    }*/

                    maze[i, j] = new Cell(0, i, j);
                }
            }

            cellsInMaze.Add(maze[0, 0]);

            while (numOfEdgesInMaze < numOfCells)
            {
                for (int i = 0; i < height; i += 2)
                {
                    for (int j = 0; j < width; j += 2)
                    {
                        if (cellsInMaze.Contains(maze[i, j]))
                        {
                            List<Cell> neighbours = maze[i, j].GetNeighbours(maze, cellsInMaze);

                            if (neighbours.Count > 0)
                            {

                                Cell neighbour = neighbours.ElementAt(random.Next(neighbours.Count));
                                Cell middleNeighbour = maze[neighbour.i - (neighbour.i - i), neighbour.j - (neighbour.j - j)];

                                neighbour.type = 1;
                                middleNeighbour.type = 1;

                                cellsInMaze.Add(neighbour);
                                cellsInMaze.Add(middleNeighbour);

                                maze[neighbour.i, neighbour.j].type = 1;
                                maze[middleNeighbour.i, middleNeighbour.j].type = 1;

                                numOfEdgesInMaze++;
                            }
                        }
                    }
                }
            }

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

        List<Cell> neighbours = new List<Cell>();

        public Cell(int type, int j, int i)
        {
            this.type = type;
            this.i = i;
            this.j = j;
        }

        public List<Cell> GetNeighbours(Cell[,] maze, List<Cell> verticesInMaze)
        {
            int i = this.i;
            int j = this.j;

            List<Cell> neighbours = new List<Cell>();

            if (i > 1 && !verticesInMaze.Contains(maze[i - 2, j]))
            {
                neighbours.Add(maze[i - 2, j]);
            }

            if (i < maze.GetLength(0) - 1 && !verticesInMaze.Contains(maze[i + 2, j]))
            {
                neighbours.Add(maze[i + 2, j]);
            }

            if (j > 1 && !verticesInMaze.Contains(maze[i, j - 2]))
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

