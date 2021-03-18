using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erasing : BuildData, IBuildMode
{
    public void Begin()
    {
        inv.HideHighlight(true);
        bm.ChangeModeHighlight(1);
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
            Block block = blockData[lastY, lastX];
            block.Erase(blockData, blockView, lastX, lastY);

            GameObject prefab = blockView[lastY, lastX];
            if (prefab != null)
                GameObject.Destroy(blockView[lastY, lastX].gameObject);
        }
    }

    public void RotateBlock() { }
}
