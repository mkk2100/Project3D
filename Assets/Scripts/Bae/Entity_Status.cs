using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity_Status
{
    [SerializeField]
    public Entity_Status(float _hp, float _atk, float _speed)
    {
        hp = _hp;
        atk = _atk;
        speed = _speed;
    }
    [SerializeField]
    private float hp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float speed;

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
}

