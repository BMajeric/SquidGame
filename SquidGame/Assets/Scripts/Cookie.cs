using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cookie : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

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

    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponent<Renderer>().material.color = Color.red;
            if (!Globals.mistake)
            {
                Globals.lives--;
                Globals.mistake = true;
            }
            if (Globals.lives == 0)
            {
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

            Invoke("Restart", 1f);

        }


        Debug.Log("Over");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Globals.mistake = false;
    }

}