using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private BlockType type = BlockType.empty;
    private int rotation = 0;
    private int spriteId = 0;

    public Block(BlockType type, int rotation, int spriteId)
    {
        this.type = type;
        this.rotation = rotation;
        this.spriteId = spriteId;
    }

    public void SetType(BlockType type)
    {
        this.type = type;
        SetRotation(0);
        SetSpriteId(0);
    }

    public void SetRotation(int rotation)
    {
        this.rotation = rotation;
    }

    public void Rotate()
    {
        rotation += 1;

        if (rotation >= 4)
            rotation = 0;
    }

    public void SetSpriteId(int spriteId)
    {
        if (spriteId < BlockData.Instance.sprites[type].Length && spriteId >= 0)
        {
            this.spriteId = spriteId;
        }
    }

    public virtual void Erase(Block[,] world, GameObject[,] view, int x, int y)
    {
        foreach (Vector2 offset in BlockData.Instance.blockShapes[type])
        {
            Vector2 newOffset = GameManager.Instance.RotateVector(offset, rotation * 90);

            int blockX = x + Mathf.RoundToInt(newOffset.x);
            int blockY = y + Mathf.RoundToInt(newOffset.y);

            world[blockX, blockY] = new Block(BlockType.empty, 0, 0);
        }

        world[x, y] = new Block(BlockType.empty, 0, 0);
        GameObject.Destroy(view[x, y]);
    }

    public void PlaceLinkBlocks(Block[,] world, int x, int y)
    {
        foreach (Vector2 offset in BlockData.Instance.blockShapes[type])
        {
            Vector2 newOffset = GameManager.Instance.RotateVector(offset, rotation * 90);

            int blockX = x + Mathf.RoundToInt(newOffset.x);
            int blockY = y + Mathf.RoundToInt(newOffset.y);

            world[blockX, blockY] = new LinkBlock(type, rotation, spriteId, x, y);
        }
    }

    public bool GetValidPosition(Block[,] world, int x, int y)
    {
        if (x < 0 || y < 0)
            return false;

        if (world[x, y].GetBlockType() != BlockType.empty)
            return false;

        // Might be wrong look here later
        int width = world.GetLength(0);
        int height = world.GetLength(1);

        foreach (Vector2 offset in BlockData.Instance.blockShapes[type])
        {
            Vector2 newOffset = GameManager.Instance.RotateVector(offset, rotation * 90);

            int blockX = x + Mathf.RoundToInt(newOffset.x);
            int blockY = y + Mathf.RoundToInt(newOffset.y);

            if (blockX < 0 || blockX >= width)
                return false;

            if (blockY < 0 || blockY >= height)
                return false;

            if (world[blockX, blockY].GetBlockType() != BlockType.empty)
                return false;
        }

        return true;
    }

    public BlockType GetBlockType()
    {
        return type;
    }

    public int GetRotation()
    {
        return rotation;
    }

    public int GetSpriteId()
    {
        return spriteId;
    }
}
