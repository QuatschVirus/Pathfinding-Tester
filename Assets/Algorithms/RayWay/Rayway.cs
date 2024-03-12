using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.RayWay {
    public class RayWay : Algorithm
    {
        public override string Name => "RayWay";
        public bool showRay;
        public int maxIterations;

        [SerializeField]List<Node> path = new();
        List<Node> avoid = new();

        Node origin;
        Node target;

        public override bool ComputePath()
        {
            origin = Node.Morph(helper.origin);
            target = Node.Morph(helper.target);

            path = new List<Node>(new Node[] {origin});

            bool found = false;

            int counter = 0;
            while (counter < maxIterations && !found)
            {
                helper.path = path.Cast<Pathfinding.Node>().ToList();
                counter++;
                Debug.Log("Running iteration " + counter.ToString() + ", Path length: " + path.Count.ToString());

                if (path.Count < 1) return false;

                Node lastNode = path.Last();

                lastNode.GetComponent<Collider2D>().enabled = false;
                RaycastHit2D hit = Physics2D.Raycast(lastNode.Position, target.Position - lastNode.Position, lastNode.DistanceToTarget);
                lastNode.GetComponent<Collider2D>().enabled = true;
                Debug.Log("Raycast Hit: " + hit.transform.gameObject.name, hit.transform.gameObject);
                if (hit.transform == target.transform)
                {
                    found = true;
                    break;
                }
                if (hit.transform.TryGetComponent(out Node n))
                {
                    Debug.Log("Hit Node");
                    path.Add(n);
                    continue;
                }
                if (hit.transform.TryGetComponent(out Pathfinding.Node pN))
                {
                    Debug.Log("Hit standard Node, morphing");
                    path.Add(Node.Morph(pN));
                    continue;
                }
                if (!hit.transform.parent.TryGetComponent(out Obstacle o)) 
                {
                    Debug.Log("Hit unknown");
                    avoid.Add(lastNode);
                    path.Remove(lastNode);
                    continue;
                }

                Node[] bypass = o.GetClosestBypass(lastNode, target, 1, 0.1f, 0, avoid.ToArray());
                if (bypass == null) { avoid.Add(lastNode); path.Remove(lastNode); continue; }
                path.AddRange(bypass);
            }
            if (found)
            {
                path.Add(target);
                helper.path = path.Cast<Pathfinding.Node>().ToList();
            }
            return found;
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 drawFrom;
            if (path.Count > 0 && path.Last().transform != helper.target.transform)
            {
                drawFrom = path.Last().Position;
            }
            else
            {
                drawFrom = helper.origin.Position;
            }
            if (showRay) { Debug.DrawRay(drawFrom, helper.target.Position - drawFrom, Color.blue, Time.deltaTime); }
        }
    }
}