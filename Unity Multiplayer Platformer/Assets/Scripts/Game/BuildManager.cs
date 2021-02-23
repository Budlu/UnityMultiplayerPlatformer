using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [SerializeField] Transform worldHolder;

    // Get size from somewhere
    int height = 40, width = 40;
    bool placing = true;

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
        world = new RenderedBlock[height,width];
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

        inv = GetComponent<Inventory>();
        cam = FindObjectOfType<Camera>();

        UpdateSelectKey(this, System.EventArgs.Empty);
        InputManager.Instance.ControlsChanged += UpdateSelectKey;
    }

    private void UpdateSelectKey(object sender, System.EventArgs args)
    {
        select1 = InputManager.Instance.map[Inputs.select1];
    }

    void Update()
    {
        CheckForPlacement();
    }

    private void CheckForPlacement()
    {
        if (placing && Input.GetKeyDown(select1))
        {
            Vector2 placePos = cam.ScreenToWorldPoint(Input.mousePosition);
            placePos = new Vector2(placePos.x + 0.5f, placePos.y + 0.5f);

            int xInt = Mathf.RoundToInt(Mathf.Floor(placePos.x));
            int yInt = Mathf.RoundToInt(Mathf.Floor(placePos.y));

            if (yInt >= height || xInt >= width || yInt < 0 || xInt < 0)
                return;

            RenderedBlock renderedBlock = world[xInt, yInt];

            if (renderedBlock.GetBlock().getType() == BlockType.empty)
                Place(inv.GetSelectedBlock(), xInt, yInt);
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
}