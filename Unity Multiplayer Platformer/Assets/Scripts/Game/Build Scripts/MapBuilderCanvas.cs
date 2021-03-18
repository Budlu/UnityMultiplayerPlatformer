using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapBuilderCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] items;
    [SerializeField] Image highlight;

    [SerializeField] Image[] modes;
    [SerializeField] Image modeHighlight;

    Inventory inv;

    bool pointerIn = false;

    void Start()
    {
        inv = BuildManager.Instance.GetComponent<Inventory>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIn = true;

        BuildManager.Instance.ChangeMode(new Interacting());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIn = false;

        if (BuildManager.Instance.GetActiveBuildMode() is Interacting)
            BuildManager.Instance.ChangeToLastMode();
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

        ClickChangeMode(0);
    }

    public void HideHighlight(bool hidden)
    {
        highlight.gameObject.SetActive(!hidden);
    }

    public bool GetPointerIn()
    {
        return pointerIn;
    }

    public void ClickChangeMode(int modeId)
    {
        ChangeModeHighlight(modeId);

        switch (modeId)
        {
            case 0:
                BuildManager.Instance.SetLastMode(new Placing());
                break;
            case 1:
                BuildManager.Instance.SetLastMode(new Erasing());
                break;
            case 2:
                BuildManager.Instance.SetLastMode(new Selecting());
                break;
            default:
                BuildManager.Instance.ChangeMode(new Interacting());
                break;
        }
    }

    public void ChangeModeHighlight(int modeId)
    {
        modeHighlight.transform.position = modes[modeId].transform.position;
    }
}
