using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public Node[] GetClosestBypass(Node from, Node to, float precision, float precisionStep, float minimumPrecision, Node[] banned)
        {
            List<Node> options = new();
            do
            {
                if (precision < minimumPrecision) break;
                options = to.GetClosestFromWithTiebreaker(nodes, new Node[] {from}.ToList(), precision, banned);
                if (options == null) return null;
                if (options.Count == 0) return null;
                precision -= precisionStep;
            } while (options.Count > 1);
            Node end = options.First();
            if (end == null) return null;
            Node[] connection = end.GetClostestConnection(from, banned).Reverse().ToArray();
            return connection;
        }
    }
}