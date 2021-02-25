using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freecam : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 15f, scrollThreshold = 10f, zoomSpeed = 15f;
    [SerializeField] float width = 40f, height = 40f;
    [SerializeField] float startSize = 5f, minSize = 5f, maxSize;
    [SerializeField] float invWidth = 150f, canvasWidth = 1920f;
    float minX, maxX, minY, maxY;

    Camera cam;
    GridGenerator grid;

    void Start()
    {
        grid = FindObjectOfType<GridGenerator>();

        cam = GetComponent<Camera>();
        cam.orthographicSize = startSize;

        minX = startSize * cam.aspect - 0.5f;
        maxX = width - minX - 1f;
        minY = startSize - 0.5f;
        maxY = height - minY - 1f;

        maxSize = Mathf.Min(width / cam.aspect / 2, height / 2);

        minX -= (invWidth / canvasWidth * cam.aspect * startSize * 2);
        cam.transform.position = new Vector3(minX, cam.transform.position.y, cam.transform.position.z);
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

        float targetSize = cam.orthographicSize + -dScroll.y * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(targetSize, minSize, maxSize);

        float size = cam.orthographicSize;
        minX = size * cam.aspect - 0.5f - (invWidth / canvasWidth * cam.aspect * size * 2);
        maxX = width - (size * cam.aspect - 0.5f) - 1f;
        minY = size - 0.5f;
        maxY = height - minY - 1f;

        if (maxX < minX)
            maxX = minX;

        if (maxY < minY)
            maxY = minY;

        grid.MultiplyLineWidth(size / startSize);
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
