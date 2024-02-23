using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RayWay
{
    public class Obstacle : MonoBehaviour
    {
        public List<Node> nodes;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Node[] getClosestBypass(Node from, Node to, Node[] banned)
        {
            Node end = to.GetClosestFrom(nodes.ToArray(), banned);
            if (end == null) return null;
            Node[] connection = end.GetClostestConnection(from, banned);
            return connection;
        }
    }
}