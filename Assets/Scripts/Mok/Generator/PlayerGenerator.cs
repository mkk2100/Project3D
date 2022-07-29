using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Instantiate(player,transform.position,Quaternion.identity);
    }
}
