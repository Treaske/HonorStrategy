using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileControler : MonoBehaviour
{
private SpriteRenderer spriteRenderer;
private bool isMouseOver = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Detectar si el mouse está sobre este objeto
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        if (hitCollider != null && hitCollider.gameObject == gameObject)
        {
            if (!isMouseOver)
            {
                ShowTile();
                isMouseOver = true;
            }
        }
        else
        {
            if (isMouseOver)
            {
                HideTile();
                isMouseOver = false;
            }
        }
    }

    public void ShowTile()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1); // Totalmente visible
        }
    }

    public void HideTile()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0); // Totalmente transparente
        }
    }
}
