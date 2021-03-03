using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [SerializeField] Transform worldHolder;
    GameObject hoverBlock;
    SpriteRenderer hoverSprite;
    Block selected = new Block(BlockType.empty, 0, 0);

    Color emptyPlace = new Color(0f, 1f, 0f, 0.5f);
    Color overwritePlace = new Color(1f, 0f, 0f, 0.5f);

    // Get size from somewhere
    int height = 40, width = 40;
    int lastX = -1, lastY = -1;
    [SerializeField] bool placing = true, erasing = false, hovering = true;
    float hoverOffset = -1f;

    Block[,] blockData;
    GameObject[,] blockView;
    Inventory inv;
    Camera cam;

    KeyCode select1;

    void Awake()
    {
        if (FindObjectsOfType<BuildManager>().Length > 1)
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
        InitializeWorld();
        inv = GetComponent<Inventory>();
        cam = FindObjectOfType<Camera>();

        hoverBlock = Instantiate(BlockData.Instance.prefabs[BlockType.empty], transform);
        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.color = emptyPlace;
        hoverBlock.transform.position = new Vector3(hoverBlock.transform.position.x, hoverBlock.transform.position.y, hoverOffset);

        UpdateSelectKey();
        InputManager.Instance.ControlsChanged += UpdateSelectKey;
    }

    private void InitializeWorld()
    {
        blockData = new Block[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                blockData[i, k] = new Block(BlockType.empty, 0, 0);
            }
        }

        blockView = new GameObject[height, width];
    }

    private void UpdateSelectKey()
    {
        select1 = InputManager.Instance.map[Inputs.select1];
    }

    void Update()
    {
        MoveHover();
        CheckForPlacement();
    }

    private void MoveHover()
    {
        if (hovering)
        {
            Vector2 placePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition) + new Vector2(0.5f, 0.5f);

            int xInt = Mathf.RoundToInt(Mathf.Floor(placePos.x));
            int yInt = Mathf.RoundToInt(Mathf.Floor(placePos.y));

            if (yInt >= height || xInt >= width || yInt < 0 || xInt < 0)
                return;

            if (xInt == lastX && yInt == lastY)
                return;

            lastX = xInt;
            lastY = yInt;

            hoverBlock.transform.position = new Vector3(xInt, yInt, hoverOffset);
            UpdateHoverColor();
        }
    }

    private void UpdateHoverColor()
    {
        if (erasing || selected.GetValidPosition(blockData, lastX, lastY))
            hoverSprite.color = emptyPlace;
        else
            hoverSprite.color = overwritePlace;
    }

    private void CheckForPlacement()
    {
        if (!placing)
            return;

        if (Input.GetKey(select1))
        {
            if (erasing)
            {
                Erase();
            }
            else if (selected.GetValidPosition(blockData, lastX, lastY))
            {
                Place(selected, lastX, lastY);
                UpdateHoverColor();
            }
        }
    }

    private void Erase()
    {
        Block block = blockData[lastX, lastY];
        block.Erase(blockData, blockView, lastX, lastY);

        GameObject prefab = blockView[lastX, lastY];
        if (prefab != null)
            Destroy(blockView[lastX, lastY].gameObject);
    }

    private void Place(Block block, int xPos, int yPos)
    {
        if (blockView[xPos, yPos] != null)
            Destroy(blockView[xPos, yPos].gameObject);

        GameObject newBlock = BlockData.Instance.prefabs[block.GetBlockType()];

        blockView[xPos, yPos] = Instantiate(newBlock, worldHolder);
        blockView[xPos, yPos].transform.position = new Vector2(xPos, yPos);
        blockView[xPos, yPos].transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
        blockView[xPos, yPos].GetComponent<SpriteRenderer>().sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];

        blockData[lastX, lastY] = new Block(block.GetBlockType(), block.GetRotation(), block.GetSpriteId());
        blockData[lastX, lastY].PlaceLinkBlocks(blockData, lastX, lastY);
    }

    public void SetPlacing(bool placing)
    {
        this.placing = placing;
    }

    public void SetErasing(bool erasing)
    {
        this.erasing = erasing;
        UpdateHoverColor();
    }

    public void UpdateHoverBlock(Block block)
    {
        Vector3 startPos = hoverBlock.transform.position;

        Destroy(hoverBlock.gameObject);

        hoverBlock = Instantiate(BlockData.Instance.prefabs[block.GetBlockType()], transform);
        hoverBlock.transform.position = startPos;
        hoverBlock.transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);

        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];

        selected = block;
        UpdateHoverColor();
    }
}