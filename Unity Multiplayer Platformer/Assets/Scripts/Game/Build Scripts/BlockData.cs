using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    empty,
    square,
    spike,
    slope,
    oneDirectional,
    eraser
}

public class BlockData : MonoBehaviour
{
    public static BlockData Instance;

    public Dictionary<BlockType, int> spriteCount;
    public Dictionary<BlockType, Vector2[]> blockShapes;
    public Dictionary<BlockType, GameObject> prefabs = new Dictionary<BlockType, GameObject>();
    public Dictionary<BlockType, Sprite[]> sprites = new Dictionary<BlockType, Sprite[]>();
    public Dictionary<BlockType, Sprite[]> itemSprites = new Dictionary<BlockType, Sprite[]>();

    void Awake()
    {
        if (!(FindObjectsOfType<BlockData>().Length > 1))
        {
            Instance = this;

            InitializeSpriteCount();
            InitializeBlockShapes();
            LoadPrefabs();
            LoadSprites();
        }
    }

    private void InitializeSpriteCount()
    {
        spriteCount = new Dictionary<BlockType, int>
        {
            { BlockType.empty, 1 },
            { BlockType.square, 4 },
            { BlockType.spike, 1 },
            { BlockType.eraser, 1 }
        };
    }

    private void InitializeBlockShapes()
    {
        Vector2[] spike = new Vector2[] { new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };

        blockShapes = new Dictionary<BlockType, Vector2[]>
        {
            { BlockType.empty, new Vector2[0] },
            { BlockType.square, new Vector2[0] },
            { BlockType.spike, spike },
            { BlockType.eraser, new Vector2[0] }
        };
    }

    private void LoadPrefabs()
    {
        GameObject empty = Resources.Load<GameObject>("Prefabs/Empty");
        prefabs.Add(BlockType.empty, empty);

        GameObject square = Resources.Load<GameObject>("Prefabs/Square");
        prefabs.Add(BlockType.square, square);

        GameObject spike = Resources.Load<GameObject>("Prefabs/Spike");
        prefabs.Add(BlockType.spike, spike);

        GameObject eraser = Resources.Load<GameObject>("Prefabs/Eraser");
        prefabs.Add(BlockType.eraser, eraser);
    }

    private void LoadSprites()
    {
        // UPDATE LATER
        Sprite[] empty = new Sprite[1];
        Sprite[] emptyItem = new Sprite[1];
        empty[0] = Resources.Load<Sprite>("Sprites/Empty");
        sprites.Add(BlockType.empty, empty);
        emptyItem[0] = Resources.Load<Sprite>("Items/Empty");
        itemSprites.Add(BlockType.empty, emptyItem);

        Sprite[] spike = new Sprite[1];
        Sprite[] spikeItem = new Sprite[1];
        spike[0] = Resources.Load<Sprite>("Sprites/Spike");
        sprites.Add(BlockType.spike, spike);
        spikeItem[0] = Resources.Load<Sprite>("Items/Spike");
        itemSprites.Add(BlockType.spike, spikeItem);


        Sprite[] eraser = new Sprite[1];
        eraser[0] = Resources.Load<Sprite>("Sprites/Eraser");
        sprites.Add(BlockType.eraser, eraser);

        int squareCount = spriteCount[BlockType.square];
        Sprite[] square = new Sprite[squareCount];
        Sprite[] squareItems = new Sprite[squareCount];
        for (int i = 0; i < squareCount; i++)
        {
            square[i] = Resources.Load<Sprite>("Sprites/Square" + i);
            squareItems[i] = Resources.Load<Sprite>("Items/Square" + i);
        }
        sprites.Add(BlockType.square, square);
        itemSprites.Add(BlockType.square, square);
    }
}
