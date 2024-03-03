using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    static public int turnosPartida = 3;

    private List<CharInfo> posPlayer = new List<CharInfo>();
    private List<CharInfo> posEnemy = new List<CharInfo>();

    void Start()
    {
        // Crear los dos campos para cada jugador llamando a gridCreation

        // guardar las listas de characters que provienen de gridCreation

        //llamar a la comprobacion de posiciones de char1Info

    }


    void Update()
    {
        //crar una forma de iteracion para los turnos
    }
}
