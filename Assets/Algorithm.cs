using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public abstract class Algorithm : MonoBehaviour
    {
        protected PathfinderHelper helper;

        public abstract string Name { get; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Setup(PathfinderHelper helper)
        {
            this.helper = helper;
        }

        public abstract bool ComputePath();
    }
}