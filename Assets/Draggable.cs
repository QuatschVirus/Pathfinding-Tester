using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    bool dragging;
    Camera cam;

    Vector3 startPos;

    public GameObject referenceOverride;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (referenceOverride == null) referenceOverride = gameObject;
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
            referenceOverride.transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) { transform.position = startPos; }
    }

    private void OnMouseDown()
    {
        dragging = true;
        Selection.activeObject = referenceOverride;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
