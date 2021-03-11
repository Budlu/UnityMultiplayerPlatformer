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
        // Might need reassignment of last mode here
        End();

        BuildData.mode = mode;
        BuildData.mode.Begin();
    }

    public void End()
    {
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

            bm.StartCoroutine(bm.ChangeModeOnRelease(select1, lastMode));
        }

        if (Input.GetKeyDown(select2))
        {
            ChangeMode(lastMode);
        }
    }
}
