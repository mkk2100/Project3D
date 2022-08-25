//Write by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Player : Entity_Base
    {
        public static Entity_Player entity_Player;

        Animator animator;
        Rigidbody rigidbody;
        CapsuleCollider capColi;
        protected float angle;
        protected float turnSpeed;
        protected Vector3 attackArea = new Vector3(1.5f, 1.5f, 1.5f);

        private bool isGround;
        bool isDead;

        protected float invincibleCool = 1.0f;
        [SerializeField]
        protected float invincibleCurr = 1.0f;

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
            entityStatus = new Entity_Status(3, 1.5f, 3, 300);
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            capColi = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            invincibleTime();
        }

        // ??? ??????
        public override bool Move(float _speed)
        {
            if (_speed == 0.0f) // 0을 전달받았으면 애니메이션 끄고 이동 X
            {
                animator.SetBool("isWalking", false);
                return false;
            }

            transform.position += transform.forward * _speed * Time.deltaTime;
            animator.SetBool("isWalking", true);

            return true;
        }

        private void invincibleTime()
        {
            if(invincibleCool > invincibleCurr)
            {
                invincibleCurr += Time.deltaTime;
            }
        }
        public override bool Rotation(float _x, float _z)
        {
            angle = Mathf.Atan2(_x, _z);
            angle = Mathf.Rad2Deg * angle;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            return true;
        }
        public override bool Jump(float _jumpForce)
        {
            Debug.Log("Jump?");
            if (isGround == true)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector3.up * _jumpForce);
                return true;
            }
            return false;
        }
        public override bool Attack(float _atk)
        {
            Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), attackArea / 2, this.transform.rotation, LayerMask.GetMask("Monster"), QueryTriggerInteraction.UseGlobal);
            if(colls.Length > 0)
            {
                foreach (Collider co in colls)
                {
                    float enemyHp = co.GetComponent<Entity_Monster>().Damaged(entityStatus.Atk);
                    Debug.Log(enemyHp);
                }
                return true;
            }
            Debug.Log("Target Lost");
            return false;
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
        public override float Damaged(float _damage)
        {
            if (invincibleCurr < invincibleCool) return -1.0f;
            entityStatus.Hp -= _damage;

            if (entityStatus.Hp <= 0)
            {
                Destroyed();
                return 0;
            }
            else
            {
                invincibleCurr = 0.0f;
                return (int)entityStatus.Hp;
            }
        }
        public override bool Destroyed()
        {
            isDead = true;
            Destroy(gameObject);
            return true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isGround = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, attackArea);
        }
        
    }
}
