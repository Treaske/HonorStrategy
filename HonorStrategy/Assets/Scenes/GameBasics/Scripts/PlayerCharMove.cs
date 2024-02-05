using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharMove : MonoBehaviour
{
   
   private int direction = 1;
   private Vector3 movement;

    void Update()
    {
        //2 * direction
        movement = new Vector3(0f, 0f, 0f);
        transform.position = transform.position + movement * Time.deltaTime;
    }
}
