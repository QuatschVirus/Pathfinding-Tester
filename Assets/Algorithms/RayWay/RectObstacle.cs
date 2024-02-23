using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RayWay
{
    /// <summary>
    /// Only needed for rectangular obstacles that need to change in size
    /// </summary>
    [ExecuteInEditMode]
    public class RectObstacle : Obstacle
    {
        public GameObject colliderObject;
        public Vector2 size;
        List<Vector2> offsets = new List<Vector2>();

        // Start is called before the first frame update
        void Start()
        {
            foreach (Node n in nodes)
            {
                Vector2 baseOffset = ((Vector2)n.Position) - size / 2;
                float x = baseOffset.x < 0 ? baseOffset.x + size.x : baseOffset.x;
                float y = baseOffset.y < 0 ? baseOffset.y + size.y : baseOffset.y;
                offsets.Add(new(x, y));
            }
        }

        // Update is called once per frame
        void Update()
        {
            colliderObject.transform.localScale = size;
            foreach 
        }

        private void OnValidate()
        {
            size = Vector2.Max(size, Vector2.zero);
        }
    }
}