using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Pathfinding.GBFS
{
    // Node class inheriting from a base Pathfinding.Node class.
    public class Node : Pathfinding.Node
    {
        // A list to hold connected nodes, establishing the graph structure.
        public List<Node> connected;

        // Heuristic value property used in Greedy Best-First Search (GBFS) for decision making.
        public float HeuristicValue { get; set; }

        // Parent node property used to trace back the path from the destination to the source.
        public Node Parent { get; set; }

        void Start()
        {

        }

        void Update()
        {

        }

        // Returns the closest connected node to the specified node, excluding any nodes in the banned list.
        public Node GetClosestConnected(Node node, Node[] banned)
        {
            Node closest = this; // Start with the current node as the closest.
            float closestDistance = DistanceToNode(node); // Calculate initial distance to the target node.

            // Iterate through each connected node to find the closest.
            foreach (Node child in connected)
            {
                // Check if child node is closer, not banned, and not blocked.
                if (child.DistanceToNode(node) < closestDistance && !banned.Contains(node) && !node.Blocked)
                {
                    closest = child; // Update closest node.
                }
            }

            return closest; // Return the closest node found.
        }


        /*public Node[] GetConnected(Node currentNode)
        {
            List<Node> neighbors = new List<Node>(); // List to hold all neighbors

            foreach (Node targetNode in allNodes)
            {
                // Skip the current node to avoid self-targeting
                if (targetNode == currentNode) continue;

                // Calculate the direction from the current node to the target node
                Vector2 direction = targetNode.transform.position - currentNode.transform.position;

                // Send a raycast from the current node to the target node
                RaycastHit2D hit = Physics2D.Raycast(currentNode.transform.position, direction.normalized, direction.magnitude);

                // Check if the raycast hit something
                if (hit.collider != null)
                {
                    Node hitNode = hit.collider.GetComponent<Node>();
                    // If the raycast directly hits the target node and it's not blocked or in the avoid list, add it to neighbors
                    if (hitNode == targetNode && !targetNode.Blocked && !avoid.Contains(targetNode))
                    {
                        neighbors.Add(targetNode);
                    }
                }
            }

            return neighbors; // Return the list of neighbors
        }*/


        // Returns an array containing the current node and its closest connection to the specified node, excluding banned nodes.
        public Node[] GetClostestConnection(Node node, Node[] banned)
        {
            Node closest = GetClosestConnected(node, banned); // Find the closest connected node.

            // Return an array with the current node and the closest connected node, or just the current node if no closer connection is found.
            return closest != this ? new Node[] { this, closest } : new Node[] { this };
        }

        // Returns the closest nodes from a given list, applying a tiebreaker if necessary, and considering various constraints.
        public List<Node> GetClosestFromWithTiebreaker(List<Node> from, List<Node> deciders, float precision, Node[] banned, float maxDistance = float.MaxValue)
        {
            if (from.Count < 1) return null; // Return null if the list is empty.

            // Order nodes by descending distance to the current node.
            from.OrderByDescending(n => n.DistanceToNode(this));

            // Update maxDistance considering the precision threshold.
            maxDistance = Math.Min(from.First().DistanceToNode(this) + precision, maxDistance);

            // Remove nodes that are banned, blocked, or beyond the maxDistance.
            from.RemoveAll(n => banned.Contains(n) || n.Blocked || n.DistanceToNode(this) > maxDistance);

            // Debugging: Log the names of nodes found closest.
            List<string> names = new();
            foreach (Node n in from)
            {
                names.Add(n.name);
            }
            Debug.Log("Found closest: " + string.Join(", ", names));

            // Determine the result based on the count of the 'from' list.
            switch (from.Count)
            {
                case 0: return null; // No nodes left, return null.
                case 1: return from.Take(1).ToList(); // One node left, return it.
                default:
                    {
                        // Multiple nodes left, use deciders for tiebreaking or return the list if no deciders left.
                        return deciders.Count > 0 ?
                            deciders.First().GetClosestFromWithTiebreaker(from, deciders.Skip(1).ToList(), precision, banned) :
                            from;
                    }
            }
        }

        // Returns the closest node from a given array, excluding banned nodes and considering a maximum distance.
        public Node GetClosestFrom(Node[] from, Node[] banned, float maxDistance = float.MaxValue)
        {
            Node closest = null; // Start with no closest node.
            float closestDistance = maxDistance; // Initialize closest distance with max value.

            // Iterate through the array to find the closest node.
            foreach (Node child in from)
            {
                // Check if child node is closer, not banned, and not blocked.
                if (child.DistanceToNode(this) < closestDistance && !banned.Contains(child) && !child.Blocked)
                {
                    closest = child; // Update closest node.
                    closestDistance = child.DistanceToNode(this); // Update closest distance.
                }
            }

            return closest; // Return the closest node found.
        }

        // Determines if the current node can reach the specified node without any obstructions.
        public bool CanReach(Node node)
        {
            // Cast a ray from the current node towards the specified node and check for collisions.
            RaycastHit2D hit = Physics2D.Raycast(Position, (Position - node.Position).normalized);

            // Return true if the ray hits the specified node, indicating a clear path.
            return hit.transform == node.transform;
        }

        // Converts a generic Pathfinding.Node to a GBFS-specific Node, adding necessary components if needed.
        public static Node Morph(Pathfinding.Node n)
        {
            // If 'n' is already a GBFS Node, return it as is.
            if (n is Node) { return n as Node; }

            // If 'n' has a Node component, return that component.
            if (n.gameObject.GetComponent<Node>() != null) { return n.gameObject.GetComponent<Node>(); }

            // Otherwise, add a Node component to 'n' and return it.
            return n.gameObject.AddComponent<Node>();
        }
    }

    // Enum to represent possible outcomes of a retrieval attempt in the GBFS algorithm.
    public enum RetrievalResult
    {
        Failure,        // Indicates the retrieval failed.
        Indeterminate,  // Indicates the outcome is uncertain or in progress.
        Success         // Indicates successful retrieval.
    }
}