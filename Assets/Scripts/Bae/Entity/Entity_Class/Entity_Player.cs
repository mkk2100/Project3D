//Write by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySpace
{
    public class Entity_Player : Entity_Base
    {
        Animator animator;
        Rigidbody rigidbody;
        CapsuleCollider capColi;

        // 활성화시 데미지를 받지 않음
        [SerializeField]
        bool godMode = false;
        // 플레이어 싱글턴화
        public static Entity_Player entity_Player;
        protected Vector3 attackArea = new Vector3(1.5f, 1.5f, 1.5f);
        // 땅에 붙어있는지 여부(점프 관련)
        private bool isGround;
        // 방어중인지
        bool isGuard = false;
        public bool IsGuard
        {
            get 
            {
                return isGuard;
            }
        }
        // 사망했는지
        bool isDead = false;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
        }
        // 1회 공격받은 후 이 시간동안은 플레이어가 공격을 받지 않음, 연속공격을 받고 순식간에 죽어버리는 것 방지하기 위함
        readonly protected float invincibleCool = 1.0f;
        [SerializeField]
        protected float invincibleCurr = 1.0f;

        void ComponentGet()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            capColi = GetComponent<CapsuleCollider>();
        }
        void Initalize()
        {
            entityStatus = new Entity_Status(10, 1.5f, 3, 300);
        }

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

            ComponentGet();
            Initalize();
        }

        private void Update()
        {
            InvincibleTime();
        }

        public override void Move(float _speed)
        {
            if (isGuard == true) return; // 가드중이면 이동불가

            if (_speed == 0.0f) // 0을 전달받았으면 애니메이션 끄고 이동 X
            {
                animator.SetBool("isWalking", false);
                return;
            }
            transform.position += transform.forward * _speed * Time.deltaTime;
            animator.SetBool("isWalking", true);

            return;
        }

        //피격 후 1초간의 무적시간 함수
        private void InvincibleTime()
        {
            if(invincibleCool > invincibleCurr)
            {
                invincibleCurr += Time.deltaTime;
            }
        }
        // 입력받은 각도로 서서히 회전함
        public override void Rotation(float _x, float _z)
        {
            float turnSpeed = 10;
            float angle = Mathf.Atan2(_x, _z);
            angle = (Mathf.Rad2Deg * angle);
            Quaternion targetRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation , turnSpeed * Time.deltaTime);
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

        public void Guard(bool _active)
        {
            if (isGround == false) return; // 점프 중에는 사용불가

            if (_active == true && isGuard == false)
            {
                isGuard = _active;
                //가드 올릴때 애니메이션 추가 ㄱㄱ
            }
            else if (_active == false && isGuard == true)
            {
                isGuard = _active;
                //가드 내릴때 애니메이션 추가 ㄱㄱ
            }
        }

        public override int Attack(float _atk)
        {
            Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x + (attackArea.z / 2) * Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), transform.position.y, this.transform.position.z + (attackArea.z / 2) * Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad)), attackArea / 2, this.transform.rotation, LayerMask.GetMask("Monster"), QueryTriggerInteraction.UseGlobal);
            if(colls.Length > 0)
            {
                foreach (Collider co in colls)
                {
                    co.GetComponent<Entity_Monster>().Damaged(entityStatus.Atk);
                }
                return 1;
            }
            else
            {
                return 0;
            }
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


        public override void Damaged(float _damage)
        {
            if (godMode == true) return;

            if (invincibleCurr < invincibleCool) return;
            entityStatus.Hp -= _damage;

            if (entityStatus.Hp <= 0)
            {
                Destroyed();
            }
            else // 무적시간 초기화
            {
                invincibleCurr = 0.0f;
            }
        }
        public override void Destroyed()
        {
            isDead = true;
            Destroy(gameObject); // 오브젝트 파괴대신 사망 애니메이션 등 넣으면 괜찮을듯
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
