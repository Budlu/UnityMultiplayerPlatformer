using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapBuilderCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] items;
    BuildManager bm;

    void Start()
    {
        bm = FindObjectOfType<BuildManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bm.SetPlacing(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bm.SetPlacing(true);
    }

    public void UpdateSlot(int slot, Block block)
    {
        items[slot].sprite = BlockData.Instance.sprites[block.GetBlockType()][block.GetSpriteId()];
        items[slot].rectTransform.rotation = Quaternion.Euler(0, 0, block.GetRotation() * 90);
    }
}
