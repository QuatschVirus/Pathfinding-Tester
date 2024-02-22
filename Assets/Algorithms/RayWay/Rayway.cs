using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.RayWay {
    public class RayWay : Algorithm
    {
        public override string Name => "RayWay";

        List<Pathfinding.Node> avoid;

        public override bool ComputePath()
        {
            while (true)
            {
                Pathfinding.Node lastNode = helper.path.Last();
                RaycastHit2D hit = Physics2D.Raycast(lastNode.Position, (helper.target.Position - lastNode.Position).normalized, lastNode.DistanceToTarget);
                if (hit.transform == helper.target.transform)
                {
                    break;
                }
                Obstacle o = hit.transform.GetComponent<Obstacle>();
                if (o == null) { avoid.Add(lastNode); helper.path.Remove(lastNode); continue; }
                

            }
            helper.path.Add(helper.target);
            return true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}