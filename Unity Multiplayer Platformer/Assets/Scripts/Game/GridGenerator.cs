using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject gridLine;
    [SerializeField] float height = 40f, width = 40f;
    [SerializeField] float lineWidth = .05f;

    List<GameObject> horizontalLines = new List<GameObject>();
    List<GameObject> verticalLines = new List<GameObject>();

    KeyCode gridToggleKey;
    bool visible = true;

    void Start()
    {
        UpdateGridKey(this, System.EventArgs.Empty);
        InputManager.Instance.ControlsChanged += UpdateGridKey;

        for (int i = 0; i <= height; i++)
        {
            GameObject line = Instantiate(gridLine, transform);
            line.transform.localScale = new Vector3(width, lineWidth, 0);
            line.transform.position = new Vector3(width / 2 - 0.5f, i - 0.5f, 0);

            horizontalLines.Add(line);
        }   

        for (int k = 0; k <= width; k++)
        {
            GameObject line = Instantiate(gridLine, transform);
            line.transform.localScale = new Vector3(lineWidth, height, 0);
            line.transform.position = new Vector3(k - 0.5f, height / 2 - 0.5f, 0);

            verticalLines.Add(line);
        }
    }

    public void MultiplyLineWidth(float multiplier)
    {
        foreach (GameObject line in horizontalLines)
        {
            line.transform.localScale = new Vector3(width, lineWidth * multiplier, 0);
        }

        foreach (GameObject line in verticalLines)
        {
            line.transform.localScale = new Vector3(lineWidth * multiplier, height, 0);
        }
    }

    void Update()
    {
        GridToggle();
    }

    private void GridToggle()
    {
        if (Input.GetKeyDown(gridToggleKey))
        {
            ToggleVisibility();
        }
    }

    private void ToggleVisibility()
    {
        visible = !visible;

        foreach (GameObject line in horizontalLines)
        {
            line.SetActive(visible);
        }

        foreach (GameObject line in verticalLines)
        {
            line.SetActive(visible);
        }
    }

    private void UpdateGridKey(object sender, System.EventArgs e)
    {
        gridToggleKey = InputManager.Instance.map[Inputs.grid];
    }
}