using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Inputs
{
    grid
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Dictionary<Inputs, KeyCode> map;
    public event EventHandler ControlsChanged;

    private void Awake()
    {
        if (FindObjectsOfType<InputManager>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        map = new Dictionary<Inputs, KeyCode>()
        {
            { Inputs.grid, KeyCode.G }
        };
    }

    public void onControlsChanged()
    {
        ControlsChanged?.Invoke(this, EventArgs.Empty);
    }
}
