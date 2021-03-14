using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelecting : BuildData, IBuildMode
{
    Block block;

    public SpriteSelecting(Block block)
    {
        this.block = block;
    }

    public void Begin()
    {
        BlockType type = block.GetBlockType();
        int count = BlockData.Instance.spriteCount[type];

        if (count < 2)
        {
            mode.ChangeMode(lastMode);
            return;
        }

        spriteSelection.ShowSprites(type, Input.mousePosition);
    }

    public void ChangeMode(IBuildMode mode)
    {
        if (mode is Interacting)
            return;

        End();

        BuildData.mode = mode;
        BuildData.mode.Begin();
    }

    public void End()
    {
        hoverBlock.SetActive(false);

        spriteSelection.DisableSprites();
    }

    public void RotateBlock() {}

    public void Update()
    {
        if (Input.GetKeyDown(select1))
        {
            Block currentBlock = inv.GetSelectedBlock();
            currentBlock.SetSpriteId(spriteSelection.GetHightlightId());
            inv.SetSlot(inv.GetActiveSlot(), currentBlock);

            bm.StartCoroutine(bm.ChangeModeOnRelease(select1));
        }

        if (Input.GetKeyDown(select2))
        {
            bm.StartCoroutine(bm.ChangeModeOnRelease(select1));
        }
    }
}
