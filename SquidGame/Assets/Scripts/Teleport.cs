using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    // Creating the positions field that holds the teleporatation positions for each tile
    private Transform[] positions = new Transform[10];

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

    // Creating the tiles field that holds the tile game objects (used for the highlighting option)
    private GameObject[] tiles = new GameObject[10];

    [SerializeField] private GameObject left1;
    [SerializeField] private GameObject right1;
    [SerializeField] private GameObject left2;
    [SerializeField] private GameObject right2;
    [SerializeField] private GameObject left3;
    [SerializeField] private GameObject right3;
    [SerializeField] private GameObject left4;
    [SerializeField] private GameObject right4;
    [SerializeField] private GameObject left5;
    [SerializeField] private GameObject right5;

    // Materials for highlighted and non highlighted tiles
    [SerializeField] private Material BaseMaterial;
    [SerializeField] private Material HighlightMaterial;

    // Variable used for determinining if lives were lost (the player died)
    int old_lives = Globals.lives;

    // Determines the row of glass tiles, used for iteration through fields
    private int step = 0;

    // Which tile is highlighted:
    //      0 -> left
    //      1 -> right
    private int side = 0;

    // Delay imput variables
    private float time = 0f;
    private float timeDelay = 2f;
    private float chooseTime = 0f;
    private float chooseTimeDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize positions field
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

        // Initialize tiles field
        tiles[0] = left1;
        tiles[1] = right1;
        tiles[2] = left2;
        tiles[3] = right2;
        tiles[4] = left3;
        tiles[5] = right3;
        tiles[6] = left4;
        tiles[7] = right4;
        tiles[8] = left5;
        tiles[9] = right5;
    }

    // Update is called once per frame
    void Update()
    {
        time = time + 1f * Time.deltaTime;
        chooseTime = chooseTime + 1f * Time.deltaTime;
        if (chooseTime>= chooseTimeDelay && step < 10)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                side = 0;
                chooseTime = 0f;
                Debug.Log(side);

                // Hihlight the next left tile
                tiles[step].GetComponent<MeshRenderer>().material = HighlightMaterial;
                tiles[step+1].GetComponent<MeshRenderer>().material = BaseMaterial;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                side = 1;
                chooseTime = 0f;
                Debug.Log(side);

                // Hihlight the next right tile
                tiles[step + 1].GetComponent<MeshRenderer>().material = HighlightMaterial;
                tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
            }

        }

        if (time >= timeDelay && Input.GetKeyDown(KeyCode.Space))
        {
            if (side == 0)
            {
                if(step >= 10 ){
                    player.transform.position = finish.transform.position;
                    Invoke("loadaj",3 );
                }else{
                    player.transform.position = positions[step].transform.position;
                    tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
                    step += 2;
                    time = 0f;
                    Debug.Log(step);
                    Debug.Log(player.transform.position);
                }
            }
            else if (side == 1)
            {
                if(step >= 10 ){
                    player.transform.position = finish.transform.position;
                    Invoke("loadaj",3 );
                }else{
                    player.transform.position = positions[step+1].transform.position;
                    tiles[step + 1].GetComponent<MeshRenderer>().material = BaseMaterial;
                    step += 2;
                    time = 0f;
                    Debug.Log(step);
                    Debug.Log(player.transform.position);
                }
            }


        }


        if(old_lives != Globals.lives){
            tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
            tiles[step + 1].GetComponent<MeshRenderer>().material = BaseMaterial;
            step = 0;
            old_lives = Globals.lives;
        }

    }



    private void loadaj(){
        SceneManager.LoadScene("StartMenu");
    }
}
