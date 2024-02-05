using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTileSelection : MonoBehaviour
{
  
    void Update()
    {
        
    }

    public void ShowTile()
    {
        GameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    public void HideTile()
    {
        GameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
    }
}
