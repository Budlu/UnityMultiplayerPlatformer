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

    bool pointerIn = false;

    void Start()
    {
        bm = FindObjectOfType<BuildManager>();
        inv = FindObjectOfType<Inventory>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIn = true;

        bm.ChangeMode(new Interacting());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIn = false;

        if (bm.GetActiveBuildMode() is Interacting)
            bm.ChangeToLastMode();
    }

    public void UpdateSlot(int slot, Block block)
    {
        items[slot].sprite = BlockData.Instance.itemSprites[block.GetBlockType()][block.GetSpriteId()];
        items[slot].rectTransform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
    }

    public void SelectSlot(int slot)
    {
        highlight.rectTransform.position = items[slot].rectTransform.position;
        highlight.gameObject.SetActive(true);
    }

    public void HideHighlight(bool hidden)
    {
        highlight.gameObject.SetActive(!hidden);
    }

    public bool GetPointerIn()
    {
        return pointerIn;
    }
}
