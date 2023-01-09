using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum RobotStates{ Counting, Looking}

public class Robot : MonoBehaviour
{
    [SerializeField] private AudioSource jingleSource; 
    private float timer = 5f;
    private float currentTimer;
    private Animator animator;
    //private OkreniSe lutka;
    private PlayerMovement player;
    private RobotStates currentState = RobotStates.Counting;

    // Start is called before the first frame update
    void Start()
    {
        //lutka = FindObjectOfType<OkreniSe>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
        currentTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            StateMachine();
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
                Destroy(player.gameObject);
                SceneManager.LoadScene("GameOver");
            }

        }else{
            currentTimer = timer;
            animator.SetTrigger("Look");

            jingleSource.Play();
            currentState = RobotStates.Counting;
        }
    }
}
