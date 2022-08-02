using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// 플레이어의 행동 스크립트
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

    // 인자로 아무것도 넘기지 않으면 스테이터스 상의 이동속도로 이동
    public override bool Move()
    {
        return Move(entityStatus.Speed);
    }

    // 이동 스크립트
    public override bool Move(float _speed)
    {
        if(_speed == 0.0f) // 인자로 0을 받으면 애니메이션 종료하고 False 리턴
        {
            animator.SetBool("isWalking", false);    // 걷기 애니메이션 재생 끔
            return false;
        }

        transform.position += transform.forward * _speed * Time.deltaTime;
        animator.SetBool("isWalking", true);    // 걷기 애니메이션 재생 켬

        return true;
    }

    // 방향설정 + 회전 (두 함수가 굳이 나눠질 필요가 있나 싶어서 병합)
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
