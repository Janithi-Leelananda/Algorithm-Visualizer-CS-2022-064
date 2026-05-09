using AlgorithmVisualizer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms
{
    public class BFSPathfinder
    {
        private Queue<Node> queue;
        private Node targetNode;
        private Node[,] grid;
        private bool isSearching;

        private readonly int[] dx = { 0, 0, 1, -1 };
        private readonly int[] dy = { 1, -1, 0, 0 };

        public void InitializeSearch(Node[,] grid, Node start, Node end)
        {
            this.grid = grid;
            this.targetNode = end;
            this.queue = new Queue<Node>();

            this.queue.Enqueue(start);
            start.IsVisited = true;
            isSearching = true;
        }

        public bool Step()
        {
            if (queue == null || queue.Count == 0)
            {
                isSearching = false;
                return false;
            }

            Node current = queue.Dequeue();

            if (current == targetNode)
            {
                isSearching = false;
                return true;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = current.X + dx[i];
                int ny = current.Y + dy[i];

                if (nx >= 0 && nx < grid.GetLength(0) && ny >= 0 && ny < grid.GetLength(1))
                {
                    Node neighbor = grid[nx, ny];
                    if (!neighbor.IsVisited && !neighbor.IsWall)
                    {
                        neighbor.IsVisited = true;
                        neighbor.Parent = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return false;
        }

        public bool IsSearching => isSearching;
    }
}