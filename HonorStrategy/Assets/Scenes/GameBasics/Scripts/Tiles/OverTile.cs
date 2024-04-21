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
    public GameController gameController;
    public int tileSelected = 0;

    void OnMouseEnter()
    {
        Vector3 mousePos = gameObject.transform.position;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);
        // cuando hay un character seleccionado
        if (gameController.charSelection == 1)
        {
            List<CharInfo> characInColumn = gridCreation.posPlayer
                .Where(c => c.positionInt.y == gridPos.y)
                .OrderBy(c => c.positionInt.x)
                .ToList();

            if(characInColumn.Count > 0)
            {
                //------Cuadro de selected dorado mas uno
                gridPos = characInColumn[0].positionInt;
                gridPos.x -= 1;

                // SelectionTile
                SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                    .FirstOrDefault(tile => tile.positionInt == gridPos);

                selected.ShowTile(selectedTile); 

            }else
            {
                //------Cuadro de selected cuando no hay characters en la fila
                gridPos.z = 0;
                gridPos.x = 12;

                SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                    .FirstOrDefault(tile => tile.positionInt == gridPos);

                selected.ShowTile(selectedTile); 
            }
        } else {
            if (tileSelected == 0)
            {
                mousePos.z = 0;

                CharInfo[] charInfos = GameObject.FindObjectsOfType<CharInfo>();
                CharInfo charAtMousePos = charInfos.FirstOrDefault(charac => charac.transform.position == mousePos);
            
                //OverTile
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
            }
        }
    }

    void OnMouseExit()
    {
        Vector3 mousePos = gameObject.transform.position;
        Vector3Int gridPos = tilemap.WorldToCell(mousePos);

        if (gameController.charSelection == 1)
        {
             
            List<CharInfo> characInColumn = gridCreation.posPlayer
                .Where(c => c.positionInt.y == gridPos.y)
                .OrderBy(c => c.positionInt.x)
                .ToList();
            
            if(characInColumn.Count > 0)
            {
                //------Cuadro de selected dorado mas uno
                gridPos = characInColumn[0].positionInt;
                gridPos.x -= 1;

                SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                    .FirstOrDefault(tile => tile.positionInt == gridPos);

                selected.HideTile(selectedTile); 
            }else
            {
                gridPos.z = 0;
                gridPos.x = 12;

                SelectedTile selectedTile = GameObject.FindObjectsOfType<SelectedTile>()
                    .FirstOrDefault(tile => tile.positionInt == gridPos);

                selected.HideTile(selectedTile);     
            }
        }
        if (tileSelected == 0)
        {
            //OverTile
            GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);
            
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

                selected.HideTile(selectedTile); 
            }
        }
    }

    public void ShowTileOver(OverTile selction)
    {
        selction.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,1);
    }

    public void HideTileOver(OverTile selction)
    {
        selction.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);
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