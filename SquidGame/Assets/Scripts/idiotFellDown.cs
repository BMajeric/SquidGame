using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class idiotFellDown : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
       player.transform.position = respawn.transform.position;

      
       Globals.lives--;
       if(Globals.lives == 0){
            SceneManager.LoadScene("GameOver");
       }
    }
}
