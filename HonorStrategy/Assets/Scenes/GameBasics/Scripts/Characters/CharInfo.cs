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
    public int stage;
    
    public Sprite[] sprites;

    public Sprite[] wallSprite;

    public Sprite[] atackSprite;


    void Start()
    {
        if(colorInt <= 2)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[colorInt];
        }
    }


    public void Personaje(Vector3Int pos, int col, int hea, int dam, int mod, int stat, int stag)
    {
        positionInt = pos;
        colorInt = col;
        health = hea;
        damage = dam;
        modo = mod;
        status = stat;
        stage = stag;
    }

    public void CheckNewPosition(List<CharInfo> posicionesOcupadas, CharInfo selectedCharacter)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CharInfo characterAp = selectedCharacter;

            List<CharInfo> matchedTilesWall = new List<CharInfo>();
            List<CharInfo> matchedTilesAtack = new List<CharInfo>();

            matchedTilesWall.Add(selectedCharacter);

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.x == selectedCharacter.positionInt.x)
                .ToList();

            //------------------------------------------------------creacion de wall-----------------------------------------------------------------------------------------------------
            for( int y = selectedCharacter.positionInt.y; y < sameLineCharacters.Count; y++)
            {
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(selectedCharacter.positionInt.x, y + 1, selectedCharacter.positionInt.z));;

                if(characterAp != null)
                {
                    if(characterAp.colorInt == selectedCharacter.colorInt)
                    {
                        matchedTilesWall.Add(characterAp);
                    } else {
                        y = sameLineCharacters.Count;
                    }   
                } else{
                    y = sameLineCharacters.Count;
                }
            }    

            for( int i = selectedCharacter.positionInt.y; i > 0; i--)
            {
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(selectedCharacter.positionInt.x, i - 1, selectedCharacter.positionInt.z));;

                if(characterAp != null)
                {
                    if(characterAp.colorInt == selectedCharacter.colorInt)
                    {
                        matchedTilesWall.Add(characterAp);
                    } else {
                        i = 0;
                    }   
                } else{
                    i = 0;
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
            
            HandleNewPositionWall(posicionesOcupadas);
            
            //------------------------------------------------------creacion de wall-----------------------------------------------------------------------------------------------------
            //------------------------------------------------------creacion de atack-----------------------------------------------------------------------------------------------------

            for( int f = selectedCharacter.positionInt.x; f < (selectedCharacter.positionInt.x + 3); f++)
            {
                //Debug.Log("Hola " + selectedCharacter.positionInt);
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(f, selectedCharacter.positionInt.y, selectedCharacter.positionInt.z));;
                //Debug.Log("Hola: " + characterAp.positionInt);
                if(characterAp != null)
                {
                    if(characterAp.colorInt == selectedCharacter.colorInt)
                    {
                        //Debug.Log("Hola ap " + characterAp.colorInt);
                        //Debug.Log("Hola selec " + selectedCharacter.colorInt);
                        matchedTilesAtack.Add(characterAp);
                    }  
                }
            }    

            //------------------------------------------------------creacion de atack-----------------------------------------------------------------------------------------------------

            if (matchedTilesAtack.Count > 2)
            {
                foreach (var character in matchedTilesAtack)
                {
                    //Debug.Log("Hola");
                    character.GetComponent<SpriteRenderer>().sprite = atackSprite[character.colorInt];

                    int order = ((12 - (character.positionInt.y)) * 10) + (12 -(character.positionInt.x));

                    character.GetComponent<SpriteRenderer>().sortingOrder = order;

                    character.modo = 1;
                    character.stage = 3;
                }
                HandleNewPositionAttack(posicionesOcupadas, matchedTilesAtack);
            }

        }
        
    }

    void HandleNewPositionWall(List<CharInfo> posicionesOcupadas)
    {
        int orderLayer = 0;
        for (int a = 0; a < 12; a++)
        {

            //Comprobamos columna entera para saber si hay dos muros que puedan unirse y mejorarse
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
                            if((characterIn.modo == 0 && characterIn.status < characterWall.status) || characterIn.modo == 2)
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
                                    //Debug.Log("Hola desde if: " + characterWall.positionInt);
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

    void HandleNewPositionAttack(List<CharInfo> posicionesOcupadas, List<CharInfo> matchedTilesAtack)
    {
        
        CharInfo characterAp = matchedTilesAtack[2];
        //Debug.Log("Hola desde: " + matchedTilesAtack[2].positionInt);

        List<CharInfo> sameColumnCharacters = posicionesOcupadas
            .Where(c => c.positionInt.y == matchedTilesAtack[2].positionInt.y)
            .ToList();

        for (int i = (12 - (sameColumnCharacters.Count - 3)); i < 12; i++)
        {
            characterAp = posicionesOcupadas.FirstOrDefault(c => c.positionInt == new Vector3Int(i + 1, characterAp.positionInt.y, characterAp.positionInt.z));;
            //Debug.Log("Hola desde for: " + characterAp.positionInt);
            if(characterAp != null && characterAp.modo > matchedTilesAtack[2].modo)
            {
                posicionesOcupadas.Remove(characterAp);

                characterAp.positionInt.x -= 3;
                Vector3 posicionChar;

                for(int z = 2; z >= 0; z--)
                {
                    posicionesOcupadas.Remove(matchedTilesAtack[z]);
                    matchedTilesAtack[z].positionInt.x += 1;
                    posicionChar = matchedTilesAtack[z].transform.position;
                    matchedTilesAtack[z].transform.position = new Vector3(posicionChar.x + 0.5f, posicionChar.y + 0.25f, posicionChar.z);;
                    posicionesOcupadas.Add(matchedTilesAtack[z]);
                }

                posicionChar = characterAp.transform.position;
                characterAp.transform.position = new Vector3(posicionChar.x - 1.5f, posicionChar.y - 0.75f, posicionChar.z);;
                posicionesOcupadas.Add(characterAp);

            }
        }

    }
}