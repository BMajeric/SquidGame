using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idiotFellDown : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
       player.transform.position = respawn.transform.position;
    }
}
