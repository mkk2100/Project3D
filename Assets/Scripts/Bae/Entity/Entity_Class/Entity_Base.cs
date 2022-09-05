using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// 플레이어, 몬스터 등 엔터티들의 행동에 대한 베이스 스크립트

namespace EntitySpace
{
    public abstract class Entity_Base : MonoBehaviour
    {
        [SerializeField]
        public Entity_Status entityStatus;

        public void Move()
        {
            Move(entityStatus.Speed);
        }
        public abstract void Move(float _speed);
        public abstract void Rotation(float _x, float _z);
        public void Jump()
        {
            Jump(entityStatus.JumpForce);
        }
        public abstract void Jump(float _jumpForce);

        public int Attack()
        {
            return Attack(entityStatus.Atk);
        }
        public abstract int Attack(float _atk);

        public virtual void Destroyed()
        {
            Destroy(this.gameObject);
        }
        public abstract void Damaged(float _damage);
    }

}