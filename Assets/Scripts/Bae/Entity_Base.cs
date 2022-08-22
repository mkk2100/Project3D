using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Write by baejinseok

// �÷��̾�, ���� �� ����Ƽ���� �ൿ�� ���� ���̽� ��ũ��Ʈ

namespace EntitySpace
{
    public abstract class Entity_Base : MonoBehaviour
    {
        [SerializeField]
        protected Entity_Status entityStatus;

        public bool Move()
        {
            return Move(entityStatus.Speed);
        }
        public abstract bool Move(float _speed);
        public abstract bool Rotation(float _x, float _z);
        public bool Jump()
        {
            return Move(entityStatus.JumpForce);
        }
        public abstract void Jump(float _jumpForce);

        public bool Attack()
        {
            return Attack(entityStatus.Atk);
        }
        public abstract bool Attack(float _atk);

        
    }

}