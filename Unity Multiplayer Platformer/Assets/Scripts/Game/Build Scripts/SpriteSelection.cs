using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelection : MonoBehaviour
{
    [SerializeField] GameObject holderObject;
    [SerializeField] float loopTime = 0.05f;

    [SerializeField] Vector2 xBounds = new Vector2(500f, 1420f);
    [SerializeField] Vector2 yBounds = new Vector2(300f, 780f);

    List<SpriteHolder> spriteHolders = new List<SpriteHolder>();
    BuildManager bm;
    Inventory inventory;
    Coroutine highlightRoutine;

    int highlightBlock = -1;
    Vector3 center;
    float rotation;

    void Start()
    {
        bm = FindObjectOfType<BuildManager>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void SetPosition(Vector3 position)
    {
        Vector3 clampedPos = new Vector3(Mathf.Clamp(position.x, xBounds.x, xBounds.y),
            Mathf.Clamp(position.y, yBounds.x, yBounds.y), position.z);

        transform.position = clampedPos;
        center = clampedPos;
    }

    public void ShowSprites(BlockType type, Vector3 mousePosition)
    {
        SetPosition(mousePosition);

        Sprite[] sprites = BlockData.Instance.sprites[type];
        if (sprites.Length == 0)
            return;

        rotation = 360f / sprites.Length;

        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject holder = Instantiate(holderObject, transform);
            holder.transform.rotation = Quaternion.Euler(0f, 0f, rotation * i + 180f);

            SpriteHolder spriteHolder = holder.GetComponent<SpriteHolder>();
            spriteHolder.SetSprite(sprites[i], rotation * i);
            spriteHolders.Add(spriteHolder);
        }

        gameObject.SetActive(true);

        SetHighlight(0);
        highlightRoutine = StartCoroutine(Highlight());
    }

    IEnumerator Highlight()
    {
        while (true)
        {
            Vector2 mousePos = Input.mousePosition;
            float angle = Mathf.Atan2(mousePos.y - center.y, mousePos.x - center.x) * Mathf.Rad2Deg + 180f;
            float degrees = angle + rotation / 2;

            if (degrees >= 360)
                degrees -= 360;

            int block = Mathf.FloorToInt(degrees) / Mathf.CeilToInt(rotation);


            if (block != highlightBlock)
                SetHighlight(block);

            yield return new WaitForSeconds(loopTime);
        }
    }

    private void SetHighlight(int block)
    {
        highlightBlock = block;

        foreach (SpriteHolder holder in spriteHolders)
        {
            holder.SetHighlight(false);
        }

        spriteHolders[block].SetHighlight(true);
    }

    public void DisableSprites()
    {
        foreach (SpriteHolder holder in spriteHolders)
        {
            Destroy(holder.gameObject);
        }
        spriteHolders.Clear();

        StopCoroutine(highlightRoutine);
        highlightBlock = -1;
        gameObject.SetActive(false);
    }

    public int GetHightlightId()
    {
        return highlightBlock;
    }
}
