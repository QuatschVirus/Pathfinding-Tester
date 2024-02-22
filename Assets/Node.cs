using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding
{
    public class Node : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public float DistanceToTarget => (PathfinderHelper.Instance.target.Position - Position).magnitude;
        public float DistanceToOrigin => (PathfinderHelper.Instance.origin.Position - Position).magnitude;

        public bool Blocked => blockedBy.Count > 0;
        public readonly List<GameObject> blockedBy;

        public float DistanceToNode(Node n) => (n.Position - Position).magnitude;

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
            blockedBy.Add(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            blockedBy.Remove(collision.gameObject);
        }
    }
}