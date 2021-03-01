using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [SerializeField] Transform worldHolder;
    [SerializeField] RenderedBlock hoverBlock;
    SpriteRenderer hoverSprite;

    Color emptyPlace = new Color(0f, 1f, 0f, 0.5f);
    Color overwritePlace = new Color(1f, 0f, 0f, 0.5f);

    // Get size from somewhere
    int height = 40, width = 40;
    int lastX = -1, lastY = -1;
    bool placing = true, erasing = false, hovering = true;
    float hoverOffset;

    RenderedBlock[,] world;
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

        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.color = emptyPlace;
        hoverOffset = hoverBlock.transform.position.z;

        UpdateSelectKey();
        InputManager.Instance.ControlsChanged += UpdateSelectKey;
    }

    private void InitializeWorld()
    {
        world = new RenderedBlock[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                GameObject block = Instantiate(BlockData.Instance.prefabs[BlockType.empty], worldHolder);
                RenderedBlock renderedBlock = block.GetComponent<RenderedBlock>();

                renderedBlock.SetBlock(new Block(BlockType.empty, 0, 0));
                renderedBlock.transform.position = new Vector2(i, k);

                world[i, k] = renderedBlock;
            }
        }
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
        if (hoverBlock.GetValidPosition(world, lastX, lastY) || erasing)
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
                Place(new Block(BlockType.empty, 0, 0), lastX, lastY);
            }
            else if (hoverBlock.GetValidPosition(world, lastX, lastY))
            {
                Place(inv.GetSelectedBlock(), lastX, lastY);
                UpdateHoverColor();
            }
        }
    }

    private void Place(Block block, int xPos, int yPos)
    {
        Destroy(world[xPos, yPos].gameObject);

        GameObject newBlock = BlockData.Instance.prefabs[block.GetBlockType()];
        world[xPos, yPos] = Instantiate(newBlock, worldHolder).GetComponent<RenderedBlock>();
        world[xPos, yPos].transform.position = new Vector2(xPos, yPos);
        world[xPos, yPos].SetBlock(block);
    }

    public void SetPlacing(bool placing)
    {
        this.placing = placing;
    }

    public void SetErasing(bool erasing)
    {
        this.erasing = erasing;
    }

    public void UpdateHoverBlock(Block block)
    {
        Vector2 startPos = hoverBlock.transform.position;

        Destroy(hoverBlock.gameObject);

        hoverBlock = Instantiate(BlockData.Instance.prefabs[block.GetBlockType()], transform).GetComponent<RenderedBlock>();
        hoverBlock.transform.position = startPos;
        hoverBlock.transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);

        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        UpdateHoverColor();
    }
}