using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding
{
    public class Node : MonoBehaviour
    {
        public Vector3 position => transform.position;
        public float distanceToTarget => (PathfinderHelper.Instance.target.position - position).magnitude;
        public float distanceToOrigin => (PathfinderHelper.Instance.origin.position - position).magnitude;

        public bool blocked { get; private set; }
        public GameObject blockedBy;

        public float DistanceToNode(Node n) => (n.position - position).magnitude;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            blocked = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            blocked = false;
        }
    }
}