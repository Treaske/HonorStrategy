using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridCreation : MonoBehaviour
{
    public InitialPosition initialPosition;
    [SerializeField] Tilemap campoPropio;
    [SerializeField] Tilemap campoRival;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;
    [SerializeField] int totalSoldiers = 20;  // Cambia este valor al número deseado de soldados
    public Sprite[] wallSprite;
    public OverTile overTile;
    public SelectedTile selectTile;
    

    public List<CharInfo> posPlayer = new List<CharInfo>();
    public List<CharInfo> posEnemy = new List<CharInfo>();

    //[SerializeField] Tilemap dupPlayer;
    public List<CharInfo> dupPlayer = new List<CharInfo>();
    public List<CharInfo> dupEnemy = new List<CharInfo>();

    static public int gridHeight = 12;

    public void GenerateGrid()
    {
        // creacion de dos grids de 12 de ancho y largo, con la creacion de una linea para separarlos

        for (int x = (gridHeight); x > 0; x--)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                campoPropio.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }

        gridHeight += 1;

        for (int y = 0; y < (gridHeight - 1); y++)
        {   
            campoPropio.SetTile(new Vector3Int(gridHeight, y, 0), tiles[0]);
        }

        gridHeight -= 1;

        for (int x = (gridHeight + 2); x < (gridHeight * 2) + 2; x++)
        {
            for (int y = 0; y < (gridHeight); y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                campoPropio.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }
       
        for (int y = 0; y < gridHeight; y++)
        {
          
            for (int x =0; x < ((gridHeight * 2) + 2); x++)
            {
                var tileLocation = new Vector3Int(x, y, 0);

                if (campoPropio.HasTile(tileLocation))
                {                    
                    var overlayTile = Instantiate(overTile, transform);
                    var cellWorldPosition = campoPropio.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = 1;

                    var selectionTile = Instantiate(selectTile, transform);
                    cellWorldPosition = campoPropio.GetCellCenterWorld(tileLocation);

                    selectionTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                    selectionTile.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    Vector3Int targetPosition = new Vector3Int(x, y, 0);
                    selectionTile.positionInt = targetPosition;
                }
            }
        }
    }

    public void DuplicateGrid()
    {
        for (int y = 0; y < (gridHeight); y++)
        {   
            campoRival.SetTile(new Vector3Int(gridHeight + 1, y + 13, 0), tiles[0]);
        }


        //Pruebas para encontrar la posicion de los tiles
        //campoRival.SetTile(new Vector3Int(13, 13, 0), tiles[0]);
        //campoPropio.SetTile(new Vector3Int(14, 13, 0), tiles[1]);
        //campoPropio.SetTile(new Vector3Int(gridHeight, 14, 0), tiles[1]);
        //var tileRan = new Vector3Int(12, 11, 0);
        //campoPropio.SetTile(new Vector3Int(x, y, 0), randomTile);
        //campoRival.SetTile(new Vector3Int(15, 13, 0), campoPropio.GetTile(tileRan));   


        int apoyoX = 14; //variables para recorrer el tilemap al contrario y asi poder dar la vuelta al grid
        int apoyoY = 11;

        for (int x = gridHeight; x > 0; x--)
        {
            for (int y = gridHeight + 1; y < (gridHeight * 2) + 1; y++)
            {
                var tileColor = new Vector3Int(apoyoX, apoyoY, 0);
                campoRival.SetTile(new Vector3Int(x, y, 0), campoPropio.GetTile(tileColor));
                apoyoY -= 1;
            }
            apoyoX += 1;
            apoyoY = 11;
        }   

        apoyoX = 12;
        apoyoY = 11;

        gridHeight += 1;
        for (int x = gridHeight + 1; x < (gridHeight * 2); x++)
        {
            for (int y = gridHeight; y < (gridHeight * 2) + 1; y++)
            {
                var tileColor = new Vector3Int(apoyoX, apoyoY, 0);
                campoRival.SetTile(new Vector3Int(x, y, 0), campoPropio.GetTile(tileColor));
                apoyoY -= 1;
            }
            apoyoX -= 1;
            apoyoY = 11;
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

            Vector3 playerWorldPosition = campoPropio.GetCellCenterWorld(targetGridPos);
            characterObj.transform.position = playerWorldPosition;

            //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
            int order = ((12 - aleatorio) * 10) + (12 - targetGridPos.x);                    
            spriteRenderer.sortingOrder = order;
            playerChar.positionInt = targetGridPos;

            //datos del character, a cambiar cuando haya mas tipos
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

            Vector3 playerWorldPosition = campoPropio.GetCellCenterWorld(targetGridPos);
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

    public void CharacPlayerDup()
    {
        //Metodo para crear el ejercito del jugador desde la vista del enemigo

        CharInfo playerChar = posPlayer[0];

        int apoyoX = 14; //variables para recorrer el tilemap al contrario y asi poder dar la vuelta al grid
        int apoyoY = 24;

        for (int x = 12; x > 0; x--)
        {
            for (int y = 0; y < 12; y++)
            {
                CharInfo characterAp = posPlayer.FirstOrDefault(c => c.positionInt == new Vector3Int(x, y, 0));;
                
                if (characterAp != null)
                {  
                    GameObject characterObj = Instantiate(characterEnemy, transform);
                    CharInfo dupChar = characterObj.GetComponent<CharInfo>();
                    SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();
                    Vector3Int targetGridPos = new Vector3Int(0, 0, 0);

                    int index = posPlayer.IndexOf(characterAp);
                
                    playerChar = posPlayer[index];
                    targetGridPos = playerChar.positionInt;
                    //Debug.Log("Posicion" + x + ": " + targetGridPos);

                    dupChar.positionInt = playerChar.positionInt;
                    dupChar.colorInt = playerChar.colorInt;
                    dupChar.damage = playerChar.damage;
                    dupChar.health = playerChar.health;
                    dupChar.modo = playerChar.modo;
                    dupChar.status = playerChar.status;

                    targetGridPos.Set(apoyoX, apoyoY, (10 - y));
                    //Debug.Log("numero X: " + x);
                    //Debug.Log("Posicion" + x + ": " + targetGridPos);

                    Vector3 playerWorldPosition = campoPropio.GetCellCenterWorld(targetGridPos);
                    characterObj.transform.position = playerWorldPosition;

                    int order = ((13 - (12 - (dupChar.positionInt.y))) * 10) + (12 - (dupChar.positionInt.x));
                    dupChar.GetComponent<SpriteRenderer>().sortingOrder = order;

                    if(dupChar.modo == 0)
                    {
                        if(dupChar.status == 2)
                        {
                            dupChar.GetComponent<SpriteRenderer>().sortingOrder += 3;
                        } else 
                        {
                            dupChar.GetComponent<SpriteRenderer>().sortingOrder += 1;
                        }
                    }

                    dupPlayer.Add(dupChar);

                    if (dupChar.status == 2)
                    {
                        characterObj.GetComponent<SpriteRenderer>().sprite = wallSprite[dupChar.status];
                    }
                }
                apoyoY -= 1;
            }
            apoyoY = 24;
            apoyoX += 1;
        }
    }

    public void CharacEnemyDup()
    {
        //Metodo para crear el ejercito del enemigo desde la vista del enemigo

         CharInfo playerChar = posEnemy[0];

        int apoyoX = 12; //variables para recorrer el tilemap al contrario y asi poder dar la vuelta al grid
        int apoyoY = 13;

        for (int x = 14; x < 25; x++) //vigilar si con 25 vale para toda la columna
        {
            for (int y = 11; y > 0; y--)
            {
                CharInfo characterAp = posEnemy.FirstOrDefault(c => c.positionInt == new Vector3Int(x, y, 0));;
                //Debug.Log("numero X: " + characterAp.positionInt);
                if (characterAp != null)
                {  
                    GameObject characterObj = Instantiate(characterPlayer, transform);
                    CharInfo dupChar = characterObj.GetComponent<CharInfo>();
                    SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();
                    Vector3Int targetGridPos = new Vector3Int(0, 0, 0);

                    int index = posEnemy.IndexOf(characterAp);
                
                    playerChar = posEnemy[index];
                    targetGridPos = playerChar.positionInt;
                    //Debug.Log("Posicion" + x + ": " + targetGridPos);

                    dupChar.positionInt = playerChar.positionInt;
                    dupChar.colorInt = playerChar.colorInt;
                    dupChar.damage = playerChar.damage;
                    dupChar.health = playerChar.health;
                    dupChar.modo = playerChar.modo;
                    dupChar.status = playerChar.status;

                    targetGridPos.Set(apoyoX, apoyoY, (10 - y));
                    Debug.Log("numero X: " + x);
                    Debug.Log("Posicion" + x + ": " + targetGridPos);

                    Vector3 playerWorldPosition = campoPropio.GetCellCenterWorld(targetGridPos);
                    characterObj.transform.position = playerWorldPosition;

                    int order = ((12 - (12 - (dupChar.positionInt.y))) * 10) + ((dupChar.positionInt.x) - 12);
                    dupChar.GetComponent<SpriteRenderer>().sortingOrder = order;

                    dupEnemy.Add(dupChar);

                    if (dupChar.status == 2)
                    {
                        characterObj.GetComponent<SpriteRenderer>().sprite = wallSprite[dupChar.status];
                    }
                }
                apoyoY += 1;
            }
            apoyoY = 13;
            apoyoX -= 1;
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