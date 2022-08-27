// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Monster_Slime : Entity_Monster
    {
        private Entity_Player entity_Player;

        Animator animator;
        new Rigidbody rigidbody;
        CapsuleCollider capColi;
        protected float angle;
        protected float turnSpeed;
        protected Vector3 attackArea = new Vector3(1.0f, 1.0f, 1.0f);

        private bool isGround;
        public bool isDead;
        private void Awake()
        {
            turnSpeed = 10;
            entityStatus = new Entity_Status(3, 1, 0.25f, 100);
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            capColi = GetComponent<CapsuleCollider>();
        }

        public override bool Move(float _speed)
        {
            if (_speed == 0.0f)
            {
                //animator.SetBool("isWalking", false);
                return false;
            }
            transform.position += transform.forward * _speed * Time.deltaTime;
            //animator.SetBool("isWalking", true);

            return true;
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
            Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), attackArea / 2, this.transform.rotation, LayerMask.GetMask("Player"), QueryTriggerInteraction.UseGlobal);
            if (colls.Length > 0)
            {
                foreach (Collider co in colls)
                {
                    entity_Player = co.GetComponent<Entity_Player>();
                    if (entity_Player.IsGuard())
                    {
                        Debug.Log(Vector3.Dot(entity_Player.transform.forward, this.transform.position));
                        if (Vector3.Dot(entity_Player.transform.forward, this.transform.position) < 0) return false; // 가드중 정면타격시 데미지 안들어감
                    }
                    float enemyHp = entity_Player.Damaged(entityStatus.Atk);
                    Debug.Log(enemyHp);
                }
                return true;
            }
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
        public override float Damaged(float _damage)
        {
            entityStatus.Hp -= _damage;
            if (entityStatus.Hp <= 0)
            {
                Destroyed();
                return 0.0f;
            }
            else return entityStatus.Hp;
        }

        public override bool Destroyed()
        {
            isDead = true;
            Destroy(gameObject);
            return true;
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
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, attackArea);
        }
    }
        

}
