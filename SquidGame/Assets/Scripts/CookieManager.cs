using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CookieManager : MonoBehaviour
{
    private Part[] parts;
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        parts = GetComponentsInChildren<Part>();

        foreach (var item in parts)
        {
            item.initialize(this);
        }
    }


    public void CheckForWin(){
        foreach (var item in parts)
        {
            if(!item.Done) return;
        }
        SceneManager.LoadScene("GT game");
    }
}
