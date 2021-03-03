using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int activeSlot = 0;
    int itemSlots = 10;

    BuildManager bm;
    MapBuilderCanvas canvas;
    Block[] items;
    KeyCode[] keys;
    KeyCode rotate;

    void Start()
    {
        bm = GetComponent<BuildManager>();
        canvas = FindObjectOfType<MapBuilderCanvas>();

        items = new Block[itemSlots];
        for (int i = 0; i < items.Length-1; i++)
        {
            SetSlot(i, new Block(BlockType.square, 0, 0));
        }
        SetSlot(8, new Block(BlockType.spike, 0, 0));
        items[items.Length - 1] = new Block(BlockType.eraser, 0, 0);

        keys = new KeyCode[itemSlots];

        UpdateKeys();
        InputManager.Instance.ControlsChanged += UpdateKeys;

        ChangeSlot(0);
    }

    private void SetSlot(int slot, Block block)
    {
        items[slot] = block;
        canvas.UpdateSlot(slot, block);
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

    public void ChangeSlot(int slot)
    {
        activeSlot = slot;
        Block newBlock = items[slot];

        bm.UpdateHoverBlock(newBlock);
        bm.SetErasing(slot == itemSlots - 1);
        canvas.SelectSlot(slot);
    }

    private void CheckRotate()
    {
        if (Input.GetKeyDown(rotate) && activeSlot != itemSlots-1)
        {
            Block block = items[activeSlot];

            block.Rotate();
            SetSlot(activeSlot, block);
            bm.UpdateHoverBlock(block);
        }
    }

    public Block GetSelectedBlock()
    {
        return items[activeSlot];
    }
}