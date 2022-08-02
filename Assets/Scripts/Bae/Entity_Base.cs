using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// �÷��̾�, ���� �� ����Ƽ���� �ൿ�� ���� ���̽� ��ũ��Ʈ
public abstract class Entity_Base : MonoBehaviour
{
    [SerializeField]
    protected Entity_Status entityStatus;

    public abstract bool Move();
    public abstract bool Move(float _speed);
    public abstract bool Rotation(Vector2 _vec2);
    public abstract void Jump();
    public abstract void Jump(float _jumpForce);
}
