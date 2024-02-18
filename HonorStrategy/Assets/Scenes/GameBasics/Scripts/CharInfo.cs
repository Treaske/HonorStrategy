using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharInfo : MonoBehaviour
{
    public Vector3Int positionInt;
    public int color;
    public int health;
    public int damage;
    public int modo;

    public Sprite[] sprites;

    public Sprite wallSprite;


    void Start()
    {
        color = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[color];
    }


    public void Personaje(Vector3Int pos, int col, int hea, int dam, int mod)
    {
        positionInt = pos;
        color = col;
        health = hea;
        damage = dam;
        modo = mod;
    }

    public void CheckNewPosition(List<CharInfo> posicionesOcupadas, CharInfo selectedCharacter)
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mousePos.z = 0;
            //Vector3Int targetGridPos = tilemap.WorldToCell(mousePos);

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
                    if(characterAp.color == selectedCharacter.color)
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
                    if(characterAp.color == selectedCharacter.color)
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
                    character.GetComponent<SpriteRenderer>().sprite = wallSprite;

                    int order = ((12 - (character.positionInt.y)) * 10) + (12 -(character.positionInt.x));

                    character.GetComponent<SpriteRenderer>().sortingOrder = order;

                    character.modo = 0;
                    character.color = 3;
                }
            }

            HandleNewPosition(posicionesOcupadas);   
        }
    
    }

    void HandleNewPosition(List<CharInfo> posicionesOcupadas)
    {
        for (int a = 0; a < 12; a++)
        {

            List<CharInfo> sameLineCharacters = posicionesOcupadas
                .Where(c => c.positionInt.y == a)
                .ToList();

            if (sameLineCharacters.Count > 0)
            {
                for (int l = 12; l > (12 - sameLineCharacters.Count); l--)
                {
                    CharInfo characterWall = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l, a, 0));;
                    //Debug.Log("debug characterWall " + characterWall.positionInt);

                    if (characterWall.positionInt.x != 12 && characterWall.modo == 0)
                    {
                        CharInfo characterIn = sameLineCharacters.FirstOrDefault(c => c.positionInt == new Vector3Int(l + 1, a, 0));;
                            
                        posicionesOcupadas.Remove(characterIn);
                        posicionesOcupadas.Remove(characterWall);
                        int orderLayer = 0;

                        for(int x = l; x < 12; x++)
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

                        orderLayer = ((12 - (characterWall.positionInt.y)) * 10) + (12 -(characterWall.positionInt.x));
                        characterWall.GetComponent<SpriteRenderer>().sortingOrder = orderLayer;
                        posicionesOcupadas.Add(characterWall);
                    }
                } 
            }   
        }
    }

   

}
