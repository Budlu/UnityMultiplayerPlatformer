using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapBuilderCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] items;
    [SerializeField] Image highlight;

    BuildManager bm;
    Inventory inv;
    int eraserSlot = 9;

    void Start()
    {
        bm = FindObjectOfType<BuildManager>();
        inv = FindObjectOfType<Inventory>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bm.SetPlacing(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bm.ResumeState();
    }

    public void UpdateSlot(int slot, Block block)
    {
        items[slot].sprite = BlockData.Instance.itemSprites[block.GetBlockType()][block.GetSpriteId()];
        items[slot].rectTransform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
    }

    public void SelectSlot(int slot)
    {
        if (slot == eraserSlot)
            highlight.rectTransform.position = new Vector2(-100f, -100f);
        else
            highlight.rectTransform.position = items[slot].rectTransform.position;
    }
}
