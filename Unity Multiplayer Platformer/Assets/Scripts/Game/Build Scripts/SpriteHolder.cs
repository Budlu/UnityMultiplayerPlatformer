using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHolder : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image[] components;

    Color highlightColor = new Color(.1f, .1f, 1f, 1f);
    Color unhighlightColor = Color.black;

    public void SetSprite(Sprite sprite, float rotation)
    {
        image.sprite = sprite;
        image.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
    }

    public void SetHighlight(bool value)
    {
        if (value)
        {
            foreach (Image component in components)
                component.color = highlightColor;
        }
        else
        {
            foreach (Image component in components)
                component.color = unhighlightColor;
        }
    }
}
