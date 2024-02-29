using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


namespace Pathfinding
{
    public class PathfinderHelper : MonoBehaviour
    {
        public static PathfinderHelper Instance { get; protected set; }

        public Node origin;
        public Node target;

        public Text algorithmName;
        public Text timeField;

        Algorithm algorithm;

        public List<Node> path;

        LineRenderer lR;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            algorithm = GetComponent<Algorithm>();
            lR = GetComponent<LineRenderer>();

            if (algorithm == null) { Debug.LogError("No algorithm attached!"); return; }
            algorithm.Setup(this);
            algorithmName.text = algorithm.Name;
        } 

        // Update is called once per frame
        void Update()
        {
            lR.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                lR.SetPosition(i, path[i].Position);
            }
        }

        public void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        public void RunPathfinder()
        {
            ClearLog();
            Debug.Log("Computing!");
            if (algorithm == null) { Debug.LogError("No algorithm attached!"); return; }

            path.Clear();
            path.Add(origin);

            float time = Time.time;
            Debug.Log("Starting time measurments");
            algorithm.ComputePath();
            float total = Time.time - time;
            Debug.Log("Stopping measurements");

            timeField.text = $"{Mathf.Round(total * 100000) / 100}ms";
            if (!path[^1].transform == target.transform)
            {
                timeField.text += " - Failed to find path";
                lR.startColor = Color.red;
                lR.endColor = Color.red;
            } else
            {
                lR.startColor = Color.green;
                lR.endColor = Color.green;
            }
        }
    }
}