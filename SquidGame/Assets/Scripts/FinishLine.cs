using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Dalgona cookie scene");
        /*
        PlayerMovement a = other.GetComponent<PlayerMovement>();
        if(a != null){
           SceneManager.LoadScene("StartMenu");
        }
        */
    }
}
