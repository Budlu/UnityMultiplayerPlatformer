using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderedBlock : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector2[] overlapPositions;

    Block block;

    public void SetBlock(Block block)
    {
        this.block = block;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, block.GetRotation() * 90));
        spriteRenderer.sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];
    }

    public bool GetValidPosition(RenderedBlock[,] world, int xInt, int yInt)
    {
        if (xInt < 0 || yInt < 0)
            return false;

        if (world[xInt, yInt].block.GetBlockType() != BlockType.empty)
            return false;

        // Might be wrong look here later
        int width = world.GetLength(0);
        int height = world.GetLength(1);

        foreach (Vector2 position in overlapPositions)
        {
            int xPos = Mathf.RoundToInt(position.x);
            int yPos = Mathf.RoundToInt(position.y);

            if (xInt + xPos < 0 || xInt + xPos > width)
                return false;

            if (yInt + yPos < 0 || yInt + yPos > height)
                return false;

            if (world[xInt + Mathf.RoundToInt(position.x), yInt + Mathf.RoundToInt(position.y)].block.GetBlockType() != BlockType.empty)
                return false;
        }

        return true;
    }

    public Block GetBlock()
    {
        return block;
    }
}
