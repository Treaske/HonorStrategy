using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridCreation : MonoBehaviour
{
    public InitialPosition initialPosition;

    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;
    [SerializeField] int totalSoldiers = 20;  // Cambia este valor al número deseado de soldados
    public Sprite[] wallSprite;
    public OverTile overTile;
    public GameObject overLayTile;

    public List<CharInfo> posPlayer = new List<CharInfo>();
    public List<CharInfo> posEnemy = new List<CharInfo>();

    static public int gridHeight = 12;

    public void GenerateGrid()
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

       
        for (int y = 0; y < gridHeight; y++)
        {
          
            for (int x =0; x < gridHeight*2; x++)
            {
                var tileLocation = new Vector3Int(x, y, 0);

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


    public void GenerateCharactersPlayer()
    {
        // codigo que genera un numero de de characters, y los reparte de manera aleatoria

        int remainingSoldiers = totalSoldiers;

        while(remainingSoldiers > 0)
        {
            int aleatorio = Mathf.RoundToInt(Random.Range(0f, 11f));

            GameObject characterObj = Instantiate(characterPlayer, transform);
            CharInfo playerChar = characterObj.GetComponent<CharInfo>();
            SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

            Vector3Int targetGridPos = new Vector3Int(12, aleatorio, 0);

            while (posPlayer.Any(c => c.positionInt == targetGridPos))
            {
                targetGridPos -= new Vector3Int(1, 0, 0);
            }

            Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
            characterObj.transform.position = playerWorldPosition;

            //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
            int order = ((12 - aleatorio) * 10) + (12 - targetGridPos.x);                    
            spriteRenderer.sortingOrder = order;
            playerChar.positionInt = targetGridPos;

            //datos del charcter, a cambiar cuando haya mas tipos
            playerChar.positionInt = targetGridPos;
            playerChar.colorInt = Random.Range(0, 3);
            playerChar.damage = 1;
            playerChar.health = 1;
            playerChar.modo = 2;
            playerChar.status = 1;

            posPlayer.Add(playerChar);
            remainingSoldiers--;

        }    
    }

    public void GenerateCharactersEnemy()
    {
        // codigo que genera un numero de de characters, y los reparte de manera aleatoria

        int remainingSoldiers = totalSoldiers;

        while(remainingSoldiers > 0)
        {
            int aleatorio = Mathf.RoundToInt(Random.Range(0f, 11f));

            GameObject characterObj = Instantiate(characterEnemy, transform);
            CharInfo playerChar = characterObj.GetComponent<CharInfo>();
            SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

            Vector3Int targetGridPos = new Vector3Int(14, aleatorio, 0);

            while (posEnemy.Any(c => c.positionInt == targetGridPos))
            {
                targetGridPos += new Vector3Int(1, 0, 0);
            }

            Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
            characterObj.transform.position = playerWorldPosition;

            //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
            int order = ((12 - aleatorio) * 10) + (12 - targetGridPos.x);                    
            spriteRenderer.sortingOrder = order;
            playerChar.positionInt = targetGridPos;

            //datos del charcter, a cambiar cuando haya mas tipos
            playerChar.positionInt = targetGridPos;
            playerChar.colorInt = Random.Range(0, 3);
            playerChar.damage = 1;
            playerChar.health = 1;
            playerChar.modo = 2;
            playerChar.status = 1;

            posEnemy.Add(playerChar);
            remainingSoldiers--;

        }
    }

    public void HandleNewPositions()
    {
        //Comprueba los dos campos
        initialPosition.HandleNewPositionsPlayer(posPlayer, wallSprite);
        initialPosition.HandleNewPositionsEnemy(posEnemy, wallSprite);
    }
}

/* 

creacion antigua

private void GenerateCharactersPlayer()
    {
        // codigo que genera un numero de de characters, y los reparte de manera aleatoria

        int remainingSoldiers = totalSoldiers;

        while(remainingSoldiers > 0)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                int aleatorio = Mathf.RoundToInt(Random.Range(0f, 1f));

                if(aleatorio == 1)
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

                    //datos del charcter, a cambiar cuando haya mas tipos
                    playerChar.positionInt = targetGridPos;
                    playerChar.colorInt = Random.Range(0, 3);
                    playerChar.damage = 1;
                    playerChar.health = 1;
                    playerChar.modo = 2;
                    playerChar.status = 1;

                    posPlayer.Add(playerChar);
                    remainingSoldiers--;

                }
            }
        }    
    }


    private void GenerateCharactersPlayerQ()
    {
        int remainingSoldiers = totalSoldiers;

        while (remainingSoldiers > 0)
        {
            for (int x = 0; x < gridHeight; x++)
            {
               int soldiersInColumn = Random.Range(1, Mathf.Min(remainingSoldiers + 1, 10));

                Debug.Log(" columna: " + soldiersInColumn);

                // for que recorre una a una las columnas y genera un numero aleatorio de characters
                    
                for (int y = 0; y < soldiersInColumn; y++)
                {
                    Debug.Log(" columna: ");

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
                    remainingSoldiers--;
                    
                }
            }
        }
    }

   
    */