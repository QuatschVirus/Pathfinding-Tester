using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    bool dragging;
    Camera cam;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = startPos.z;
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) { transform.position = startPos; }
    }

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging= false;
    }
}
