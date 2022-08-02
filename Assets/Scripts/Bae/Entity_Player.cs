using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// �÷��̾��� �ൿ ��ũ��Ʈ
public class Entity_Player : Entity_Base
{
    Animator animator;
    Rigidbody rigidbody;

    protected float angle;
    protected float turnSpeed;

    private bool isGround;
    private void Awake()
    {
        turnSpeed = 10;
        entityStatus = new Entity_Status(3, 1, 3, 300);
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // ���ڷ� �ƹ��͵� �ѱ��� ������ �������ͽ� ���� �̵��ӵ��� �̵�
    public override bool Move()
    {
        return Move(entityStatus.Speed);
    }

    // �̵� ��ũ��Ʈ
    public override bool Move(float _speed)
    {
        if(_speed == 0.0f) // ���ڷ� 0�� ������ �ִϸ��̼� �����ϰ� False ����
        {
            animator.SetBool("isWalking", false);    // �ȱ� �ִϸ��̼� ��� ��
            return false;
        }

        transform.position += transform.forward * _speed * Time.deltaTime;
        animator.SetBool("isWalking", true);    // �ȱ� �ִϸ��̼� ��� ��

        return true;
    }

    // ���⼳�� + ȸ�� (�� �Լ��� ���� ������ �ʿ䰡 �ֳ� �; ����)
    public override bool Rotation(Vector2 _vec2)
    {
        angle = Mathf.Atan2(_vec2.x, _vec2.y);
        angle = Mathf.Rad2Deg * angle;

        Quaternion targetPosition = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition, turnSpeed * Time.deltaTime);

        return true;
    }

    public override void Jump()
    {
        Jump(entityStatus.JumpForce);
    }

    public override void Jump(float _jumpForce)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.red);
        Debug.Log("CallJump");

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
        {
            if (hit.transform.CompareTag("Ground")) isGround = true;
            else isGround = false;
        }
        else isGround = false;

        if (isGround == true && rigidbody.velocity.y <= 0)
        {
            rigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }
}
