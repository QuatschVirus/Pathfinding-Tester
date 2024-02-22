using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;


namespace Pathfinding
{
    public class PathfinderHelper : MonoBehaviour
    {
        public static PathfinderHelper Instance { get; protected set; }

        public Node origin;
        public Node target;

        Algorithm algorithm;

        public List<Node> path;

        LineRenderer lR;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            algorithm = GetComponent<Algorithm>();
            if (algorithm != null) { algorithm.Setup(this); }
            lR = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            lR.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                lR.SetPosition(i, path[i].position);
            }
        }
    }
}