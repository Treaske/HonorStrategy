using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class SelectedTile : MonoBehaviour
{
    public Vector3Int positionInt;
    
    public void ShowTile(SelectedTile selction)
    {
        selction.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,1);
    }

    public void HideTile(SelectedTile selction)
    {
        selction.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);
    }
}