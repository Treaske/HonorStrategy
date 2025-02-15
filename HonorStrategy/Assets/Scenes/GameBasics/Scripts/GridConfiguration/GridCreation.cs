using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridCreation : MonoBehaviour
{ 
    public InitialPosition initialPosition;
    //public List<CharInfo> posPlayer = new List<CharInfo>();
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
    static float minSize = 1f;
    static float maxSize = 10f;
    static float maxY = 15f;
    static float minY = 0f;
    static float maxX = 15f;
    static float minX = -15f;
    static float panSpeed = 0.1f;
    static float scrollSpeed = 1f;

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
}