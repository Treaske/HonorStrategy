using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using TMPro;

public class GameController : MonoBehaviour
{    
    [SerializeField] Tilemap tilemap;
    public GridCreation gridCreation;
    static public int turnosPartida = 5;
    public CharInfo selectedCharacter;
    public SelectedTile selected;
    public OverTile selectedOver;
    OverTile overTiles;
    public int charSelection = 0;
    public TMP_Text textoTurno;
    public TMP_Text textoMovimientos;
    public int textTurn;


    void Start()
    {
        //Crear Campo
        gridCreation.GenerateGrid();

        //Crear ejercitos
        gridCreation.GenerateCharactersPlayer();
        gridCreation.GenerateCharactersEnemy();
        
        //Comprobar posiciones iniciales
        gridCreation.HandleNewPositions();

        //Crear campo dado la vuelta
        gridCreation.DuplicateGrid();

        //Generar ejercitos dados la vuelta
        gridCreation.CharacPlayerDup();
        gridCreation.CharacEnemyDup();

        textTurn = 0;

        textoMovimientos.text = "Movimientos: " + turnosPartida;
        textoTurno.text = "Turno: " + textTurn;

        //llamar a la comprobacion de posiciones de char1Info
    }

    void Update()
    {
        HandleCharacterSelection();
        //Codigo para los turnos
        if(turnosPartida != 0 && selectedCharacter)
        {
            HandleCharacterMovement();

            //Mostrar por pantalla en numero del turno que se encuentra la partida y los movimientos que te quedan
            textoMovimientos.text = "Movimientos: " + turnosPartida;
            textoTurno.text = "Turno: " + textTurn;
        } 
    }

    void HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0) && charSelection == 0)
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

                //Mantener el selectedTile pintado para que se entienda que charac se va a mover
                mousePos = characInColumn[0].transform.position;
                mousePos.z = 1;

                overTiles = GameObject.FindObjectsOfType<OverTile>()
                    .FirstOrDefault(tile => tile.transform.position == mousePos);

                overTiles.tileSelected = 1;    
                charSelection = 1; 

                selectedOver.ShowTileOver(overTiles);
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

                //buscar manera de que seleccione los dos characters, tanto el de la vista del jugador como la del rival
                //mover todo este codigo a un script a parte donde  se maneje todo el movimiento de los personajes

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

                    turnosPartida--;
                    Debug.Log("Turnos: " + turnosPartida);
                    selectedCharacter.CheckNewPosition(gridCreation.posPlayer, selectedCharacter);
                    selectedCharacter = null;

                    
                    //------- Borrar overTile para que no de errores visuales
                    
                    List<CharInfo> characInColumn = gridCreation.posPlayer
                        .Where(c => c.positionInt.y == targetGridPos.y)
                        .OrderBy(c => c.positionInt.x)
                        .ToList();

                    if(characInColumn.Count > 1)
                    {
                        mousePos = characInColumn[1].transform.position;
                        mousePos.z = 1;

                        SelectedTile selectedTiles = GameObject.FindObjectsOfType<SelectedTile>()
                            .FirstOrDefault(tile => tile.transform.position == mousePos);

                        selected.HideTile(selectedTiles);

                        //Borrar el selectedTile pintado 

                        overTiles.tileSelected = 0;    
                    } 
                    selectedOver.HideTileOver(overTiles);
                    charSelection = 0;
  
                }
            }
        }
    }
}