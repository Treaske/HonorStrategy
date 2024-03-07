using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class InitialPosition : MonoBehaviour
{
    public void HandleNewPositionsPlayer(List<CharInfo> posicionesOcupadas, Sprite[] wallSprite)
    {
        //recorre todas las posiciones
        for (int c = 0; c < 12; c++)
        {
            List<CharInfo> sameColumnCharacters = posicionesOcupadas
                .Where(a => a.positionInt.y == c)
                .ToList();

            for (int l = 12; l > (12 - sameColumnCharacters.Count); l--)
            {
                CharInfo charIn = sameColumnCharacters.FirstOrDefault(a => a.positionInt == new Vector3Int(l, c, 0));;

                HashSet<CharInfo> matchedTilesWall = new HashSet<CharInfo>();
                matchedTilesWall.Add(charIn);

                List<CharInfo> sameLineCharacters = posicionesOcupadas
                    .Where(a => a.positionInt.x == l)
                    .ToList();

                //guarda todos los characters del mismo color primero a derecha, despues a izquierda
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
                //Comprueba si hay mas de tres characters del mismo color

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
        
        HandleNewPositionWallPlayer(posicionesOcupadas, wallSprite);
    }

    public void HandleNewPositionWallPlayer(List<CharInfo> posicionesOcupadas, Sprite[] wallSprite)
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
                            //comprobacion para que el se coloce segun el modo (muro, ataque, normal)
                            if((characterIn.status < characterWall.status) || characterIn.modo > characterWall.modo)
                            {
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
                                
                                //Comprobaciones para sumar los muros si son del mismo tipo
                            } else if ((characterIn.modo == 0 && characterIn.status == characterWall.status) && characterIn.status != 2)
                            {
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

    //Cambiar codigo para que se pueda hacer en uno solo-------------------------------------------------------------------------------------------------------------------------

    public void HandleNewPositionsEnemy(List<CharInfo> posicionesOcupadas, Sprite[] wallSprite)
    {
        //recorre todas las posiciones
        for (int c = 0; c < 12; c++)
        {
            List<CharInfo> sameColumnCharacters = posicionesOcupadas
                .Where(a => a.positionInt.y == c)
                .ToList();

            for (int l = 14; l < (14 + sameColumnCharacters.Count); l++)
            {
                CharInfo charIn = sameColumnCharacters.FirstOrDefault(a => a.positionInt == new Vector3Int(l, c, 0));;

                HashSet<CharInfo> matchedTilesWall = new HashSet<CharInfo>();
                matchedTilesWall.Add(charIn);

                List<CharInfo> sameLineCharacters = posicionesOcupadas
                    .Where(a => a.positionInt.x == l)
                    .ToList();

                //guarda todos los characters del mismo color primero a derecha, despues a izquierda
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
                //Comprueba si hay mas de tres characters del mismo color

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
        
        HandleNewPositionWallEnemy(posicionesOcupadas, wallSprite);
    }

    public void HandleNewPositionWallEnemy(List<CharInfo> posicionesOcupadas, Sprite[] wallSprite)
    {
        int orderLayer = 0;
        for (int a = 0; a < 12; a++)
        {

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.y == a)
                .ToList();

            if (sameLineCharacters.Count > 0)
            {
                for (int l = (14 + sameLineCharacters.Count); l > 14; l--)
                {
                    CharInfo characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l, a, 0));;
                    CharInfo characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l - 1, a, 0));;

                    if (characterWall != null && characterIn != null)
                    {
                        if(characterWall.positionInt.x != 14 && characterWall.modo == 0)
                        {
                            //Si el character de delante tiene un modo mayor o un estatus menor se cambiara hacia detras
                            if((characterIn.status < characterWall.status) || characterIn.modo > characterWall.modo)
                            {
                                //Debug.Log("Hola desde if: " + characterWall.positionInt);
                                posicionesOcupadas.Remove(characterIn);
                                posicionesOcupadas.Remove(characterWall);

                                characterIn.positionInt.x = l;
                                Vector3 posicionCharacter = characterIn.transform.position;
                                characterIn.transform.position = new Vector3(posicionCharacter.x + 0.5f, posicionCharacter.y + 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterIn.positionInt.y)) * 10) + (12 -(characterIn.positionInt.x));
                                characterIn.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                characterWall.positionInt.x = l - 1;
                                posicionCharacter = characterWall.transform.position;
                                characterWall.transform.position = new Vector3(posicionCharacter.x - 0.5f, posicionCharacter.y - 0.25f, posicionCharacter.z);;
                                orderLayer = ((12 - (characterWall.positionInt.y)) * 10) + (12 -(characterWall.positionInt.x));
                                characterWall.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;

                                posicionesOcupadas.Add(characterWall);
                                posicionesOcupadas.Add(characterIn);
                                
                                //Comprobaciones para sumar los muros si son del mismo tipo
                            } else if ((characterIn.modo == 0 && characterIn.status == characterWall.status) && characterIn.status != 2)
                            {
                                //Debug.Log("Contador : " + (14 + sameLineCharacters.Count));
                                posicionesOcupadas.Remove(characterWall);
                                Destroy(characterWall.gameObject);

                                characterIn.status++;
                                characterIn.GetComponent<SpriteRenderer>().sprite = wallSprite[characterIn.status];

                                for(int i = l; i < (13 + sameLineCharacters.Count); i++)
                                {
                                    characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(i + 1, a, 0));;
                                    posicionesOcupadas.Remove(characterWall);
                                    characterWall.positionInt.x -= 1;
                                    Vector3 posicionCharacter = characterWall.transform.position;
                                    characterWall.transform.position = new Vector3(posicionCharacter.x - 0.5f, posicionCharacter.y - 0.25f, posicionCharacter.z);;
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
