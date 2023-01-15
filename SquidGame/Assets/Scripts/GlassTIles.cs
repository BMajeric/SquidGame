using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTIles : MonoBehaviour
{
    private GameObject[] tiles = new GameObject[10];
    private BoxCollider[] colliders = new BoxCollider[10];
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        tiles[0] = GameObject.Find("Cube (10)");
        tiles[1] = GameObject.Find("Cube (9)");
        tiles[2] = GameObject.Find("Cube (8)");
        tiles[3] = GameObject.Find("Cube (11)");
        tiles[4] = GameObject.Find("Cube (7)");
        tiles[5] = GameObject.Find("Cube (6)");
        tiles[6] = GameObject.Find("Cube (1)");
        tiles[7] = GameObject.Find("Cube (2)");
        tiles[8] = GameObject.Find("Cube (4)");
        tiles[9] = GameObject.Find("Cube (3)");

        for(int i = 0; i < 10; i++){
            colliders[i] = tiles[i].GetComponent<BoxCollider>();
        }
        
        for(int i = 0; i < 10; i +=2){
            int random_number = Random.Range(0,2);
            if(random_number == 1){
                colliders[i].isTrigger = true;
                colliders[i+1].isTrigger = false;
            }else{
                colliders[i].isTrigger = false;
                colliders[i+1].isTrigger = true;
            }
        }

        for(int i = 0; i < 10; i++){
            if(!colliders[i].isTrigger){
                if(i % 2 == 0){
                    Debug.Log(" Desno");
                }else{
                    Debug.Log("Lijevo");
                }    
            }
           
        }
    }

   
}
