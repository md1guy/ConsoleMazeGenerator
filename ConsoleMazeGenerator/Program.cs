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
            int height = 50; //if too big and live visualization is enabled, will be throwed exception TODO: try-catch for it and try to fix that
            int width = 50;

            bool visualize = false;

            Console.WindowWidth = width * 2 + 10;

            Maze maze = new Maze(height, width, visualize);

            bool[,] convertedMaze = maze.convertedGraph;


            if (!visualize)
            {
                for (int i = 0; i < convertedMaze.GetLength(0); i++)
                {
                    for (int j = 0; j < convertedMaze.GetLength(1); j++)
                    {
                        if (convertedMaze[i, j])
                        {
                            Console.Write("██");
                        }
                        else
                        {
                            Console.Write("░░");
                        }
                    }
                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 0);
            Console.Write("██");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(width * 2, height);
            Console.Write("██");

            Console.ReadLine();
        }
    }

    class Maze
    {
        static Random random = new Random();

        bool visualize;

        int height;
        int width;

        HashSet<Edge> graph = new HashSet<Edge>();
        Node[,] nodes;

        public bool[,] convertedGraph;

        public Maze(int height, int width, bool visualize)
        {
            this.height = height;
            this.width = width;
            this.visualize = visualize;

            CreateNodes();
            AddEdges();

            graph = MST(graph);
            convertedGraph = ConvertToBoolean(graph);
        }

        //fill nodes array
        void CreateNodes()
        {
            int width = (this.width % 2 == 0) ? this.width + 1 : this.width;
            int height = (this.height % 2 == 0) ? this.height + 1 : this.height;
            int nodesI = (height + 1) / 2;
            int nodesJ = (width + 1) / 2;

            nodes = new Node[nodesI, nodesJ];

            for (int k = 0, i = 0; i < nodesI; i++)
            {
                for (int z = 0, j = 0; j < nodesJ; j++)
                {
                    nodes[i, j] = new Node(k, z, visualize);
                    z += 2;
                }

                k += 2;
            }
        }

        //add edges to a maze
        void AddEdges()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (i < nodes.GetLength(0) - 1)
                    {
                        Edge edge = new Edge(nodes[i, j], nodes[i + 1, j]);

                        graph.Add(edge);
                    }

                    if (j < nodes.GetLength(1) - 1)
                    {
                        Edge edge = new Edge(nodes[i, j], nodes[i, j + 1]);

                        graph.Add(edge);
                    }
                }
            }
        }

        //get MST
        HashSet<Edge> MST(HashSet<Edge> edges)
        {
            HashSet<Edge> edgesInMST = new HashSet<Edge>();
            HashSet<Edge> possibleEdges = new HashSet<Edge>();
            HashSet<Node> nodesInMST = new HashSet<Node>();

            nodesInMST.Add(nodes[random.Next(nodes.GetLength(0)), random.Next(nodes.GetLength(1))]);

            while (edgesInMST.Count != nodes.Length - 1)
            {
                possibleEdges = FindEdges(nodesInMST);
                List<Edge> possibleEdgesToMST = new List<Edge>();

                foreach (Edge edge in possibleEdges)
                {
                    int connections = 0;

                    foreach (Node node in nodesInMST)
                    {
                        if (edge.nodeA == node || edge.nodeB == node) connections++;
                    }

                    if (connections != 1)
                    {
                        continue;
                    }

                    possibleEdgesToMST.Add(edge);
                }

                if (possibleEdgesToMST.Count > 0)
                {
                    Edge edgeToMST = possibleEdgesToMST.ElementAt(random.Next(possibleEdgesToMST.Count));

                    edgesInMST.Add(edgeToMST);
                    nodesInMST.Add(edgeToMST.nodeA);
                    nodesInMST.Add(edgeToMST.nodeB);

                    if (visualize)
                    {
                        if (edges.Contains(edgeToMST))
                        {
                            Console.SetCursorPosition(edgeToMST.j * 2, edgeToMST.i);
                            Console.WriteLine("██");
                        }

                        else
                        {
                            Console.SetCursorPosition(edgeToMST.nodeA.j * 2, edgeToMST.nodeB.i);
                            Console.WriteLine("░░");
                        }
                    }
                }
            }
        

            return edgesInMST;
        }

        //get available edges for a node
        HashSet<Edge> FindEdges(HashSet<Node> nodes)
        {
            HashSet<Edge> edges = new HashSet<Edge>();

            foreach (Node node in nodes)
            {
                foreach (Edge edge in this.graph)
                {
                    if (edge.nodeA == node || edge.nodeB == node)
                    {
                        edges.Add(edge);
                    }
                }
            }

            return edges;
        }

        //create boolean maze
        bool[,] ConvertToBoolean(HashSet<Edge> edges)
        {
            int width = (this.width % 2 == 0) ? this.width + 1 : this.width;
            int height = (this.height % 2 == 0) ? this.height + 1 : this.height;

            convertedGraph = new bool[height, width];
            convertedGraph.Fill(false);

            foreach (Edge edge in graph)
            {
                convertedGraph[edge.nodeA.i, edge.nodeA.j] = true;
                convertedGraph[edge.i, edge.j] = true;
                convertedGraph[edge.nodeB.i, edge.nodeB.j] = true;
            }

            return convertedGraph;
        }

        class Node
        {
            public int i;
            public int j;

            public Node(int i, int j, bool visualize)
            {
                this.i = i;
                this.j = j;

                if (visualize)
                {
                    Console.SetCursorPosition(j * 2, i);
                    Console.WriteLine("██");
                }
            }
        }

        class Edge
        {
            public Node nodeA;
            public Node nodeB;

            public int i;
            public int j;

            public Edge(Node nodeA, Node nodeB)
            {
                this.nodeA = nodeA;
                this.nodeB = nodeB;

                GetCoords();
            }

            void GetCoords()
            {
                this.i = nodeA.i + (nodeB.i - nodeA.i) / 2;
                this.j = nodeA.j + (nodeB.j - nodeA.j) / 2;
            }
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