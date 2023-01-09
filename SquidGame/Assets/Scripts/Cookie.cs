using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cookie : MonoBehaviour
{

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