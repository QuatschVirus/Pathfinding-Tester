using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Pathfinding.GBFS
{
    public class GBFS : Algorithm
    {
        private SortedSet<Node> openSet = new SortedSet<Node>(new NodeComparer());

        public override string Name => "Greddy";

        [SerializeField] private List<Node> path = new();
        private List<Node> avoid = new();

        Node startNode;
        Node goalNode;

        public override bool ComputePath()
        {
            startNode = Node.Morph(helper.origin);
            goalNode = Node.Morph(helper.target);

            openSet.Clear();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Min;
                openSet.Remove(currentNode);

                if (currentNode == goalNode)
                {
                    ConstructPath(currentNode);
                    return true; // Path found
                }

                foreach (Node neighbor in GetNeighbors(currentNode))
                {
                    if (!openSet.Contains(neighbor) && !neighbor.Blocked)
                    {
                        neighbor.HeuristicValue = CalculateHeuristic(neighbor.Position, goalNode.Position);
                        neighbor.Parent = currentNode;
                        openSet.Add(neighbor);
                    }
                }
            }

            // Clear the path if no path is found
            path.Clear();
            return false;
        }

        private void ConstructPath(Node node)
        {
            path.Clear();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent as Node;
            }
            path.Reverse();

            // Update your path helper here with the newly constructed path
        }

        private IEnumerable<Node> GetNeighbors(Node node)
        {
            // This returns all nodes from the 'connected' list that aren't blocked and not in the 'avoid' list.
            // Ensure that 'connected' is properly initialized and populated for each node.
            return node.connected.Where(neighbor => !neighbor.Blocked && !avoid.Contains(neighbor));
        }

        private float CalculateHeuristic(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }
    }

    public class NodeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return x.HeuristicValue.CompareTo(y.HeuristicValue);
        }
    }
}