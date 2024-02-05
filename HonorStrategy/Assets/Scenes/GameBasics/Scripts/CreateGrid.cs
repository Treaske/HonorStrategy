using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{

    [SerializeField] GameObject[] tile;
    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;
    [SerializeField] int gridHeight = 10;
    [SerializeField] int gridWidht = 10;
    [SerializeField] float tileSize = 1f;

    [SerializeField] int posPlayerX;
    [SerializeField] int posPlayerY;
    [SerializeField] int posEnemyX;
    [SerializeField] int posEnemyY;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid(){
        for (int x = (gridHeight * 2); x > 0; x--){
            for (int y = 0; y < gridWidht; y++){
                /*
                GameObject newTile;
                if(x > gridHeight){
                    newTile = Instantiate(tile2, transform);
                } else{
                    newTile = Instantiate(tile, transform);
                }
                */
                var randomTile = tile[Random.Range(0, tile.Length)];
                GameObject newTile = Instantiate(randomTile, transform);
                

                float posX = (x * tileSize + y * tileSize) / 2f;
                float posY = (x * tileSize - y * tileSize) / 4f;

                newTile.transform.position = new Vector2(posX, posY);
                newTile.name = x + "," + y;
            }
        }

        
        GameObject charPlayer = Instantiate(characterPlayer, transform);

        
        float posCharX = (posPlayerX * tileSize + posPlayerY * tileSize) / 2f;
        float posCharY = (posPlayerX * tileSize - posPlayerY * tileSize) / 4f;
        

        charPlayer.transform.position = new Vector2(posCharX, posCharY);
        charPlayer.name = "player";

        GameObject charEnemy = Instantiate(characterEnemy, transform);

        posCharX = (posEnemyX * tileSize + posEnemyY * tileSize) / 2f;
        posCharY = (posEnemyX * tileSize - posEnemyY * tileSize) / 4f;

        charEnemy.transform.position = new Vector2(posCharX, posCharY);
        charEnemy.name = "enemy";
    }
}
