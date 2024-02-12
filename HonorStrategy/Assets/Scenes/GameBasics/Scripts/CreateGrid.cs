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

    //HashSet<CharInfo> posicionesOcupadas = new HashSet<CharInfo>();
    private List<CharInfo> posicionesOcupadas = new List<CharInfo>();

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
        HandleCharacterMovement();
    }

    void HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            Debug.Log("Hello World from mouse " + gridPos.y);

            // Obtén todos los personajes en la posición x
            List<CharInfo> characInColumn = posicionesOcupadas
                .Where(c => c.position.y == gridPos.y)
                .ToList();

            Debug.Log("conyador char " + characInColumn.Count);

                // Verifica si hay al menos un personaje en la fila
            if (characInColumn.Count > 0)
            {
                // Obtén el último personaje de la fila
                selectedCharacter = characInColumn.Last();
                Debug.Log("Ultimo personaje " + selectedCharacter.position);
                //posicionesOcupadas.Remove(selectedCharacter);
                // Aquí puedes realizar acciones adicionales cuando se selecciona un personaje
            }

           // Debug.Log("Personaje encontrado en la posición: " + selectedCharacter.position);
            //Debug.Log("Color del personaje encontrado: " + selectedCharacter.color);

            // Aquí puedes realizar acciones adicionales cuando se selecciona un personaje
        }
    }



    void HandleCharacterMovement()
    {
        if (selectedCharacter != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Vector3Int targetGridPos = tilemap.WorldToCell(mousePos);

                if((targetGridPos.y != selectedCharacter.position.y) && tilemap.HasTile(targetGridPos))
                {
                    CharInfo lastCharacInColumn = posicionesOcupadas
                    .Where(c => c.position.y == targetGridPos.y)
                    .LastOrDefault();

                    Vector3Int lastCharacPosition = tilemap.WorldToCell(mousePos);

                    if (lastCharacInColumn != null)
                    {
                        lastCharacPosition = lastCharacInColumn.position;
                        lastCharacPosition -= new Vector3Int(1, 0, 0);;
                        
                    } else 
                    {
                        lastCharacPosition.x = 12;
                    }

                    Vector3 targetWorldPos = tilemap.GetCellCenterWorld(lastCharacPosition);

                    selectedCharacter.position = lastCharacPosition;
                    selectedCharacter.transform.position = targetWorldPos;

                    SpriteRenderer spriteRenderer = selectedCharacter.GetComponent<SpriteRenderer>();
                    spriteRenderer.sortingOrder = (12 - (targetGridPos.y));


                    CharInfo newCharac = selectedCharacter;

                    posicionesOcupadas.Remove(selectedCharacter);
                    posicionesOcupadas.Add(newCharac);

                    if (posicionesOcupadas.Any(c => c.position == lastCharacPosition))
                    {
                        Debug.Log("Añadido en " + selectedCharacter.position);
                        Debug.Log("LastCharacter en " + lastCharacPosition);
                        
                    }
                }

            }
        }
    }
}
/*

if (personajeEnPosicion)
{
    // Obtén todos los personajes en la posición
    List<CharInfo> personajesEnFila = posicionesOcupadas.Where(c => c.position == gridPos).ToList();

    // Verifica si hay al menos un personaje en la fila
    if (personajesEnFila.Count > 0)
    {
        // Obtén el último personaje de la fila
        selectedCharacter = personajesEnFila.Last();
        posicionesOcupadas.Remove(selectedCharacter);

        Debug.Log("Personaje encontrado en la posición: " + gridPos);
        Debug.Log("Color del personaje encontrado: " + selectedCharacter.color);

        // Aquí puedes realizar acciones adicionales cuando se selecciona un personaje
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
