using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public GameObject player;

    public Transform pos1_old;
    public Transform pos2_old;
    public Transform pos3_old;
    public Transform pos4_old;
    int old_lives = Globals.lives;


    private bool first = true;
    private Vector3 distance1;
    private Vector3 distance2;
    private Vector3 distance3;
    private Vector3 distance4;
    private bool left = false;

    // Start is called before the first frame update
    void Start()
    {
        distance1 = pos3.transform.position - pos1.transform.position;
        distance2 = pos4.transform.position - pos1.transform.position;
        distance3 = pos4.transform.position - pos2.transform.position;
        distance4 = pos3.transform.position - pos2.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
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

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
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

        }


        if(old_lives != Globals.lives){
            pos1.transform.position = pos1_old.transform.position;
            pos2.transform.position = pos2_old.transform.position;
            pos3.transform.position = pos3_old.transform.position;
            pos4.transform.position = pos4_old.transform.position;
            first = true;
        }

    }
}
