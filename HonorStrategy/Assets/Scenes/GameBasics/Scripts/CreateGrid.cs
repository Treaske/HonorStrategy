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

    private List<CharInfo> posicionesOcupadas = new List<CharInfo>();

    public CharInfo selectedCharacter;

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
            // for que recorre una a una las columnas y genera un numero aleatorio de characters
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

                //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
                int order = ((12 - x) * 10) + y;
                
                spriteRenderer.sortingOrder = order;

                playerChar.position = targetGridPos;

                //datos del charcter, a cambiar cuando haya mas tipos
                playerChar.damage = 1;
                playerChar.health = 1;

                posicionesOcupadas.Add(playerChar);
               
            }
        }
    }

    void Update()
    {
        HandleCharacterSelection();
        HandleCharacterMovement();
        selectedCharacter.CheckNewPosition(posicionesOcupadas, selectedCharacter);
    }

    void HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            Debug.Log("Hola desde " + gridPos);

            List<CharInfo> characInColumn = posicionesOcupadas
                .Where(c => c.position.y == gridPos.y)
                .ToList();

            if (characInColumn.Count > 0)
            {
                selectedCharacter = characInColumn.Last();
            }
        }
    }



    void HandleCharacterMovement()
    {
        if (selectedCharacter != null && selectedCharacter.damage != 0)
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

                    int order = ((12 - (targetGridPos.y)) * 10) + (12 -(targetGridPos.x));

                    spriteRenderer.sortingOrder = order;

                    CharInfo newCharac = selectedCharacter;

                    posicionesOcupadas.Remove(selectedCharacter);
                    posicionesOcupadas.Add(newCharac);
                }

            }
        }
    }


}
