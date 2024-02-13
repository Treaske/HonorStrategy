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

    public void CheckNewPosition(List<CharInfo> posicionesOcupadas)
    {
        CharInfo characterIn = GetComponent<CharInfo>();

        //HashSet matchedTiles = new HashSet();

        //matchedTiles.Add(characterIn);

        List<CharInfo> sameLineCharacters = posicionesOcupadas
            .Where(c => c.position.x == characterIn.position.x)
            .ToList();

        /*
        int cont = 0;

        for( int y = 0; y < 12; y++)
        {
            CharInfo characterAp = ;

            if()
            {

            }
            characterIn = posicionesOcupadas
                .Where(c => c.position.x == characterIn.position.x)
        }
        */

        if (sameLineCharacters.Count > 5)
        {

            int order = ((12 - (characterIn.position.y)) * 10) + (12 -(characterIn.position.x));
            GetComponent<SpriteRenderer>().sprite = wallSprite;
            GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }

}
