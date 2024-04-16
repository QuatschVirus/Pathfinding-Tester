using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;

// Define the namespace for the Greedy Best First Search algorithm within the pathfinding context
namespace Pathfinding.GBFS
{
    // GBFS class inherits from the Algorithm base class
    public class GBFS : Algorithm
    {
        // A sorted set to hold the nodes to be explored, sorted based on a custom comparator
        private SortedSet<Node> queue = new SortedSet<Node>(new NodeComparer());

        // Overriding the Name property from the base class to specify the algorithm's name
        public override string Name => "Greedy";

        // A list to hold the final path from start to goal node
        [SerializeField] private List<Node> path = new();

        // A list of nodes to be avoided during pathfinding
        private List<Node> avoid = new();

        // Starting node of the path
        Node startNode;

        // Goal node of the path
        Node goalNode;

        // The main function that computes the path from start to goal
        public override bool ComputePath()
        {
            // Initialize start and goal nodes based on the helper's origin and target
            startNode = Node.Morph(helper.origin);
            goalNode = Node.Morph(helper.target);

            List<Node> visited = new List<Node>();

            // Clear the open set at the beginning of path computation
            queue.Clear();
            // Add the start node to the open set
            startNode.HeuristicValue = CalculateHeuristic(startNode.Position, goalNode.Position);

            queue.Add(startNode);
            visited.Add(startNode);


            // Continue exploring nodes until there are no nodes left in the open set
            while (queue.Count > 0)
            {
                // Get the node with the lowest heuristic value from the open set
                Node currentNode = queue.Min;
                // Remove the current node from the open set as it's now being processed
                queue.Remove(currentNode);

                // Check if the current node is the goal node
                if (currentNode == goalNode)
                {
                    // Construct the path from start to goal
                    ConstructPath(visited);
                    Debug.Log("Path found 1");
                    helper.path = path.Cast<Pathfinding.Node>().ToList();
                    return true; // Path has been found
                }

                // Explore all unblocked neighbors of the current node
                foreach (Node neighbor in GetNeighbors(currentNode))
                {
                    // Add the neighbor to the open set if it's not already present and not blocked
                    if (!visited.Contains(neighbor) && !neighbor.Blocked)
                    {

               

                            // Calculate the heuristic value for the neighbor
                            neighbor.HeuristicValue = CalculateHeuristic(neighbor.Position, goalNode.Position);
                            // Set the current node as the parent of the neighbor
                            neighbor.Parent = currentNode;
                            // Add the neighbor to the open set
                            queue.Add(neighbor);
                            visited.Add(neighbor);
                        
                    }
                }
            }

            // If no path is found, clear the path list
            path.Clear();
            return false; // Path not found
        }

        // Constructs the path from the goal node back to the start node
        private void ConstructPath(List<Node> visited)
        {
            /*            // Clear the existing path
                        path.Clear();
                        // Traverse from the goal node up through the parent nodes
                        while (node != null)
                        {
                            // Add the node to the path
                            path.Add(node);
                            // Move to the parent node
                            node = node.Parent as Node;
                        }
                        // The path is constructed in reverse, so it needs to be reversed to start from the start node
                        path.Reverse();
                        // Update path helper here with the newly constructed path*/

            // https://github.com/MagmaArcade/Tree-Search-Algorithms/blob/main/Robot%20Navigation/Search.cs#L417
            path.Clear();
            List<Node> constructed = new List<Node>();
            visited.Reverse();

            foreach (Node node in visited)
            {
                // if child is goal, add to path
                if (node.Position.x == goalNode.Position.x && node.Position.y == goalNode.Position.y)
                {
                    constructed.Add(node);
                }

                if(constructed.Count > 0)
                {
                    Node last_added_node = constructed[constructed.Count - 1];
                    if (last_added_node.Parent == node)
                    {
                        constructed.Add(node);
                    }
                }

            }

            Debug.Log(constructed.Count);

            constructed.Reverse();
            path = constructed;

            Debug.Log(path);



            PathfinderHelper.Instance.path = path.Cast<Pathfinding.Node>().ToList();
        }

        // Retrieves all valid neighbors of a node
        private IEnumerable<Node> GetNeighbors(Node node)
        {

            // shoot towards all nodes
            // find target or blocking object (square?)
            // if target, target is neighbor
            // if object in the way, not a neighbor
            //list of all the nodes


            //Return neighbors that are not blocked and not in the avoid list
            IEnumerable<Node> result = node.connected.Where(neighbor => !neighbor.Blocked && !avoid.Contains(neighbor));
            return result;
        }

        // Calculate the heuristic value as the straight-line distance between two points
        private float CalculateHeuristic(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }
    }

    // Custom comparer for nodes based on their heuristic values
    public class NodeComparer : IComparer<Node>
    {
        // Compares two nodes based on their heuristic values
        public int Compare(Node x, Node y)
        {
            return x.HeuristicValue.CompareTo(y.HeuristicValue);
        }
    }
}