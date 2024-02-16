using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharInfo : MonoBehaviour
{
    public Vector3Int position;
    public int color;
    public int health;
    public int damage;

    public Sprite[] sprites;

    public Sprite wallSprite;


    void Start()
    {
        color = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[color];
    }


    public void Personaje(Vector3Int pos, int col, int hea, int dam)
    {
        position = pos;
        color = col;
        health = hea;
        damage = dam;
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
                .Where(c => c.position.x == characterIn.position.x)
                .ToList();


            for( int y = 0; y < sameLineCharacters.Count; y++)
            {
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.position == new Vector3Int(characterIn.position.x, characterIn.position.y + 1, characterIn.position.z));;

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
                characterAp = posicionesOcupadas.FirstOrDefault(c => c.position == new Vector3Int(characterIn.position.x, characterIn.position.y - 1, characterIn.position.z));;

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

                    int order = ((12 - (character.position.y)) * 10) + (12 -(character.position.x));

                    character.GetComponent<SpriteRenderer>().sortingOrder = order;

                    character.damage = 0;
                }
            }          
        }
        
/*
        if (sameLineCharacters.Count > 5)
        {

            int order = ((12 - (characterIn.position.y)) * 10) + (12 -(characterIn.position.x));
            GetComponent<SpriteRenderer>().sprite = wallSprite;
            GetComponent<SpriteRenderer>().sortingOrder = order;
        }

        */
    }

}
