using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    empty,
    square,
    slope,
    oneDirectional,
    eraser
}

public class BlockData : MonoBehaviour
{
    public static BlockData Instance;

    public Dictionary<BlockType, Sprite[]> sprites = new Dictionary<BlockType, Sprite[]>();
    public GameObject[] prefabs;

    void Awake()
    {
        if (!(FindObjectsOfType<BlockData>().Length > 1))
        {
            Instance = this;
            LoadSprites();
        }
    }

    private void LoadSprites()
    {
        // UPDATE LATER
        Sprite[] empty = new Sprite[1];
        empty[0] = Resources.Load<Sprite>("Blocks/Empty");
        sprites.Add(BlockType.empty, empty);

        Sprite[] square = new Sprite[1];
        square[0] = Resources.Load<Sprite>("Blocks/Square");
        sprites.Add(BlockType.square, square);

        Sprite[] eraser = new Sprite[1];
        eraser[0] = Resources.Load<Sprite>("Blocks/Eraser");
        sprites.Add(BlockType.eraser, eraser);
    }
}
