using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpriteRandom : MonoBehaviour
{

    public Sprite[] sprites;
    public int charColor;

    void Start()
    {
        charColor = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[charColor];
    }

}
