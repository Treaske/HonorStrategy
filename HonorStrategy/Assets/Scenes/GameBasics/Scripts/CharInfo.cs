using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharInfo : MonoBehaviour
{
    public Vector3Int positionInt;
    public int colorInt;
    public int health;
    public int damage;
    public int modo;
    public int status;

    public Sprite[] sprites;

    public Sprite[] wallSprite;


    void Start()
    {
        colorInt = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[colorInt];
    }


    public void Personaje(Vector3Int pos, int col, int hea, int dam, int mod, int stat)
    {
        positionInt = pos;
        colorInt = col;
        health = hea;
        damage = dam;
        modo = mod;
        status = stat;
    }

    public void CheckNewPosition(List<CharInfo> posicionesOcupadas, CharInfo selectedCharacter)
    {
        if (Input.GetMouseButtonDown(1))
        {

            CharInfo characterIn = selectedCharacter;
            CharInfo characterAp = selectedCharacter;

            HashSet<CharInfo> matchedTiles = new HashSet<CharInfo>();

            matchedTiles.Add(selectedCharacter);

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.x == characterIn.positionInt.x)
                .ToList();


            for( int y = 0; y < sameLineCharacters.Count; y++)
            {
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(characterIn.positionInt.x, characterIn.positionInt.y + 1, characterIn.positionInt.z));;

                if(characterAp != null)
                {
                    if(characterAp.colorInt == selectedCharacter.colorInt)
                    {
                        matchedTiles.Add(characterAp);
                        characterIn = characterAp;
                    } else {
                        y = sameLineCharacters.Count;
                    }   
                }
            }    

            characterIn = selectedCharacter;

            for( int i = 0; i < sameLineCharacters.Count; i++)
            {
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(characterIn.positionInt.x, characterIn.positionInt.y - 1, characterIn.positionInt.z));;

                if(characterAp != null)
                {
                    if(characterAp.colorInt == selectedCharacter.colorInt)
                    {
                        matchedTiles.Add(characterAp);
                        characterIn = characterAp;
                    } else {
                        i = sameLineCharacters.Count;
                    }
                }
            }

            if (matchedTiles.Count > 2)
            {
                foreach (var character in matchedTiles)
                {
                    character.GetComponent<SpriteRenderer>().sprite = wallSprite[1];

                    int order = ((12 - (character.positionInt.y)) * 10) + (12 -(character.positionInt.x));

                    character.GetComponent<SpriteRenderer>().sortingOrder = order;

                    character.modo = 0;
                    character.colorInt = 3;
                }
            }

            //HandleNewPositionWall(posicionesOcupadas);
        }
    
    }

    void HandleNewPositionWall(List<CharInfo> posicionesOcupadas)
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
                            if((characterIn.modo == 0 && characterIn.status < characterWall.status) || characterIn.modo == 1)
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

/*
    void FuseCharacters(List<CharInfo> posicionesOcupadas, int y)
    {

        for(int x = 12; x > 0; x--)
        {    

            CharInfo characterInWall = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(x, y, 0));;
            CharInfo characterApWall = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(x - 1, y, 0));;

            if(characterApWall != null && characterInWall != null)
            {
                
                if(characterInWall.status == characterApWall.status && (characterInWall.modo == 0 && characterApWall.modo == 0))
                {
                    characterInWall.status++;
                    characterInWall.GetComponent<SpriteRenderer>().sprite = wallSprite[characterInWall.status];

                    posicionesOcupadas.Remove(characterApWall);
                    Destroy(characterApWall.gameObject);

                    List<CharInfo> sameLineCharacters = posicionesOcupadas
                        .Where(c => c.positionInt.y == y)
                        .OrderByDescending(c => c.positionInt.x)
                        .Skip(1)
                        .ToList();

                    foreach (CharInfo character in sameLineCharacters)
                    {
                        character.positionInt.x += 1;
                        Vector3 posicionCharacter = character.transform.position;
                        character.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                        posicionesOcupadas.Add(character);
                    }
                }
            } else{
                x = 0;
            }

        }
    }
*/

   

}
/*
                    CharInfo characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l, a, 0));;
                    CharInfo characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l + 1, a, 0));;

                    if ((characterWall != null && characterIn != null) && (characterWall.positionInt.x != 12 && characterWall.modo == 0))
                    {
                        Debug.Log("Character Wall " + characterWall.positionInt);
                        Debug.Log("Character In " + characterIn.positionInt);
                            
                        posicionesOcupadas.Remove(characterIn);
                        posicionesOcupadas.Remove(characterWall);
                        int orderLayer = 0;

                        for(int x = l; x < 12; x++)
                        {
                            Debug.Log("Character Wall " + x + " " + characterWall.positionInt);
                            
                            Debug.Log("Character In " + x + " " + characterIn.positionInt);
                            if (characterIn.status <= characterWall.status)
                            {
                                    
                                characterIn.positionInt.x = x;
                                Vector3 posicionCharacter = characterIn.transform.position;
                                characterIn.transform.position = new Vector3(posicionCharacter.x - 0.5f, posicionCharacter.y - 0.25f, posicionCharacter.z);;

                                orderLayer = ((12 - (characterIn.positionInt.y)) * 10) + (12 -(characterIn.positionInt.x));
                                characterIn.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                characterWall.positionInt.x = x + 1;
                                posicionCharacter = characterWall.transform.position;
                                characterWall.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;

                                posicionesOcupadas.Add(characterIn);

                                characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(x + 2, a, 0));;

                                posicionesOcupadas.Remove(characterIn);
                            }
                            
                        }

                        orderLayer = ((12 - (characterWall.positionInt.y)) * 10) + (12 -(characterWall.positionInt.x));
                        characterWall.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;
                        posicionesOcupadas.Add(characterWall);
                        
                        int countModeWall = sameLineCharacters.Count(c => c.modo == 0);
                        
                        if(countModeWall >= 2)
                        {
                            FuseCharacters(posicionesOcupadas, a);
                        }
                        }
                        */