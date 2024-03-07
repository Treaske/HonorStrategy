using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTile : MonoBehaviour
{
    
    void Update()
    {
        
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color( 1,1,1,0);
    }
}
