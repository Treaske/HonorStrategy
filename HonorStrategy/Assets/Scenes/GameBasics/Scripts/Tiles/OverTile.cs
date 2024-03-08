using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class OverTile : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    public SelectedTile selected;
    public GridCreation gridCreation;

    void OnMouseEnter()
    {
        //gameObject.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,1);

        Vector3 mousePos = gameObject.transform.position;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);

        List<CharInfo> characInColumn = gridCreation.posPlayer
            .Where(c => c.positionInt.y == gridPos.y)
            .OrderBy(c => c.positionInt.x)
            .ToList();

        if(characInColumn.Count > 0)
        {
            mousePos = characInColumn[0].transform.position;
            mousePos.z = 1;

            SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                .FirstOrDefault(tile => tile.transform.position == mousePos);

            selected.ShowTile(selectedTile); 
        }
    }

    void OnMouseExit()
    {
        //gameObject.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);

        Vector3 mousePos = gameObject.transform.position;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);

        List<CharInfo> characInColumn = gridCreation.posPlayer
            .Where(c => c.positionInt.y == gridPos.y)
            .OrderBy(c => c.positionInt.x)
            .ToList();

        if(characInColumn.Count > 0)
        {
            mousePos = characInColumn[0].transform.position;
            mousePos.z = 1;

            SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                .FirstOrDefault(tile => tile.transform.position == mousePos);

            selected.HideTile(selectedTile); 
        }
    }
}
/*
 Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);

        List<CharInfo> characInColumn = gridCreation.posPlayer
            .Where(c => c.positionInt.y == gridPos.y)
            .OrderBy(c => c.positionInt.x)
            .ToList();
            */