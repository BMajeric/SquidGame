using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FellDown : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;
    [SerializeField] private Image[] hearts;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Globals.lives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       player.transform.position = respawn.transform.position;

      
       Globals.lives--;
       if(Globals.lives == 0){
            SceneManager.LoadScene("GameOver");
       }
       for (int i = 0; i < hearts.Length; i++)
       {
           if (i < Globals.lives)
           {
               hearts[i].enabled = true;
           }
           else
           {
               hearts[i].enabled = false;
           }
        }
    }
}
