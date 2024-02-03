using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMenu : MonoBehaviour
{

    public void Jugar(string sceneJuego){
        SceneManager.LoadScene(sceneJuego);
   }

   public void Salir(){
        Application.Quit();
        Debug.Log("juego cerrado");
   }
}
