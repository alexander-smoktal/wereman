using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float cameraMoveSpeed = 200;
    public float mouseMoveRegionWidth = 10;
    // We allow camera to run out of bound a bit
    public float allowedOffset = 30;
    public float allowedZoom = 3;
    public float zoomSpeed = 10;

    // Hex mesh to not move camera outside of the map
    public BoundingRect MoveBoundingRect { get; set; }
    private Camera cameraComponent;
    private float maxZoom;
    private float minZoom;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        maxZoom = cameraComponent.orthographicSize;
        minZoom = maxZoom * allowedZoom;
    }

    // Update is called once per frame
    void Update () {
        if (MoveBoundingRect == null)
        {
            return;
        }

        Vector2 pos = transform.position;
        float viewHeigth = cameraComponent.orthographicSize;
        float viewWidth = viewHeigth * cameraComponent.aspect;

        /// Mouse movement
        if (Input.mousePosition.y <= mouseMoveRegionWidth && pos.y - viewHeigth > MoveBoundingRect.bottom - allowedOffset)
        {
            transform.Translate(Vector3.down * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.mousePosition.y >= Screen.height - mouseMoveRegionWidth && pos.y + viewHeigth < MoveBoundingRect.top + allowedOffset)
        {
            transform.Translate(Vector3.up * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.mousePosition.x <= mouseMoveRegionWidth && pos.x - viewWidth > MoveBoundingRect.left - allowedOffset)
        {
            transform.Translate(Vector3.left * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.mousePosition.x >= Screen.width - mouseMoveRegionWidth && pos.x + viewWidth < MoveBoundingRect.right + allowedOffset)
        {
            transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.Self);
        }

        float wheelScroll = Input.GetAxis("Mouse ScrollWheel");
        // XXX: Actually min zoom is bigger, because camera size becomes bigger when zoom out
        if ((wheelScroll > 0 && cameraComponent.orthographicSize > maxZoom)
            || (wheelScroll < 0 && cameraComponent.orthographicSize < minZoom))
        {
            cameraComponent.orthographicSize -= wheelScroll * zoomSpeed;
        }

        /// Keyboard movement
        transform.Translate((new Vector3(0, Input.GetAxis("Vertical"), 0) * cameraMoveSpeed * Time.deltaTime), Space.Self);
        transform.Translate((new Vector3(Input.GetAxis("Horizontal"), 0, 0) * cameraMoveSpeed * Time.deltaTime), Space.Self);
    }
}
