using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;
    public Transform pos7;
    public Transform pos8;
    public Transform pos9;
    public Transform pos10;
    public Transform finish;
    public GameObject player;

    private Transform[] positions = new Transform[10];
   

    int old_lives = Globals.lives;

    private int step = 0; 
    private bool teleported = false;
    private bool first = true;
    private Vector3 distance1;
    private Vector3 distance2;
    private Vector3 distance3;
    private Vector3 distance4;
    private bool left = false;

    private float time = 0f;
    private float timeDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        positions[0] = pos1;
        positions[1] = pos2;
        positions[2] = pos3;
        positions[3] = pos4;
        positions[4] = pos5;
        positions[5] = pos6;
        positions[6] = pos7;
        positions[7] = pos8;
        positions[8] = pos9;
        positions[9] = pos10;
        /*
        distance1 = pos3.transform.position - pos1.transform.position;
        distance2 = pos4.transform.position - pos1.transform.position;
        distance3 = pos4.transform.position - pos2.transform.position;
        distance4 = pos3.transform.position - pos2.transform.position;
        */
    }

    // Update is called once per frame
    void Update()
    {
        time = time + 1f * Time.deltaTime;
        if (time >= timeDelay)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                /*
                left = true;
                if (first)
                {
                    player.transform.position = pos1.transform.position;
                    first = false;
                }
                else
                {
                    if (left)
                    {
                        pos1.transform.position += distance1;
                        pos2.transform.position += distance1;
                        player.transform.position = pos1.transform.position;
                        Debug.Log("Na lijevoj idem lijevo");
                    }
                    else
                    {
                        pos1.transform.position += distance4;
                        pos2.transform.position += distance4;
                        player.transform.position = pos1.transform.position;
                        Debug.Log("Na desnoj idem lijevo");
                    }
                }
                */
                if(step >= 10 ){
                    player.transform.position = finish.transform.position;
                    Invoke("loadaj",3 );
                }else{
                    player.transform.position = positions[step].transform.position;
                    step += 2;
                    time = 0f;
                    Debug.Log(step);
                    Debug.Log(player.transform.position);
                }
            

            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
               /*
                left = false;
                if (first)
                {
                    player.transform.position = pos2.transform.position;
                    first = false;
                }
                else
                {
                    if (!left)
                    {
                        pos2.transform.position += distance3;
                        pos1.transform.position += distance3;
                        player.transform.position = pos2.transform.position;

                        Debug.Log("Na desnoj idem desno");
                    }
                    else
                    {
                        pos2.transform.position += distance2;
                        pos1.transform.position += distance2;
                        player.transform.position = pos2.transform.position;
                        Debug.Log("Na lijevoj idem desno");
                    }
                }
                */
                 if(step >= 10 ){
                    player.transform.position = finish.transform.position;
                    Invoke("loadaj",3 );
                }else{
                    player.transform.position = positions[step+1].transform.position;
                    step += 2;
                    time = 0f;
                    Debug.Log(step);
                    Debug.Log(player.transform.position);
                }
            
            }

        }


        if(old_lives != Globals.lives){
           step = 0;
           old_lives = Globals.lives;
        }

    }

    private void loadaj(){
        SceneManager.LoadScene("StartMenu");
    }

}
