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
    }
}