using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.SFXPlay("Test", AudioManager.instance.SFXLibrary[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}