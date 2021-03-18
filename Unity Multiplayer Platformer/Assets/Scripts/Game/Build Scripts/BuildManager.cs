using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    private BuildData buildData = new BuildData();
    private BuildSettings buildSettings = new BuildSettings();

    public event Action<int, int> onDimensionsChanged;

    [SerializeField] Transform worldHolder;

    GameObject hoverBlock;
    SpriteRenderer hoverSprite;
    SpriteSelection spriteSelection;
    Block selected = new Block(BlockType.empty, 0, 0);

    Color emptyPlace = new Color(0f, 1f, 0f, 0.5f);

    int height, width;
    float hoverOffset = -1f;

    Block[,] blockData;
    GameObject[,] blockView;
    Inventory inv;
    Camera cam;
    MapBuilderCanvas canvas;
    Timer timer;

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

    IEnumerator Start()
    {
        height = (buildSettings.maxBuildHeight + buildSettings.minBuildHeight) / 2;
        width = (buildSettings.maxBuildWidth + buildSettings.minBuildWidth) / 2;

        InitializeWorld();

        inv = GetComponent<Inventory>();
        cam = FindObjectOfType<Camera>();
        canvas = FindObjectOfType<MapBuilderCanvas>();
        timer = FindObjectOfType<Timer>();

        timer.Initialize(buildSettings.buildTime);
        timer.StartCountdown();

        FindObjectOfType<GridGenerator>().GenerateLines(buildSettings.maxBuildWidth, buildSettings.maxBuildHeight);

        spriteSelection = FindObjectOfType<SpriteSelection>();
        spriteSelection.gameObject.SetActive(false);

        hoverBlock = Instantiate(BlockData.Instance.prefabs[BlockType.empty], transform);
        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.color = emptyPlace;
        hoverBlock.transform.position = new Vector3(hoverBlock.transform.position.x, hoverBlock.transform.position.y, hoverOffset);

        UpdateSelectKeys();
        InputManager.Instance.ControlsChanged += UpdateSelectKeys;

        buildData.SetDimensions(height, width);
        buildData.SetMapState(blockData, blockView);
        buildData.SetObjectData(this, worldHolder, hoverBlock, hoverSprite, spriteSelection, selected, inv, cam);
        buildData.Start();

        yield return new WaitForEndOfFrame();
        onDimensionsChanged?.Invoke(width, height);
    }

    public void ChangeMode(IBuildMode mode)
    {
        buildData.SetBuildMode(mode);
    }

    public void SetLastMode(IBuildMode mode)
    {
        buildData.SetLastMode(mode);
    }

    public void ChangeToLastMode()
    {
        buildData.ChangeToLastMode();
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

    public IEnumerator ChangeModeOnRelease(KeyCode key)
    {
        while (true)
        {
            if (!Input.GetKey(key))
            {
                buildData.ForceEnd();

                yield return new WaitForEndOfFrame();
                
                if (canvas.GetPointerIn())
                {
                    buildData.AssignBuildMode(new Interacting());
                    buildData.SetLastMode(new Placing());
                }
                else
                {
                    buildData.SetBuildMode(new Placing());
                }

                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IBuildMode GetActiveBuildMode()
    {
        return buildData.GetActiveBuildMode();
    }

    public void ChangeModeHighlight(int modeId)
    {
        canvas.ChangeModeHighlight(modeId);
    }

    public void AddWidth(int val)
    {
        int newWidth = width + val;

        if (newWidth <= buildSettings.maxBuildWidth && newWidth >= buildSettings.minBuildWidth)
        {
            width = newWidth;
            onDimensionsChanged?.Invoke(width, height);
        }
    }

    public void AddHeight(int val)
    {
        int newHeight = height + val;

        if (newHeight <= buildSettings.maxBuildHeight && newHeight >= buildSettings.minBuildHeight)
        {
            height = newHeight;
            onDimensionsChanged?.Invoke(width, height);
        }
    }
}