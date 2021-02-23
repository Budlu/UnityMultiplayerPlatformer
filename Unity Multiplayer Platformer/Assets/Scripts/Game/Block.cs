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

    public void SetSpriteId(int spriteId)
    {
        if (spriteId < BlockData.Instance.sprites[(int)type].Length && spriteId >= 0)
        {
            this.spriteId = spriteId;
        }
    }

    public BlockType getType()
    {
        return type;
    }

    public int getRotation()
    {
        return rotation;
    }

    public int getSpriteId()
    {
        return spriteId;
    }
}
