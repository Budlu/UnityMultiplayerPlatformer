using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Inputs
{
    grid,
    slot1,
    slot2,
    slot3,
    slot4,
    slot5,
    slot6,
    slot7,
    slot8,
    slot9,
    eraser,
    select1,
    select2,
    rotate,
    sprites
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Dictionary<Inputs, KeyCode> map;
    public event Action ControlsChanged;

    void Awake()
    {
        if (!(FindObjectsOfType<InputManager>().Length > 1))
        {
            Instance = this;
        }
    }

    void Start()
    {
        map = new Dictionary<Inputs, KeyCode>()
        {
            { Inputs.grid, KeyCode.G },
            { Inputs.slot1, KeyCode.Alpha1 },
            { Inputs.slot2, KeyCode.Alpha2 },
            { Inputs.slot3, KeyCode.Alpha3 },
            { Inputs.slot4, KeyCode.Alpha4 },
            { Inputs.slot5, KeyCode.Alpha5 },
            { Inputs.slot6, KeyCode.Alpha6 },
            { Inputs.slot7, KeyCode.Alpha7 },
            { Inputs.slot8, KeyCode.Alpha8 },
            { Inputs.slot9, KeyCode.Alpha9 },
            { Inputs.eraser, KeyCode.E },
            { Inputs.select1, KeyCode.Mouse0 },
            { Inputs.select2, KeyCode.Mouse1 },
            { Inputs.rotate, KeyCode.R },
            { Inputs.sprites, KeyCode.Q }
        };
    }

    public void onControlsChanged()
    {
        ControlsChanged?.Invoke();
    }
}
