using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public class Player_Contoller : MonoBehaviour
    {
        [SerializeField]
        private Entity_Status entityStatus;
        
        private Animator animator;
        private Vector2 movingInput;
        private float angle;
        private float turnSpeed = 10;
        private Quaternion targetPosition;

        private void Start()
        {
            entityStatus = new Entity_Status(3,1,3);
            animator = GetComponent<Animator>();
        }
        void FixedUpdate()
        {
            // if (Input.GetKey(KeyCode.A))
            // {
            //     transform.Translate(-entityStatus.Speed * Time.deltaTime, 0, 0);
            //     // playerAnimator.SetBool("isWalking", true);
            // }
            // else if (Input.GetKey(KeyCode.D))
            // {
            //     transform.Translate(entityStatus.Speed * Time.deltaTime, 0, 0);
            //     // playerAnimator.SetBool("isWalking", true);
            // }
            // if (Input.GetKey(KeyCode.S))
            // {
            //     transform.Translate(0, 0, -entityStatus.Speed * Time.deltaTime);
            //     // playerAnimator.SetBool("isWalking", true);
            // }
            // else if (Input.GetKey(KeyCode.W))
            // {
            //     transform.Translate(0, 0, entityStatus.Speed * Time.deltaTime);
            //     // playerAnimator.SetBool("isWalking", true);
            // }
            // // else
            // //     playerAnimator.SetBool("isWalking", false);

            SetInput();
            if(Mathf.Abs(movingInput.x) < 1 && Mathf.Abs(movingInput.y) < 1)    // 플레이어의 입력이 없습니까?
            {
                animator.SetBool("isWalking", false);   // 걷기 애니메이션 재생 끔
                return;
            }
            SetDirection();
            SetRotation();
            Move();
        }

        // 플레이어 입력 설정
        private void SetInput()
        {
            movingInput.x = Input.GetAxisRaw("Horizontal");
            movingInput.y = Input.GetAxisRaw("Vertical");
        }

        // 플레이어 입력에 따라 캐릭터 바라보는 방향 설정
        private void SetDirection()
        {
            angle = Mathf.Atan2(movingInput.x, movingInput.y);
            angle = Mathf.Rad2Deg * angle;
        }

        // 플레이어 입력에 따라 캐릭터 회전 설정
        private void SetRotation()
        {
            targetPosition = Quaternion.Euler(0,angle,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition, turnSpeed * Time.deltaTime);
        }

        // 플레이어 입력에 따라 캐릭터 따라 움직임 설정
        private void Move()
        {
            transform.position += transform.forward * entityStatus.Speed * Time.deltaTime;
            animator.SetBool("isWalking", true);    // 걷기 애니메이션 재생 켬
        }
    }
}