using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// 엔터티들의 공통 스테이터스 클래스

[System.Serializable]
public class Entity_Status
{
    [SerializeField]
    public Entity_Status(float _hp, float _atk, float _speed, float _jumpForce)
    {
        hp = _hp;
        atk = _atk;
        speed = _speed;
        jumpForce = _jumpForce;
    }
    [SerializeField]
    private float hp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    public float Atk
    {
        get { return atk; }
        set { atk = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
}

