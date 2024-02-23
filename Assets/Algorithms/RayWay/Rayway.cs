using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.RayWay {
    public class RayWay : Algorithm
    {
        public override string Name => "RayWay";

        public bool step;
        public bool next;
        public bool showRay;

        [SerializeField]List<Node> path = new List<Node>();
        List<Node> avoid = new List<Node>();

        Node origin;
        Node target;

        public override bool ComputePath()
        {
            origin = Node.Morph(helper.origin);
            target = Node.Morph(helper.target);

            path = new List<Node>(new Node[] {origin});

            int counter = 0;
            while (true)
            {
                helper.path = path.Cast<Pathfinding.Node>().ToList();
                counter++;
                Debug.Log("Running iteration " + counter.ToString() + ", Path length: " + path.Count.ToString());

                if (path.Count < 1) return false;

                Node lastNode = path.Last();

                lastNode.GetComponent<Collider2D>().enabled = false;
                RaycastHit2D hit = Physics2D.Raycast(lastNode.Position, target.Position - lastNode.Position, lastNode.DistanceToTarget);
                lastNode.GetComponent<Collider2D>().enabled = true;
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform == target.transform)
                {
                    break;
                }
                Obstacle o = hit.transform.parent.GetComponent<Obstacle>();
                if (o == null) { avoid.Add(lastNode); path.Remove(lastNode); continue; }

                Node[] bypass = o.getClosestBypass(lastNode, target, avoid.ToArray());
                if (bypass == null) { avoid.Add(lastNode); path.Remove(lastNode); continue; }
                path.AddRange(bypass);
                if (step) { while (!next) {; } next = false; }
            }
            path.Add(target);
            helper.path = path.Cast<Pathfinding.Node>().ToList();
            return true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 drawFrom = path.Count > 0 ? path.Last().Position : helper.origin.Position;
            if (showRay) { Debug.DrawRay(drawFrom, helper.target.Position - drawFrom, Color.blue, Time.deltaTime); }
        }
    }
}