using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int activeSlot = 0;
    int itemSlots = 10;

    BuildManager bm;
    Block[] items;
    KeyCode[] keys;
    KeyCode rotate;

    void Start()
    {
        bm = GetComponent<BuildManager>();
        items = new Block[itemSlots];

        for (int i = 0; i < items.Length-1; i++)
        {
            items[i] = new Block(BlockType.square, 0, 0);
        }
        items[items.Length - 1] = new Block(BlockType.empty, 0, 0);

        keys = new KeyCode[itemSlots];

        UpdateKeys();
        InputManager.Instance.ControlsChanged += UpdateKeys;

        ChangeSlot(0);
    }

    private void UpdateKeys()
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
        keys[9] = map[Inputs.slot10];

        rotate = map[Inputs.rotate];
    }

    void Update()
    {
        CheckKeyPress();
        CheckRotate();
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

        // Change to if later when doing UI highlighting
        bm.SetErasing(slot == itemSlots - 1);

        Block newBlock = items[slot];
        bm.UpdateHoverBlock(newBlock);
    }

    private void CheckRotate()
    {
        if (Input.GetKeyDown(rotate) && activeSlot != itemSlots-1)
        {
            Block block = items[activeSlot];

            block.Rotate();
            bm.UpdateHoverBlock(block);
        }
    }

    public Block GetSelectedBlock()
    {
        return items[activeSlot];
    }
}