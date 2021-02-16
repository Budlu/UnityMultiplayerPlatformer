using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freecam : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 15f, scrollThreshold = 10f, zoomSpeed = 5f;
    [SerializeField] float width = 40f, height = 40f;
    [SerializeField] float startSize = 5f, minSize = 5f, maxSize;
    float minX, maxX, minY, maxY, startMaxX, startMaxY;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = startSize;

        minX = startSize * cam.aspect - 0.5f;
        maxX = width - minX - 1f;
        minY = startSize - 0.5f;
        maxY = height - minY - 1f;

        startMaxX = maxX;
        startMaxY = maxY;

        maxSize = Mathf.Min(width / cam.aspect / 2, height / 2);
    }

    void Update()
    {
        Zoom();
        Move();
    }

    void Zoom()
    {
        Vector2 dScroll = Input.mouseScrollDelta;

        if (dScroll == Vector2.zero)
            return;

        float targetSize = cam.orthographicSize + dScroll.y * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(targetSize, minSize, maxSize);

        minX = cam.orthographicSize * cam.aspect - 0.5f;
        maxX = width - minX - 1f;
        minY = cam.orthographicSize - 0.5f;
        maxY = height - minY - 1f;
    }

    void Move()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 targetPos = transform.position;

        if (mousePos.x < scrollThreshold)
        {
            targetPos.x -= scrollSpeed * Time.deltaTime;
        }
        else if (mousePos.x > Screen.width - scrollThreshold)
        {
            targetPos.x += scrollSpeed * Time.deltaTime;
        }

        if (mousePos.y < scrollThreshold)
        {
            targetPos.y -= scrollSpeed * Time.deltaTime;
        }
        else if (mousePos.y > Screen.height - scrollThreshold)
        {
            targetPos.y += scrollSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(Mathf.Clamp(targetPos.x, minX, maxX),
            Mathf.Clamp(targetPos.y, minY, maxY), transform.position.z);
    }
}
