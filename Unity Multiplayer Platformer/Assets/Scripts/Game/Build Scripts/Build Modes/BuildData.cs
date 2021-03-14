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

    public void SetMapState(Block[,] blockData, GameObject[,] blockView)
    {
        BuildData.blockData = blockData;
        BuildData.blockView = blockView;
    }

    public void SetDimensions(int height, int width)
    {
        BuildData.height = height;
        BuildData.width = width;
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
}
