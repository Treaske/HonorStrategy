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
    float tileSize = 1f;

    [SerializeField] int posPlayerX;
    [SerializeField] int posPlayerY;
    [SerializeField] int posEnemyX;
    [SerializeField] int posEnemyY;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
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

        GameObject charPlayer = Instantiate(characterPlayer, transform);

        float posCharX = (posPlayerX * tileSize + posPlayerY * tileSize) / 2f;
        float posCharY = (posPlayerX * tileSize - posPlayerY * tileSize) / 4f;

        Vector3Int playerCellPosition = tilemap.WorldToCell(new Vector3(posCharX, posCharY, 0));
        Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(playerCellPosition);

        charPlayer.transform.position = playerWorldPosition;
        charPlayer.name = "player";

        GameObject charEnemy = Instantiate(characterEnemy, transform);

        posCharX = (posEnemyX * tileSize + posEnemyY * tileSize) / 2f;
        posCharY = (posEnemyX * tileSize - posEnemyY * tileSize) / 4f;

        charEnemy.transform.position = new Vector2(posCharX, posCharY);
        charEnemy.name = "enemy";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            if (tilemap.HasTile(gridPos))
                Debug.Log("Hello World from " + gridPos);
        }
    }
}

/*
        for (int x = (gridHeight * 2); x > 0; x--){
            for (int y = 0; y < gridWidht; y++){

                var randomTile = tile[Random.Range(0, tile.Length)];
                GameObject newTile = Instantiate(randomTile, transform);
                

                float posX = (x * tileSize + y * tileSize) / 2f;
                float posY = (x * tileSize - y * tileSize) / 4f;

                newTile.transform.position = new Vector2(posX, posY);
                newTile.name = x + "," + y;
            }
        }
*/
/*
GameObject charPlayer = Instantiate(characterPlayer, transform);

float posCharX = (posPlayerX * tileSize + posPlayerY * tileSize) / 2f;
float posCharY = (posPlayerX * tileSize - posPlayerY * tileSize) / 4f;

Vector3Int playerCellPosition = tilemap.WorldToCell(new Vector3(posCharX, posCharY, 0));
Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(playerCellPosition);

charPlayer.transform.position = playerWorldPosition;
charPlayer.name = "player";

GameObject charEnemy = Instantiate(characterEnemy, transform);

posCharX = (posEnemyX * tileSize + posEnemyY * tileSize) / 2f;
posCharY = (posEnemyX * tileSize - posEnemyY * tileSize) / 4f;

Vector3Int enemyCellPosition = tilemap.WorldToCell(new Vector3(posCharX, posCharY, 0));
Vector3 enemyWorldPosition = tilemap.GetCellCenterWorld(enemyCellPosition);

charEnemy.transform.position = enemyWorldPosition;
charEnemy.name = "enemy";
*/