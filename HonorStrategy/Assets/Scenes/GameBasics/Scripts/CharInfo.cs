using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    public Vector3Int position;
    public int color;
    public int health;
    public int damage;

    public Sprite[] sprites;

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

}
