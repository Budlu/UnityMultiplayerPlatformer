using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [SerializeField] Transform worldHolder;
    [SerializeField] SpriteRenderer hoverSprite;
    [SerializeField] float hoverAlpha = 0.5f;

    // Get size from somewhere
    int height = 40, width = 40;
    bool placing = true;
    bool erasing = false;
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

        hoverOffset = hoverSprite.transform.position.z;

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
                GameObject block = Instantiate(BlockData.Instance.prefabs[(int)BlockType.empty], worldHolder);
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
        CheckForPlacement();
    }

    private void CheckForPlacement()
    {
        if (placing)
        {
            Vector2 placePos = cam.ScreenToWorldPoint(Input.mousePosition);
            placePos = new Vector2(placePos.x + 0.5f, placePos.y + 0.5f);

            int xInt = Mathf.RoundToInt(Mathf.Floor(placePos.x));
            int yInt = Mathf.RoundToInt(Mathf.Floor(placePos.y));

            if (yInt >= height || xInt >= width || yInt < 0 || xInt < 0)
                return;

            hoverSprite.transform.position = new Vector3(xInt, yInt, hoverOffset);

            RenderedBlock renderedBlock = world[xInt, yInt];
            if (renderedBlock.GetBlock().GetBlockType() == BlockType.empty)
            {
                hoverSprite.color = new Color(0, 1, 0, hoverAlpha);

                if (Input.GetKey(select1))
                    Place(inv.GetSelectedBlock(), xInt, yInt);
            }
            else if (erasing)
            {
                hoverSprite.color = new Color(0, 1, 0, hoverAlpha);

                if (Input.GetKey(select1))
                    Place(inv.GetSelectedBlock(), xInt, yInt);
            }
            else
            {
                hoverSprite.color = new Color(1, 0, 0, hoverAlpha);
            }
        }
    }

    private void Place(Block block, int xPos, int yPos)
    {
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
        hoverSprite.transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
        hoverSprite.sprite = BlockData.Instance.sprites[(int)block.GetBlockType()][block.GetSpriteId()];
    }
}