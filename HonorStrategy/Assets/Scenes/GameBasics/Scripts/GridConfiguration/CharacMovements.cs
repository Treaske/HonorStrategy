using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacMovements : MonoBehaviour
{
    public Vector3Int startGridPos = new Vector3Int(1, 0, 0); // Posición inicial
    public float moveSpeed = 2f; // Velocidad de movimiento entre posiciones
    public Animator animator; // Referencia al Animator del personaje
    //public List<CharInfo> posPlayer = new List<CharInfo>(); // Lista de posiciones ocupadas por otros personajes
    
    /*
    public void StartMoving() // Método público para iniciar el movimiento
    {
        if (movementCoroutine == null) // Verifica que no haya otra corrutina de movimiento en curso
        {
            movementCoroutine = StartCoroutine(MoveToPosition());
        }
    }
    */
    public /*IEnumerator*/ void MoveToPosition()
    {
        /*
        Vector3Int targetGridPos = startGridPos;
        while (targetGridPos.x < 12)
        {
            // Calcula la siguiente posición
            Vector3Int nextPos = targetGridPos + new Vector3Int(1, 0, 0);

            // Verifica si la siguiente posición está libre
            
            while (posPlayer.Any(c => c.positionInt == nextPos))
            {
                // Actualiza el targetGridPos a la siguiente posición
                targetGridPos = nextPos;

                // Activa la animación de movimiento
                animator.SetBool("isMoving", true);

                // Interpola suavemente entre la posición actual y la siguiente
                Vector3 startPos = transform.position;
                Vector3 endPos = new Vector3(targetGridPos.x, targetGridPos.y, targetGridPos.z);

                float elapsedTime = 0;
                while (elapsedTime < 1 / moveSpeed)
                {
                    transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime * moveSpeed));
                    elapsedTime += Time.deltaTime;
                    yield return null; // Espera al siguiente frame
                }

                // Asegura que la posición final sea exacta
                transform.position = endPos;

                // Desactiva la animación de movimiento
                animator.SetBool("isMoving", false);

                Debug.Log($"Personaje avanzó a la posición {targetGridPos.x}");
            }
        }

        Debug.Log($"Posición final del personaje: {targetGridPos.x}");
        */
    }
}
