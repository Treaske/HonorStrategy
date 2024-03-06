using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameController : MonoBehaviour
{    
    [SerializeField] Tilemap tilemap;
    public GridCreation gridCreation;
    static public int turnosPartida = 3;
    public CharInfo selectedCharacter;

    void Start()
    {
        //Crear Campo
        gridCreation.GenerateGrid();

        //Crear ejercitos
        gridCreation.GenerateCharactersPlayer();
        gridCreation.GenerateCharactersEnemy();
        
        //Comprobar posiciones iniciales
        gridCreation.HandleNewPositions();

        //llamar a la comprobacion de posiciones de char1Info

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

            List<CharInfo> characInColumn = gridCreation.posPlayer
                .Where(c => c.positionInt.y == gridPos.y)
                .OrderBy(c => c.positionInt.x)
                .ToList();

            if (characInColumn.Count > 0)
            {
                selectedCharacter = characInColumn.First();
            }
        }
        
    }

    void HandleCharacterMovement()
    {
        if (selectedCharacter != null && selectedCharacter.modo == 2)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Vector3Int targetGridPos = tilemap.WorldToCell(mousePos);

                if((targetGridPos.y != selectedCharacter.positionInt.y) && tilemap.HasTile(targetGridPos))
                {
                    CharInfo lastCharacInColumn = gridCreation.posPlayer
                    .Where(c => c.positionInt.y == targetGridPos.y)
                    .OrderBy(c => c.positionInt.x)
                    .FirstOrDefault();

                    Vector3Int lastCharacPosition = tilemap.WorldToCell(mousePos);

                    if (lastCharacInColumn != null)
                    {
                        lastCharacPosition = lastCharacInColumn.positionInt;
                        lastCharacPosition -= new Vector3Int(1, 0, 0);;
                        
                    } else 
                    {
                        lastCharacPosition.x = 12;
                    }

                    Vector3 targetWorldPos = tilemap.GetCellCenterWorld(lastCharacPosition);

                    selectedCharacter.positionInt = lastCharacPosition;
                    selectedCharacter.transform.position = targetWorldPos;

                    SpriteRenderer spriteRenderer = selectedCharacter.GetComponent<SpriteRenderer>();

                    int order = ((12 - (targetGridPos.y)) * 10) + (12 -(targetGridPos.x));

                    spriteRenderer.sortingOrder = order;

                    CharInfo newCharac = selectedCharacter;

                    gridCreation.posPlayer.Remove(selectedCharacter);
                    gridCreation.posPlayer.Add(newCharac);
                }
                if(selectedCharacter != null)
                {
                    selectedCharacter.CheckNewPosition(gridCreation.posPlayer, selectedCharacter);
                    selectedCharacter = null;
                }
            }
            
        }
    }
}