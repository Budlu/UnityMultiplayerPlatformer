using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int activeSlot = 0;
    int itemSlots = 9;

    Block[] items;
    KeyCode[] keys;

    void Start()
    {
        items = new Block[itemSlots];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Block(BlockType.square, 0, 0);
        }

        keys = new KeyCode[itemSlots];

        UpdateKeys(this, System.EventArgs.Empty);
        InputManager.Instance.ControlsChanged += UpdateKeys;
    }

    private void UpdateKeys(object sender, System.EventArgs args)
    {
        Dictionary<Inputs, KeyCode> map = InputManager.Instance.map;

        keys[0] = map[Inputs.slot1];
        keys[1] = map[Inputs.slot2];
        keys[2] = map[Inputs.slot3];
        keys[3] = map[Inputs.slot4];
        keys[4] = map[Inputs.slot5];
        keys[5] = map[Inputs.slot6];
        keys[6] = map[Inputs.slot7];
        keys[7] = map[Inputs.slot8];
        keys[8] = map[Inputs.slot9];
    }

    void Update()
    {
        CheckKeyPress();
    }

    private void CheckKeyPress()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                ChangeSlot(i);
            }
        }
    }

    private void ChangeSlot(int slot)
    {
        activeSlot = slot;
    }

    public Block GetSelectedBlock()
    {
        return items[activeSlot];
    }
}