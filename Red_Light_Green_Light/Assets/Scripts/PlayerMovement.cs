using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 lastPos;
    private float speed = 6f;
    private bool moving;

    void Moved(){

        var displacement = transform.position - lastPos;
        lastPos = transform.position;
    
        if(displacement.magnitude > 0.01)  // return true if char moved 1mm
        {
            moving = true;
        } else {
            moving = false;
        }
    }

    public bool isMoving(){
        return moving;
    }
    
    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        Moved(); 

    }
    
}
