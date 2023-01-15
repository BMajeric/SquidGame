using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLineIsland : MonoBehaviour
{
   public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("StartMenu");
        //Invoke("loadaj",3 );
        
    }
    private void loadaj(){
        SceneManager.LoadScene("StartMenu");
        /*
        PlayerMovement a = other.GetComponent<PlayerMovement>();
        if(a != null){
           SceneManager.LoadScene("StartMenu");
        }
        */
    }
}
}
