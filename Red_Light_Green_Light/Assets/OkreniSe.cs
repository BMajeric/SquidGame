using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkreniSe : MonoBehaviour
{ 
    bool okrenut = true;
    int counter=0;
    void Start(){
        StartCoroutine(waiter());
    }

    IEnumerator waiter(){
        while(counter<10){
        //Rotate 90 deg
        transform.Rotate(new Vector3(0, 180, 0));


        //Wait for 4 seconds
        yield return new WaitForSeconds(4);

        //Rotate 40 deg
        transform.Rotate(new Vector3(0, -180, 0));
      
        //Wait for 2 seconds
        yield return new WaitForSeconds(4);
        counter++;
        if(okrenut == true){
            okrenut =false;
        }else{
            okrenut = true;
        }
        }
    }
    public bool jeOkrenut (){
        return okrenut;
    }
}



/*public Transform otherObject;
 public float speed;
 
 void Update(){
    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1 * Time.deltaTime);
    transform.rotation;
 }*/
 
   /* {float smooth = 5.0f;
    float tiltAngle = 180.0f;

    void Update()
    {
        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundY = Input.GetAxis("Horizontal") * tiltAngle;
      

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, tiltAroundY, 0);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
    }}*/

    
  /* {float smooth = 100.0f;
    float tiltAroundY = 180.0f;
    void RotateTo(){
         float smooth = 100.0f;
         float tiltAroundY = 180.0f;
        Quaternion target = Quaternion.Euler(0, tiltAroundY, 0);
        // Dampen towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target,  Time.deltaTime * smooth);

    }
    void RotateFrom(){
      transform.Rotate(new Vector3(0, 180, 0));

    }

  
    void Update()
    {

        RotateTo();
       
       

    }}*/
    
 
