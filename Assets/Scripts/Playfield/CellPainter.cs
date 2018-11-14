using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellPainter : MonoBehaviour {
    [Header("Cell backgrounds")]
    public Sprite sandBackground;
    public Sprite stone;

    List<Sprite> staticSprites = new List<Sprite>();
    List<Sprite> dynamicSprites = new List<Sprite>();

    [Header("Renderers")]
    public CellSpriteRenderer rendererPrefab;
    List<CellSpriteRenderer> renderers = new List<CellSpriteRenderer>();
    // Sprites order. Monotonically increases when add static layers.
    // Serves as order start while adding dynamic layers
    int order = 0;

    public void Init(CellType cellType)
    {
        if ((cellType.Get() & CellType.Type.Sand) != CellType.Type.None)
        {
            staticSprites.Add(sandBackground);
        }

        if ((cellType.Get() & CellType.Type.Stone) != CellType.Type.None)
        {
            staticSprites.Add(stone);
        }

        // We rotate all static layer above ground to be more random
        int rotator = 0;
        foreach (Sprite sprite in staticSprites)
        {
            CellSpriteRenderer renderer = Instantiate(rendererPrefab);
            renderer.transform.SetParent(transform, false);
            if (rotator++ > 0)
            {
                renderer.transform.Rotate(new Vector3(0, 0, Random.value * 360));
            }
            renderer.SetSprite(sprite, ++order);
            renderers.Add(renderer);
        }
    }

    // Use this for initialization
    void Avake()
    {
        Redraw();
    }

    // Update is called once per frame
    void Redraw()
    {
        // First destroy all dynamic sprites
        while (renderers.Count > staticSprites.Count)
        {
            int index = renderers.Count - 1;
            Destroy(renderers[index].gameObject);
            renderers.RemoveAt(index);
        }

        // Now add new if needed
        int dynOrder = order;
        foreach (Sprite sprite in dynamicSprites)
        {
            CellSpriteRenderer renderer = Instantiate(rendererPrefab);
            renderer.transform.SetParent(transform, false);
            renderer.SetSprite(sprite, ++dynOrder);
            renderers.Add(renderer);
        }
    }

    public void Clear()
    {
        if (dynamicSprites.Count > 0)
        {
            dynamicSprites.Clear();
            Redraw();
        }
    }

    public void Draw(Sprite sprite)
    {
        dynamicSprites.Add(sprite);
        Redraw();
    }
}
