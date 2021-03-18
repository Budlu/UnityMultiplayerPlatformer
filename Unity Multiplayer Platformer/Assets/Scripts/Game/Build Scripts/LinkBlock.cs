using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBlock : Block
{
    int parentX = -1, parentY = -1;

    public LinkBlock(BlockType type, int rotation, int spriteId, int parentX, int parentY) : base(type, rotation, spriteId)
    {
        this.parentX = parentX;
        this.parentY = parentY;
    }

    public override void Erase(Block[,] world, GameObject[,] view, int x, int y)
    {
        world[parentY, parentX].Erase(world, view, parentX, parentY);
    }

    public int GetParentX()
    {
        return parentX;
    }

    public int GetParentY()
    {
        return parentY;
    }
}
