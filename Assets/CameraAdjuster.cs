using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraAdjuster : MonoBehaviour
{
    [Min(0f)]public float multiplier;

    Vector3 drag;
    Camera cam;
    float startSize;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cam.orthographicSize = Mathf.Max(cam.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * -multiplier, 1);
        }
        if (Input.GetMouseButtonDown(1)) { drag = cam.ScreenToWorldPoint(Input.mousePosition); }
        if (Input.GetMouseButton(1))
        {
            transform.Translate(drag - cam.ScreenToWorldPoint(Input.mousePosition));
            drag = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) { transform.position = new Vector3(0, 0, -1); cam.orthographicSize = startSize; }
    }
}
