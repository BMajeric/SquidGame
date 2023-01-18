using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum RobotStates{ Counting, Looking}

public class Robot : MonoBehaviour
{
    [SerializeField] private AudioSource jingleSource; 
    [SerializeField] private AudioSource deathSound; 
    private float timer = 5f;
    private float currentTimer;
    private Animator animator;
    private bool respawned = false;
    //private OkreniSe lutka;
    private PlayerMovement player;
    private RobotStates currentState = RobotStates.Counting;

    [SerializeField] private Transform pl;
    [SerializeField] private Transform respawn;

    [SerializeField] private Image[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        //lutka = FindObjectOfType<OkreniSe>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        currentTimer = timer;
        Globals.lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            StateMachine();
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

    private void StateMachine(){
        if(currentState ==  RobotStates.Counting ){
            Count();
        }else if(currentState == RobotStates.Looking){
            Look();
        }
    }

    private void Count(){
        if(!jingleSource.isPlaying){
            animator.SetTrigger("Look");
            currentState = RobotStates.Looking;
        }
    }

    private void Look(){
        if(currentTimer > 0){
            currentTimer = currentTimer-Time.deltaTime;
            
            if(player.isMoving()){
                if(Globals.lives > 0){
                    if(respawned == false){
                        Globals.lives--;
                        deathSound.Play();
                    }
                    respawned = true;    
                    pl.transform.position = respawn.transform.position;
                }
            }
            if(Globals.lives == 0){
                Destroy(player.gameObject);
                SceneManager.LoadScene("GameOver");
            }

        }else{
            currentTimer = timer;
            animator.SetTrigger("Look");
            respawned = false;
            jingleSource.Play();
            currentState = RobotStates.Counting;
        }
    }
}
