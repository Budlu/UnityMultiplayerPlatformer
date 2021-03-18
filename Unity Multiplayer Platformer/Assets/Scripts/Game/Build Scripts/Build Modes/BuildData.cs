using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildData
{
    protected static BuildManager bm;
    protected static Transform worldHolder;
    protected static GameObject hoverBlock;
    protected static SpriteRenderer hoverSprite;
    protected static SpriteSelection spriteSelection;
    protected static Block selected;
    protected static Inventory inv;
    protected static Camera cam;

    protected static Block[,] blockData;
    protected static GameObject[,] blockView;

    protected static int height, width;
    protected static int lastX = -1, lastY = -1;

    protected static KeyCode select1;
    protected static KeyCode select2;

    protected static IBuildMode mode = new Placing();
    protected static IBuildMode lastMode = new Placing();

    public void Start()
    {
        BuildManager.Instance.onDimensionsChanged += WorldUpdate;
    }

    public void SetMapState(Block[,] blockData, GameObject[,] blockView)
    {
        BuildData.blockData = blockData;
        BuildData.blockView = blockView;
    }

    public void SetDimensions(int height, int width)
    {
        BuildData.width = width;
        BuildData.height = height;
    }

    public void SetSelectKeys(KeyCode select1, KeyCode select2)
    {
        BuildData.select1 = select1;
        BuildData.select2 = select2;
    }

    public void SetObjectData(BuildManager bm, Transform worldHolder, GameObject hoverBlock, SpriteRenderer hoverSprite,
        SpriteSelection spriteSelection, Block selected, Inventory inv, Camera cam)
    {
        BuildData.bm = bm;
        BuildData.worldHolder = worldHolder;
        BuildData.hoverBlock = hoverBlock;
        BuildData.hoverSprite = hoverSprite;
        BuildData.spriteSelection = spriteSelection;
        BuildData.selected = selected;
        BuildData.inv = inv;
        BuildData.cam = cam;
    }

    public void SetBuildMode(IBuildMode mode)
    {
        BuildData.mode.ChangeMode(mode);
    }

    public void SetLastMode(IBuildMode mode)
    {
        lastMode = mode;
    }

    public void AssignBuildMode(IBuildMode mode)
    {
        BuildData.mode = mode;
        mode.Begin();
    }

    public void ChangeToLastMode()
    {
        mode.End();

        mode = lastMode;
        mode.Begin();
    }

    public void ModeUpdate()
    {
        MouseMovement();
        mode.Update();
    }

    public void UpdateHoverBlock(Block block)
    {
        selected = block;

        if (mode is Placing)
        {
            Placing placing = (Placing)mode;
            placing.ChangeHoverBlock(block);
        }
    }

    protected void MouseMovement()
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
    }

    public void TryRotate()
    {
        mode.RotateBlock();
    }

    public void TryChangeSprite(Block block)
    {
        mode.ChangeMode(new SpriteSelecting(block));
    }

    public IBuildMode GetActiveBuildMode()
    {
        return mode;
    }

    public void ForceEnd()
    {
        mode.End();
    }

    public void WorldUpdate(int width, int height)
    {
        if (width == BuildData.width && height < BuildData.height)
        {
            for (int i = height; i < BuildData.height; i++)
            {
                for (int k = 0; k < BuildData.width; k++)
                {
                    blockData[i, k].Erase(blockData, blockView, k, i);
                }
            }
        }
        else if (width < BuildData.width)
        {
            for (int i = 0; i < BuildData.height; i++)
            {
                for (int k = width; k < BuildData.width; k++)
                {
                    blockData[i, k].Erase(blockData, blockView, k, i);
                }
            }
        }

        ReplaceArrays(width, height);

        BuildData.width = width;
        BuildData.height = height;
    }

    private void ReplaceArrays(int width, int height)
    {
        Block[,] newBlockData = new Block[height, width];
        GameObject[,] newBlockView = new GameObject[height, width];

        for (int i = 0; i < Mathf.Min(BuildData.height, height); i++)
        {
            for (int k = 0; k < Mathf.Min(BuildData.width, width); k++)
            {
                newBlockData[i, k] = blockData[i, k];
                newBlockView[i, k] = blockView[i, k];
            }
        }

        for (int i = BuildData.height; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                newBlockData[i, k] = new Block(BlockType.empty, 0, 0);
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int k = BuildData.width; k < width; k++)
            {
                newBlockData[i, k] = new Block(BlockType.empty, 0, 0);
            }
        }

        blockData = newBlockData;
        blockView = newBlockView;
    }
}
