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

    public Sprite[] wallSprite;

    void Start()
    {
        GenerateGrid();
        GenerateCharacters();
        HandleNewPositionsAll();
    }

    void Update()
    {
        HandleCharacterSelection();
        HandleCharacterMovement();    
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
                
                while (posicionesOcupadas.Any(c => c.positionInt == targetGridPos))
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

                posicionesOcupadas.Add(playerChar);
               
            }
        }
    }

    

    void HandleCharacterSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            //Debug.Log("Hola desde " + gridPos);

            List<CharInfo> characInColumn = posicionesOcupadas
                .Where(c => c.positionInt.y == gridPos.y)
                .OrderBy(c => c.positionInt.x)
                .ToList();

            if (characInColumn.Count > 0)
            {
                selectedCharacter = characInColumn.First();
                //Debug.Log("posicion" + selectedCharacter.positionInt);
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
                    CharInfo lastCharacInColumn = posicionesOcupadas
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

                    posicionesOcupadas.Remove(selectedCharacter);
                    posicionesOcupadas.Add(newCharac);
                }
                if(selectedCharacter != null)
                {
                    selectedCharacter.CheckNewPosition(posicionesOcupadas, selectedCharacter);
                    //Debug.Log("debug characterWall " + selectedCharacter.positionInt);
                    selectedCharacter = null;
                }
            }
            
        }
    }

    void HandleNewPosition(List<CharInfo> posicionesOcupadas)
    {
        int orderLayer = 0;
        for (int a = 0; a < 12; a++)
        {

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.y == a)
                .ToList();

            if (sameLineCharacters.Count > 0)
            {
                for (int l = (12 - sameLineCharacters.Count); l < 12; l++)
                {
                    CharInfo characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l, a, 0));;
                    CharInfo characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l + 1, a, 0));;

                    if (characterWall != null && characterIn != null)
                    {
                        if(characterWall.positionInt.x != 12 && characterWall.modo == 0)
                        {
                            if((characterIn.status < characterWall.status) || characterIn.modo > characterWall.modo)
                            {
                                //Debug.Log("Hola desde" + characterWall.positionInt);
                                posicionesOcupadas.Remove(characterIn);
                                posicionesOcupadas.Remove(characterWall);

                                characterIn.positionInt.x = l;
                                Vector3 posicionCharacter = characterIn.transform.position;
                                characterIn.transform.position = new Vector3(posicionCharacter.x - 0.5f, posicionCharacter.y - 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterIn.positionInt.y)) * 10) + (12 -(characterIn.positionInt.x));
                                characterIn.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                characterWall.positionInt.x = l + 1;
                                posicionCharacter = characterWall.transform.position;
                                characterWall.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterWall.positionInt.y)) * 10) + (12 -(characterWall.positionInt.x));
                                characterWall.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                posicionesOcupadas.Add(characterWall);
                                posicionesOcupadas.Add(characterIn);

                            } else if ((characterIn.modo == 0 && characterIn.status == characterWall.status) && characterIn.status != 2)
                            {
                                Debug.Log("Hola desde if " + characterWall.positionInt);
                                posicionesOcupadas.Remove(characterWall);

                                characterIn.status++;
                                characterIn.GetComponent<SpriteRenderer>().sprite = wallSprite[characterIn.status];

                                Destroy(characterWall.gameObject);

                                for(int i = l; i > (13 - sameLineCharacters.Count); i--)
                                {
                                    Debug.Log("Hola desde if: " + characterWall.positionInt);
                                    characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(i - 1, a, 0));;
                                    posicionesOcupadas.Remove(characterWall);
                                    characterWall.positionInt.x += 1;
                                    Vector3 posicionCharacter = characterWall.transform.position;
                                    characterWall.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                                    posicionesOcupadas.Add(characterWall);
                                    
                                }
                            }
                        }
                    }
                    
                } 
            }   
        }
    }

    public void HandleNewPositionsAll()
    {
        for (int c = 0; c < 12; c++)
        {
            List<CharInfo> sameColumnCharacters = posicionesOcupadas
                .Where(a => a.positionInt.y == c)
                .ToList();

            //Debug.Log("Hola desde if: " + sameColumnCharacters.Count);

            for (int l = 12; l > (12 - sameColumnCharacters.Count); l--)
            {
                CharInfo charIn = sameColumnCharacters.FirstOrDefault(a => a.positionInt == new Vector3Int(l, c, 0));;
                
                //Debug.Log("Hola " + charIn.positionInt);

                HashSet<CharInfo> matchedTilesWall = new HashSet<CharInfo>();

                matchedTilesWall.Add(charIn);

                List<CharInfo> sameLineCharacters = posicionesOcupadas
                    .Where(a => a.positionInt.x == l)
                    .ToList();

                for (int f = c; f < 12; f++)
                {
                    CharInfo charAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(charIn.positionInt.x, f + 1, charIn.positionInt.z));;

                    if(charAp != null)
                    {
                        if(charAp.colorInt == charIn.colorInt)
                        {
                            matchedTilesWall.Add(charAp);
                        } else {
                            f = 12;
                        }   
                    } else{
                        f = 12;
                    }
                }

                for (int f = c; f > 0; f--)
                {
                    CharInfo charAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(charIn.positionInt.x, f - 1, charIn.positionInt.z));;

                    if(charAp != null)
                    {
                        if(charAp.colorInt == charIn.colorInt)
                        {
                            matchedTilesWall.Add(charAp);
                        } else {
                            f = 0;
                        }   
                    } else{
                        f = 0;
                    }
                }

                if (matchedTilesWall.Count > 2)
                {
                    foreach (var character in matchedTilesWall)
                    {
                        character.GetComponent<SpriteRenderer>().sprite = wallSprite[1];

                        int order = ((12 - (character.positionInt.y)) * 10) + (12 -(character.positionInt.x));

                        character.GetComponent<SpriteRenderer>().sortingOrder = order;

                        character.modo = 0;
                        character.colorInt = 3;
                    }
                }
            }
        }
        
        HandleNewPositionWall(posicionesOcupadas);
    }

    public void HandleNewPositionWall(List<CharInfo> posicionesOcupadas)
    {
        int orderLayer = 0;
        for (int a = 0; a < 12; a++)
        {

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.y == a)
                .ToList();

            if (sameLineCharacters.Count > 0)
            {
                for (int l = (12 - sameLineCharacters.Count); l < 12; l++)
                {
                    CharInfo characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l, a, 0));;
                    CharInfo characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l + 1, a, 0));;

                    if (characterWall != null && characterIn != null)
                    {
                        if(characterWall.positionInt.x != 12 && characterWall.modo == 0)
                        {
                            if((characterIn.status < characterWall.status) || characterIn.modo > characterWall.modo)
                            {
                                //Debug.Log("Hola desde" + characterWall.positionInt);
                                posicionesOcupadas.Remove(characterIn);
                                posicionesOcupadas.Remove(characterWall);

                                characterIn.positionInt.x = l;
                                Vector3 posicionCharacter = characterIn.transform.position;
                                characterIn.transform.position = new Vector3(posicionCharacter.x - 0.5f, posicionCharacter.y - 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterIn.positionInt.y)) * 10) + (12 -(characterIn.positionInt.x));
                                characterIn.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                characterWall.positionInt.x = l + 1;
                                posicionCharacter = characterWall.transform.position;
                                characterWall.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterWall.positionInt.y)) * 10) + (12 -(characterWall.positionInt.x));
                                characterWall.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                posicionesOcupadas.Add(characterWall);
                                posicionesOcupadas.Add(characterIn);

                            } else if ((characterIn.modo == 0 && characterIn.status == characterWall.status) && characterIn.status != 2)
                            {
                                //Debug.Log("Hola desde if " + characterWall.positionInt);
                                posicionesOcupadas.Remove(characterWall);

                                characterIn.status++;
                                characterIn.GetComponent<SpriteRenderer>().sprite = wallSprite[characterIn.status];

                                Destroy(characterWall.gameObject);

                                for(int i = l; i > (13 - sameLineCharacters.Count); i--)
                                {
                                    Debug.Log("Hola desde if: " + characterWall.positionInt);
                                    characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(i - 1, a, 0));;
                                    posicionesOcupadas.Remove(characterWall);
                                    characterWall.positionInt.x += 1;
                                    Vector3 posicionCharacter = characterWall.transform.position;
                                    characterWall.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                                    posicionesOcupadas.Add(characterWall);
                                    
                                }
                            }
                        }
                    }
                    
                } 
            }   
        }
    }



}
