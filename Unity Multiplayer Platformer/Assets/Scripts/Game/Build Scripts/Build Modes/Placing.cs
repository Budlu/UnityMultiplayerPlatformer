using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placing : BuildData, IBuildMode
{
    Color emptyPlace = new Color(0f, 1f, 0f, 0.5f);
    Color overwritePlace = new Color(1f, 0f, 0f, 0.5f);

    float hoverOffset = -1f;

    public void Begin()
    {
        hoverBlock.SetActive(true);
        inv.HideHighlight(false);

        MoveHover();
        ChangeHoverBlock(inv.GetSelectedBlock());
    }

    public void Update()
    {
        MoveHover();
        CheckForPlacement();
    }

    public void End()
    {
        hoverBlock.SetActive(false);
    }

    private void UpdateHoverColor()
    {
        if (selected.GetValidPosition(blockData, lastX, lastY))
            hoverSprite.color = emptyPlace;
        else
            hoverSprite.color = overwritePlace;
    }

    private void MoveHover()
    {
        MouseMovement();

        hoverBlock.transform.position = new Vector3(lastX, lastY, hoverOffset);
        UpdateHoverColor();
    }

    private void CheckForPlacement()
    {
        if (Input.GetKey(select1) && selected.GetValidPosition(blockData, lastX, lastY))
        {
            Place(selected, lastX, lastY);
            UpdateHoverColor();
        }
    }

    private void Place(Block block, int xPos, int yPos)
    {
        if (blockView[xPos, yPos] != null)
            GameObject.Destroy(blockView[xPos, yPos].gameObject);

        GameObject newBlock = BlockData.Instance.prefabs[block.GetBlockType()];

        blockView[xPos, yPos] = GameObject.Instantiate(newBlock, worldHolder);
        blockView[xPos, yPos].transform.position = new Vector2(xPos, yPos);
        blockView[xPos, yPos].transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
        blockView[xPos, yPos].GetComponent<SpriteRenderer>().sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];

        blockData[lastX, lastY] = new Block(block.GetBlockType(), block.GetRotation(), block.GetSpriteId());
        blockData[lastX, lastY].PlaceLinkBlocks(blockData, lastX, lastY);
    }

    public void ChangeHoverBlock(Block block)
    {
        Vector3 startPos = hoverBlock.transform.position;

        GameObject.Destroy(hoverBlock.gameObject);

        hoverBlock = GameObject.Instantiate(BlockData.Instance.prefabs[block.GetBlockType()], bm.transform);
        hoverBlock.transform.position = startPos;
        hoverBlock.transform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);

        hoverSprite = hoverBlock.GetComponent<SpriteRenderer>();
        hoverSprite.sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];

        UpdateHoverColor();
    }

    public void ChangeMode(IBuildMode mode)
    {
        lastMode = BuildData.mode;
        End();

        BuildData.mode = mode;
        BuildData.mode.Begin();
    }

    public void RotateBlock()
    {
        Block block = inv.GetSelectedBlock();
        block.Rotate();

        inv.SetSlot(inv.GetActiveSlot(), block);
        UpdateHoverBlock(block);
    }
}
