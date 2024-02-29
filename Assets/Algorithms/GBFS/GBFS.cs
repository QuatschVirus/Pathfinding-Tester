using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.GBFS
{
    public class GBFS : Algorithm
    {
        private SortedSet<Node> openSet = new SortedSet<Node>();

        public override string Name => "Greddy";

        public override bool ComputePath()
        {
            Node startNode = new Node(helper.origin.Position);
            Node goalNode = new Node(helper.target.Position);

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

            PathfinderHelper.Instance.path.Clear(); // Path not found
            return false;
        }

        private void ConstructPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Add(node); // Convert back to grid node
                node = node.Parent as Node;
            }
            path.Reverse();

            PathfinderHelper.Instance.path = path;
        }

        private IEnumerable<Node> GetNeighbors(Node node)
        {
            // Implement your logic to return neighbor nodes, considering your grid and movement rules
            throw new System.NotImplementedException();
        }

        private float CalculateHeuristic(Vector2 a, Vector2 b)
        {
            // Using Manhattan distance as heuristic
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public class Node : Pathfinding.Node, IComparable<Node>
        {
            public float HeuristicValue { get; set; }
            public Node Parent { get; set; }

            public Node(Vector3 position)
            {
                transform.position = position;
            }

            public int CompareTo(Node other)
            {
                return HeuristicValue.CompareTo(other.HeuristicValue);
            }
        }
    }
}
