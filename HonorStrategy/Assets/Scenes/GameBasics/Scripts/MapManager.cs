using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

/*
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.GameObject);
        } else{
            _instance = this;
        }
    }
*/
 private static MapManager _instance;
    public static MapManager Instance { get { return _instance;}}
    public OverTile overTile;
    public GameObject overLayTile;

    void Start()
    {
        //Debug.Log("hila");
        var tilemap = gameObject.GetComponentInChildren<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
       // Debug.Log("hola");
        //Debug.Log("bound: " + bounds);

        for (int z = bounds.min.z; z < bounds.max.z; z++)
        {
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
          
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tileLocation = new Vector3Int(x, y, z);

                if (tilemap.HasTile(tileLocation))
                {
                    Debug.Log("hila");
                    var overlayTile = Instantiate(overTile, overLayTile.transform);
                    var cellWorldPosition = tilemap.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                }
            }
        }
        }
    }

}
