using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    private BuildData buildData = new BuildData();

    [SerializeField] Transform worldHolder;
    GameObject hoverBlock;
    SpriteRenderer hoverSprite;
    SpriteSelection spriteSelection;
    Block selected = new Block(BlockType.empty, 0, 0);

    Color emptyPlace = new Color(0f, 1f, 0f, 0.5f);

    // Get size from somewhere
    int height = 40, width = 40;
    float hoverOffset = -1f;

    Block[,] blockData;
    GameObject[,] blockView;
    Inventory inv;
    Camera cam;

    KeyCode select1;
    KeyCode select2;

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

        spriteSelection = FindObjectOfType<SpriteSelection>();
        spriteSelection.gameObject.SetActive(false);

        hoverBlock = Instantiate(BlockData.Instance.prefabs[BlockType.empty], transform);
        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.color = emptyPlace;
        hoverBlock.transform.position = new Vector3(hoverBlock.transform.position.x, hoverBlock.transform.position.y, hoverOffset);

        UpdateSelectKeys();
        InputManager.Instance.ControlsChanged += UpdateSelectKeys;

        buildData.SetMapState(blockData, blockView);
        buildData.SetDimensions(height, width);
        buildData.SetObjectData(this, worldHolder, hoverBlock, hoverSprite, spriteSelection, selected, inv, cam);
    }

    public void ChangeMode(IBuildMode mode)
    {
        buildData.SetBuildMode(mode);
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

    private void UpdateSelectKeys()
    {
        select1 = InputManager.Instance.map[Inputs.select1];
        select2 = InputManager.Instance.map[Inputs.select2];

        buildData.SetSelectKeys(select1, select2);
    }

    public void TryRotate()
    {
        buildData.TryRotate();
    }

    void Update()
    {
        buildData.ModeUpdate();
    }

    public void UpdateHoverBlock(Block block)
    {
        buildData.UpdateHoverBlock(block);
    }

    public void TryChangeSprite(Block block)
    {
        buildData.TryChangeSprite(block);
    }

    public IEnumerator ChangeModeOnRelease(KeyCode key, IBuildMode mode)
    {
        while (true)
        {
            if (!Input.GetKey(key))
            {
                ChangeMode(mode);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}