using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timer = 30f;
    private float currentTime;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(currentTime);
            timerText.text = span.ToString(@"mm\:ss") + "\nLives: " + Globals.lives;
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
