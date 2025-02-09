using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridCreation : MonoBehaviour
{ 
    public InitialPosition initialPosition;
    public List<CharInfo> posPlayer = new List<CharInfo>();
    public int gridWidth = 10;
    public int gridHeight = 10;
    public Tilemap campoPropio;
    public TileBase[] tiles;
    public GameObject overTile;
    public GameObject selectTile;
    public Camera mainCamera;

// Movimiento de camara con las teclas y con raton
    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    public float minSize = 1f;
    public float maxSize = 10f;
    public float maxY = 15f;
    public float minY = 0f;
    public float maxX = 15f;
    public float minX = -15f;
    public float panSpeed = 0.1f;
    public float scrollSpeed = 1f;

    void Update()
    {
        // Cambiar el campo de visión con las teclas + y -
        if (Input.GetKey(KeyCode.Equals) && mainCamera.orthographicSize <= 10)
        {
            mainCamera.orthographicSize  += 0.0035f;
        }
        if (Input.GetKey(KeyCode.Minus) && mainCamera.orthographicSize >= 2)
        { 
            mainCamera.orthographicSize  -= 0.0035f;
        }

        // Zoom con la rueda del ratón
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scroll * scrollSpeed, minSize, maxSize);


        // Mover la cámara con las flechas
        float moveSpeed = 3f * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow) && mainCamera.transform.position.y <= maxY)
        {
            mainCamera.transform.position += new Vector3(0, 1, 0) * moveSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) && mainCamera.transform.position.y >= minY)
        {
            mainCamera.transform.position += new Vector3(0, -1, 0) * moveSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && mainCamera.transform.position.x >= minX)
        {
            mainCamera.transform.position += new Vector3(-1, 0, 0) * moveSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow) && mainCamera.transform.position.x <= maxX)
        {
            mainCamera.transform.position += new Vector3(1, 0, 0) * moveSpeed;
        }

        // Mover la cámara con el ratón (pan)
        if (Input.GetMouseButton(2)) // Botón del medio del ratón
        {
            float moveX = -Input.GetAxis("Mouse X") * panSpeed;
            float moveY = -Input.GetAxis("Mouse Y") * panSpeed;
            
            Vector3 newPosition = mainCamera.transform.position + new Vector3(moveX, moveY, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            mainCamera.transform.position = newPosition;
        }
    }

    public void GenerateGrid()
    {
        // Creación del primer grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                campoPropio.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }

        // Línea de separación
        for (int y = 0; y < gridHeight; y++)
        {
            campoPropio.SetTile(new Vector3Int(gridWidth, y, 0), tiles[0]);
        }

        // Creación del segundo grid
        for (int x = gridWidth + 1; x < (gridWidth * 2) + 1; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var randomTile = tiles[Random.Range(0, tiles.Length)];
                campoPropio.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }

        // Creación de overlays y selección
        for (int x = 0; x < (gridWidth * 2) + 1; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
                if (campoPropio.HasTile(tileLocation))
                {
                    var overlayTile = Instantiate(overTile, transform);
                    var cellWorldPosition = campoPropio.GetCellCenterWorld(tileLocation);
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = 1;

                    var selectionTile = Instantiate(selectTile, transform);
                    selectionTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                    selectionTile.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    selectionTile.transform.position = tileLocation;
                }
            }
        }
    }

/*
Codigo para crear un segundo campo pero en espejo por si queremos que el jugador siempre se vea en primer lugar
    public void DuplicateGrid()
    {
        for (int y = 0; y < (gridHeight); y++)
        {   
            campoRival.SetTile(new Vector3Int(gridHeight + 1, y + 13, 0), tiles[0]);
        }

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
*/
}
    /*
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

            //creamos el character en la ultima posicion posible para que avanze hasta que encuentre algo que lo pare
            Vector3Int targetGridPos = new Vector3Int(1, aleatorio, 0);

            //Llamamos al movimiento del personaje y su animacion
            //playerMov.MoveToPosition();

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
    */



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