using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;

    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;

    static public int gridHeight = 12;
    static public int gridWidth = 12;

    [SerializeField] int posPlayerX;
    [SerializeField] int posPlayerY;
    [SerializeField] int posEnemyX;
    [SerializeField] int posEnemyY;

    HashSet<Vector3Int> posicionesOcupadas = new HashSet<Vector3Int>();

    void Start()
    {
        GenerateGrid();
        GenerateCharacters();
    }

private void GenerateGrid()
    {
        for (int x = (gridHeight * 2); x > 0; x--)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }
    }

    private void GenerateCharacters()
    {
      

        for (int x = 0; x < gridWidth; x++)
        {
            int randomNum = Random.Range(0, 9);
            for (int y = 0; y < randomNum; y++)
            {
                GameObject charPlayer = Instantiate(characterPlayer, transform);

                SpriteRenderer spriteRenderer = charPlayer.GetComponent<SpriteRenderer>();

                Vector3Int targetGridPos = new Vector3Int(12, x, 0);
                
                while (posicionesOcupadas.Contains(targetGridPos))
                {
                    targetGridPos -= new Vector3Int(1, 0, 0);
                }

                Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
                charPlayer.transform.position = playerWorldPosition;
                spriteRenderer.sortingOrder = (12 - x);

                posicionesOcupadas.Add(targetGridPos);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            if (tilemap.HasTile(gridPos))
            {
                Debug.Log("Hello World from " + gridPos);
                GameObject charPlayer = Instantiate(characterPlayer, transform);

                SpriteRenderer spriteRenderer = charPlayer.GetComponent<SpriteRenderer>();

                Vector3Int targetGridPos = new Vector3Int(12, gridPos.y, gridPos.z);

                while (posicionesOcupadas.Contains(targetGridPos))
                {
                    targetGridPos -= new Vector3Int(1, 0, 0);
                }

                Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
                charPlayer.transform.position = playerWorldPosition;
                spriteRenderer.sortingOrder = (12 - gridPos.y);

                posicionesOcupadas.Add(targetGridPos);
            }
                
        }
    }
}

