using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCube : MonoBehaviour
{
    readonly float LEFTRIGHT = 11.0f;
    readonly float TOP = 7.0f;
    readonly float BOTTOM = 5.0f;

    public float moveSpeed; 

    private Vector2 speed;

    private void Start()
    {
        SetSpeed(moveSpeed);
        SetPosition();
    }

    private void SetSpeed(float moveSpeed)
    {
        speed = new Vector2(moveSpeed, -moveSpeed);
    }

    private void Update()
    {
        Spin();        
        StartCoroutine(Reflex());
    }

    private void SetPosition()
    {
        float x = Random.Range(-11, 11);
        float y = Random.Range(-5, 7);
        this.transform.position = new Vector2(x, y);
    } 

    private IEnumerator Reflex()
    {
        Vector3 delta = speed * Time.deltaTime;
        Vector3 newPos = this.transform.position + delta;

        if(newPos.x < -LEFTRIGHT)
        {
            newPos.x = - LEFTRIGHT;
            speed.x *= -1;
        }
        else if(newPos.x > LEFTRIGHT)
        {
            newPos.x = LEFTRIGHT;
            speed.x *= -1;
        }
        else if(newPos.y > TOP)
        {
            newPos.y = TOP;
            speed.y *= -1;
        }
        else if(newPos.y < -BOTTOM)
        {
            newPos.y = -BOTTOM;
            speed.y *= -1;
        }

        transform.position = newPos;

        yield return new WaitForEndOfFrame();
    }

    private void Spin()
    {
        transform.Rotate(new Vector3(1,1) * Time.deltaTime * 150);
    }





    // [Range(1,10)]
    // public int moveSpeed;
    // private Rigidbody cubeRigidbody;
    // private float randomX, randomY;
    
    // private void Start()
    // {
    //     Initialize();
    //     SetPosition();
    //     Reflex();
    // }

    // private void Update()
    // {
    //     Spin();
    // }

    // private void Initialize()
    // {
    //     cubeRigidbody = GetComponent<Rigidbody>();
    // }

    // private void SetPosition()
    // {
    //     randomX = Random.Range(-10f, 10f);
    //     randomY = Random.Range(-4f, 6f);
    //     Vector2 direction = new Vector2(randomX, randomY);
    //     this.transform.position = direction;
    // }

    // private void Reflex()
    // {
    //     cubeRigidbody.velocity = new Vector3(moveSpeed, moveSpeed, 0);
    // }
}
