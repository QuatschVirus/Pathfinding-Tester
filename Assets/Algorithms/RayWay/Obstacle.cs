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
            UpdateNodes();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateNodes()
        {
            nodes.Clear();
            foreach (GameObject child in transform)
            {
                Node n = child.GetComponent<Node>();
                if (n != null) { nodes.Add(n); continue; }
                Obstacle o = child.GetComponent<Obstacle>();
                if (o != null) { nodes.AddRange(o.nodes); continue; }
            }
        }

        public Node[] getClosestBypass(Node from, Node to, Node[] banned)
        {
            Node start = from.GetClosestFrom(nodes.ToArray(), banned);
            if (start == null) return null;

        }
    }
}