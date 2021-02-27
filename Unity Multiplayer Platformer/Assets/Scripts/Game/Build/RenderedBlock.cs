using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderedBlock : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    Block block;

    public void SetBlock(Block block)
    {
        this.block = block;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, block.GetRotation() * 90));
        spriteRenderer.sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];
    }

    public Block GetBlock()
    {
        return block;
    }
}
