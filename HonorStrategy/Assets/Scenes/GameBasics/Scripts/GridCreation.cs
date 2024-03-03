using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridCreation : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;

    private List<CharInfo> posPlayer = new List<CharInfo>();
    private List<CharInfo> posEnemy = new List<CharInfo>();

    static public int gridHeight = 12;

    void Start()
    {
        GenerateGrid();
        GenerateCharactersPlayer();
        GenerateCharactersEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        // creacion de dos grids de 12 de ancho y largo, con la creacion de una linea para separarlos

        for (int x = (gridHeight); x > 0; x--)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }

        gridHeight += 1;

        for (int y = 0; y < (gridHeight - 1); y++)
        {   
            tilemap.SetTile(new Vector3Int(gridHeight, y, 0), tiles[0]);
        }

        gridHeight -= 1;

        for (int x = (gridHeight + 2); x < (gridHeight * 2) + 2; x++)
        {
            for (int y = 0; y < (gridHeight); y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }
    }

    private void GenerateCharactersPlayer()
    {
        for (int x = 0; x < gridHeight; x++)
        {
            // for que recorre una a una las columnas y genera un numero aleatorio de characters
            int randomNum = Random.Range(0, 9);
            for (int y = 0; y < randomNum; y++)
            {
  
                GameObject characterObj = Instantiate(characterPlayer, transform);
                
                CharInfo playerChar = characterObj.GetComponent<CharInfo>();

                SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

                Vector3Int targetGridPos = new Vector3Int(12, x, 0);
                
                while (posPlayer.Any(c => c.positionInt == targetGridPos))
                {
                    targetGridPos -= new Vector3Int(1, 0, 0);
                }

                Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
                characterObj.transform.position = playerWorldPosition;

                //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
                int order = ((12 - x) * 10) + y;
                
                spriteRenderer.sortingOrder = order;

                playerChar.positionInt = targetGridPos;

                //datos del charcter, a cambiar cuando haya mas tipos
                playerChar.colorInt = Random.Range(0, 3);
                playerChar.damage = 1;
                playerChar.health = 1;
                playerChar.modo = 2;
                playerChar.status = 1;

                posPlayer.Add(playerChar);
               
            }
        }
    }

    private void GenerateCharactersEnemy()
    {
        // misma funcion para el lado contrario
        for (int x = 0; x < gridHeight; x++)
        {
            int randomNum = Random.Range(0, 9);
            for (int y = 0; y < randomNum; y++)
            {
  
                GameObject characterObj = Instantiate(characterEnemy, transform);
                
                CharInfo playerChar = characterObj.GetComponent<CharInfo>();

                SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

                Vector3Int targetGridPos = new Vector3Int(14, x, 0);
                
                while (posPlayer.Any(c => c.positionInt == targetGridPos))
                {
                    targetGridPos += new Vector3Int(1, 0, 0);
                }

                Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
                characterObj.transform.position = playerWorldPosition;

                //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
                int order = ((12 - x) * 10) + y;
                
                spriteRenderer.sortingOrder = order;

                playerChar.positionInt = targetGridPos;

                //datos del charcter, a cambiar cuando haya mas tipos
                playerChar.colorInt = Random.Range(0, 3);
                playerChar.damage = 1;
                playerChar.health = 1;
                playerChar.modo = 2;
                playerChar.status = 1;

                posPlayer.Add(playerChar);
               
            }
        }
    }
}
