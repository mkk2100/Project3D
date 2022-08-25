using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Write by baejinseok

// 플레이어 조작 스크립트
namespace EntitySpace
{
    public class Player_Controller : MonoBehaviour
    {
        Entity_Player entityPlayer;
        Entity_Status entity_Status;
        private Vector2 movingInput;
        protected float attackCool = 1.0f;
        [SerializeField]
        protected float attackCurr = 1.0f;
        


        private void Start()
        {
            entityPlayer = GetComponent<Entity_Player>();
            entity_Status = entityPlayer.entityStatus;
        }
        void FixedUpdate()
        {
            InputMove();
            InputJump();
            InputAttack();
        }
        // 플레이어 입력 설정
        private void InputMove()
        {
            movingInput.x = Input.GetAxisRaw("Horizontal");
            movingInput.y = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(movingInput.x) < 1 && Mathf.Abs(movingInput.y) < 1) entityPlayer.Move(0.0f); // 플레이어의 입력이 없으면 Move() 함수에 인자로 0을 전달하여 애니메이션 오프
            else
            {
                entityPlayer.Move();
                entityPlayer.Rotation(movingInput.x, movingInput.y);
            }
        }

        // 점프 조작
        private void InputJump()
        {
            if (Input.GetKey(KeyCode.Space)) entityPlayer.Jump();
        }

        private void InputAttack()
        {

            if (Input.GetButton("Fire1") && attackCurr >= attackCool)
            {
                Debug.Log("AttackButton");
                bool attackChk = entityPlayer.Attack();
                attackCurr = 0.0f;
            }
            if (attackCurr <= attackCool)
            {
                attackCurr += Time.deltaTime;
            }
        }
    }
}

