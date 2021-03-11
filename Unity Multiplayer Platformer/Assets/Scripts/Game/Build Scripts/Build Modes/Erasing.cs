using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erasing : BuildData, IBuildMode
{
    public void Begin()
    {
        inv.SetHighlightVisibility(false);
    }

    public void ChangeMode(IBuildMode mode)
    {
        if (mode is SpriteSelecting)
            return;

        lastMode = BuildData.mode;
        End();

        BuildData.mode = mode;
        BuildData.mode.Begin();
    }

    public void End() {}

    public void Update()
    {
        MouseMovement();

        if (Input.GetKey(select1))
        {
            Block block = blockData[lastX, lastY];
            block.Erase(blockData, blockView, lastX, lastY);

            GameObject prefab = blockView[lastX, lastY];
            if (prefab != null)
                GameObject.Destroy(blockView[lastX, lastY].gameObject);
        }
    }

    public void RotateBlock() { }
}
