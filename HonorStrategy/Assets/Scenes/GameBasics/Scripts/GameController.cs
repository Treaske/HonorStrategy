using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GridCreation gridCreation;

    static public int turnosPartida = 3;

    private List<CharInfo> playerPos = new List<CharInfo>();
    private List<CharInfo> enemyPos = new List<CharInfo>();

    void Start()
    {
        //Crear Campo
        gridCreation.GenerateGrid();

        //Crear ejercitos
        gridCreation.GenerateCharactersPlayer();
        gridCreation.GenerateCharactersEnemy();
        
        //Comprobar posiciones iniciales

        //llamar a la comprobacion de posiciones de char1Info

    }


    void Update()
    {
        
    }
}
