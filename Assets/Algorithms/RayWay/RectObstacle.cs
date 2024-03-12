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
        Vector2[] offsets = new Vector2[4];
        Vector2[] modifiers = new Vector2[] { new(-0.5f, -0.5f), new(0.5f, -0.5f), new(0.5f, 0.5f), new(-0.5f, 0.5f) };

        // Start is called before the first frame update
        void OnEnable()
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                offsets[i] = ((Vector2)nodes[i].transform.localPosition) - size * modifiers[i];
            }
        }

        // Update is called once per frame
        void Update()
        {
            colliderObject.transform.localScale = size;
            for (int i = 0; i < offsets.Length; i++)
            {
                nodes[i].transform.localPosition = size * modifiers[i] + offsets[i];
            }
        }

        private void OnValidate()
        {
            size = Vector2.Max(size, Vector2.zero);
        }
    }
}