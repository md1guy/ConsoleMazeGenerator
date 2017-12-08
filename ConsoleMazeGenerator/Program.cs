﻿using System;
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
            int height = 25;
            int width = 25;

            Maze maze = new Maze(height, width);

            bool[,] convertedMaze = maze.convertedGraph;

            for (int i = 0; i < convertedMaze.GetLength(0); i++)
            {
                for (int j = 0; j < convertedMaze.GetLength(1); j++)
                {
                    if (convertedMaze[i, j])
                    {
                        Console.Write("█");
                    }
                    else
                    {
                        Console.Write("░");
                    }
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }

    class Maze
    {
        int height;
        int width;

        List<Edge> graph = new List<Edge>();
        Node[,] nodes;

        public bool[,] convertedGraph;

        public Maze(int height, int width)
        {
            this.height = height;
            this.width = width;

            //fill nodes array
            CreateNodes();

            //add edges to a maze
            AddEdges();

            //get MST
            graph = MST(graph);

            //create maze boolean
            convertedGraph = ConvertToBoolean(graph);


        }

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
                    nodes[i, j] = new Node(k, z);
                    z += 2;
                }

                k += 2;
            }
        }

        void AddEdges()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (i < nodes.GetLength(0) - 1)
                    {
                        Edge edge = new Edge(nodes[i, j], nodes[i + 1, j]);
                        edge.GetCoords();
                        graph.Add(edge);
                    }

                    if (j < nodes.GetLength(1) - 1)
                    {
                        Edge edge = new Edge(nodes[i, j], nodes[i, j + 1]);
                        edge.GetCoords();
                        graph.Add(edge);
                    }
                }
            }
        }

        List<Edge> MST(List<Edge> edges)
        {
            List<Edge> edgesInMST    = new List<Edge>();
            List<Edge> possibleEdges = new List<Edge>();
            List<Node> nodesInMST    = new List<Node>();

            nodesInMST.Add(nodes[0, 0]);

            while (edgesInMST.Count != nodes.Length - 1)
            {
                possibleEdges = FindEdges(nodesInMST);

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

                    edgesInMST.Add(edge);
                    nodesInMST.Add(edge.nodeA);
                    nodesInMST.Add(edge.nodeB);

                    break;
                }
            }

            return edgesInMST;
        }

        List<Edge> FindEdges(List<Node> nodes)
        {
            List<Edge> edges = new List<Edge>();

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

        bool[,] ConvertToBoolean(List<Edge> edges)
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

            public Node(int i, int j)
            {
                this.i = i;
                this.j = j;
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
            }

            public void GetCoords()
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

