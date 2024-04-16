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
        Vector3 mousePos = gameObject.transform.position;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);

        mousePos.z = 0;

        CharInfo[] charInfos = GameObject.FindObjectsOfType<CharInfo>();
        CharInfo charAtMousePos = charInfos.FirstOrDefault(charac => charac.transform.position == mousePos);
       
        if(charAtMousePos){
             GetComponent<SpriteRenderer>().color = new Color( 1,1,1,1);
        }

       

        //------Cuadro de selected dorado (se colorea la casilla del ultimo character de la fila, de esa manera se entiende cual va a ser el que se mueva cuando cliques)
    
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
        // else if(characInColumn.Count = 0)
  
        //-----------------------------------------------
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);
        
        //------Cuadro de selected dorado (se colorea la casilla del ultimo character de la fila, de esa manera se entiende cual va a ser el que se mueva cuando cliques)
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
        //-----------------------------------------------
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