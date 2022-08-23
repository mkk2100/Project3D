//Write by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 관련 스크립트
namespace EntitySpace
{
    public class Entity_Player : Entity_Base
    {
        public static Entity_Player entity_Player;

        Animator animator;
        new Rigidbody rigidbody;
        CapsuleCollider capColi;
        protected float angle;
        protected float turnSpeed;

        private bool isGround;
        private void Awake()
        {
            if (entity_Player == null)
            {
                entity_Player = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            turnSpeed = 10;
            entityStatus = new Entity_Status(3, 1, 3, 300);
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            capColi = GetComponent<CapsuleCollider>();
        }

        // ??? ??????
        public override bool Move(float _speed)
        {
            if (_speed == 0.0f) // ????? 0?? ?????? ??????? ??????? False ????
            {
                animator.SetBool("isWalking", false);    // ??? ??????? ??? ??
                return false;
            }

            transform.position += transform.forward * _speed * Time.deltaTime;
            animator.SetBool("isWalking", true);    // ??? ??????? ??? ??

            return true;
        }

        // ?????? + ??? (?? ????? ???? ?????? ??? ??? ??? ????)
        public override bool Rotation(float _x, float _z)
        {
            angle = Mathf.Atan2(_x, _z);
            angle = Mathf.Rad2Deg * angle;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            return true;
        }

        public override void Jump(float _jumpForce)
        {
            if (isGround == true)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector3.up * _jumpForce);
            }

        }
        public override bool Attack(float _atk)
        {
            return true;
        }
        private void GroundCheck()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.down * ((capColi.bounds.size.y / 2) - capColi.bounds.center.y + 0.5f), Color.red);

            if (Physics.Raycast(transform.position, Vector3.down, out hit, (capColi.bounds.size.y / 2) - capColi.bounds.center.y + 0.5f) && rigidbody.velocity.y <= 0)
            {
                if (hit.transform.CompareTag("Ground")) isGround = true;
                else isGround = false;
            }
            else isGround = false;

        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                GroundCheck();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            isGround = false;
        }
    }


}