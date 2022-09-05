// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Monster_Slime : Entity_Monster
    {
        protected Entity_Player entity_Player;
        protected Animator animator;
        protected new Rigidbody rigidbody;
        protected Collider colli;
        protected float angle;
        protected float turnSpeed;
        protected Vector3 attackArea = new Vector3(1.0f, 1.0f, 1.0f);
        public bool isDead;

        void ComponentGet()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            colli = GetComponent<Collider>();
        }

        void Initialize()
        {
            turnSpeed = 10;
            entityStatus = new Entity_Status(3, 1, 0.25f, 100);
        }

        private void Awake()
        {
            Initialize();
            ComponentGet();
        }

        public override void Move(float _speed)
        {
            if (_speed == 0.0f)
            {
                //animator.SetBool("isWalking", false);
                return;
            }
            transform.position += transform.forward * _speed * Time.deltaTime;
            //animator.SetBool("isWalking", true);
        }

        public override void Rotation(float _x, float _z)
        {
            angle = Mathf.Atan2(_x, _z);
            angle = Mathf.Rad2Deg * angle;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }


        public override int Attack(float _atk)
        {
            Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), attackArea / 2, this.transform.rotation, LayerMask.GetMask("Player"), QueryTriggerInteraction.UseGlobal);
            if (colls.Length > 0)
            {
                foreach (Collider co in colls)
                {
                    entity_Player = co.GetComponent<Entity_Player>();
                    
                    if (entity_Player.IsGuard == true && Vector3.Dot(entity_Player.transform.forward, this.transform.position) < 0)
                    {
                        return 1; // 가드중 정면타격시 데미지 안들어감
                    }

                    entity_Player.Damaged(entityStatus.Atk);
                }
                return 1;
            }
            else return 0; // 공격대상을 찾지 못함
        }

        public override void Damaged(float _damage)
        {
            entityStatus.Hp -= _damage;
            if (entityStatus.Hp <= 0)
            {
                Destroyed();
            }
        }

        public override void Destroyed()
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
