using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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

    HashSet<CharInfo> posicionesOcupadas = new HashSet<CharInfo>();

    private CharInfo selectedCharacter;

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
  
                GameObject characterObj = Instantiate(characterPlayer, transform);
                
                CharInfo playerChar = characterObj.GetComponent<CharInfo>();

                SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

                Vector3Int targetGridPos = new Vector3Int(12, x, 0);
                
                while (posicionesOcupadas.Any(c => c.position == targetGridPos))
                {
                    targetGridPos -= new Vector3Int(1, 0, 0);
                }

                Vector3 playerWorldPosition = tilemap.GetCellCenterWorld(targetGridPos);
                characterObj.transform.position = playerWorldPosition;
                spriteRenderer.sortingOrder = (12 - x);

                playerChar.position = targetGridPos;

                posicionesOcupadas.Add(playerChar);
               
            }
        }
    }

    void Update()
    {
        HandleCharacterSelection();
        
    }

    void HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            // Verificar si hay un personaje en la posición seleccionada
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null && hitCollider.CompareTag("Character"))
            {
               
                // Seleccionar el nuevo personaje
                selectedCharacter = posicionesOcupadas.FirstOrDefault(c => c.position == gridPos);


                Debug.Log("Hello World from " + gridPos);

                if(selectedCharacter != null)
                {
                    Debug.Log("Hello World from " + selectedCharacter.color);
                }

                // Aquí puedes realizar acciones adicionales cuando se selecciona un personaje
                /*
                if (Input.GetMouseButtonDown(1))
                {
                    Vector3 mousePosSelec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosSelec.z = 0;
                    Vector3Int targetGridPos = tilemap.WorldToCell(mousePosSelec);
                    Debug.Log("Hello World from " + targetGridPos);

                    // Mover el personaje a la nueva posición
                    Vector3 targetWorldPos = tilemap.GetCellCenterWorld(targetGridPos);
                    selectedCharacter.transform.position = targetWorldPos;

                }
                */

            }
            else
            {
                // Desseleccionar el personaje si se hace clic fuera de los personajes
                selectedCharacter = null;
            }
        }
    }
}

/*
    void HandleCharacterMovement()
    {
        if (selectedCharacter != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Vector3Int targetGridPos = tilemap.WorldToCell(mousePos);

                // Mover el personaje a la nueva posición
                MoveCharacter(selectedCharacter, targetGridPos);
            }
        }
    }

    void MoveCharacter(GameObject character, Vector3Int targetGridPos)
    {
        // Realizar verificaciones adicionales si es necesario antes de mover
        // ...

        // Actualizar la posición del personaje en el grid
        Vector3 targetWorldPos = tilemap.GetCellCenterWorld(targetGridPos);
        character.transform.position = targetWorldPos;

        // Desseleccionar el personaje después de moverlo
        DeselectCharacter();
    }

    void DeselectCharacter()
    {
        selectedCharacter = null;
        // Aquí puedes realizar acciones adicionales cuando se desselecciona un personaje
    }
}
*/
