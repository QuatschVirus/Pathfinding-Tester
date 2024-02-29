using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Pathfinding.RayWay
{
    public class Node : Pathfinding.Node
    {
        public List<Node> connected;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Node GetClosestConnected(Node node, Node[] banned)
        {
            Node closest = this;
            float closestDistance = DistanceToNode(node);
            foreach (Node child in connected)
            {
                if (child.DistanceToNode(node) < closestDistance && !banned.Contains(node) && !node.Blocked) { closest = child; }
            }
            return closest;
        }

        public Node[] GetClostestConnection(Node node, Node[] banned)
        {
            Node closest = GetClosestConnected(node, banned);

            if (closest != this)
            {
                return new Node[] {this, closest};
            } else
            {
                return new Node[] {this};
            }
        }

        public List<Node> GetClosestFromWithTiebreaker(List<Node> from, List<Node> deciders, float precision, Node[] banned, float maxDistance = float.MaxValue)
        {
            if (from.Count < 1) return null;
            from.OrderByDescending(n => n.DistanceToNode(this));
            maxDistance = Math.Min(from.First().DistanceToNode(this) + precision, maxDistance);
            from.RemoveAll(n => banned.Contains(n) || n.Blocked || n.DistanceToNode(this) > maxDistance);
            List<string> names = new();
            foreach (Node n in from)
            {
                names.Add(n.name);
            }
            Debug.Log("Found closest: " + string.Join(", ", names));
            switch (from.Count)
            {
                case 0: return null;
                case 1: return from.Take(1).ToList();
                default:
                    {
                        if (deciders.Count > 0)
                        {
                            return deciders.First().GetClosestFromWithTiebreaker(from, deciders.TakeLast(deciders.Count - 1).ToList(), precision, banned);
                        } else
                        {
                            return from;
                        }
                    }
            }
        }

        public Node GetClosestFrom(Node[] from, Node[] banned, float maxDistance = float.MaxValue)
        {
            Node closest = null;
            float closestDistance = maxDistance;
            foreach (Node child in from)
            {
                if (child.DistanceToNode(this) < closestDistance && !banned.Contains(child) && !child.Blocked) { closest = child; }
            }
            return closest;
        }

        public bool CanReach(Node node)
        {
            RaycastHit2D hit = Physics2D.Raycast(Position, (Position - node.Position).normalized);
            return hit.transform == node.transform;
        }

        public static Node Morph(Pathfinding.Node n)
        {
            if (n is Node) { return n as Node; }
            if (n.gameObject.GetComponent<Node>() != null) { return n.gameObject.GetComponent<Node>(); }
            return n.gameObject.AddComponent<Node>();
        }
    }

    public enum RetrievalResult
    {
        Failuire,
        Indeterminate,
        Success
    }
}