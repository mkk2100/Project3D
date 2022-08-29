// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 터틀쉘의 AI 스크립트, 어떤 행동을 할지 판단하여 entity 스크립트로 값을 넘기는 역할만 수행하도록 함. 실질적인 동작은 entity 스크립트에서 수행
 */

namespace EntitySpace
{
    public class Monster_TurtleShell_Contoller : MonoBehaviour
    {
        Entity_Player entity_Player;
        Entity_Monster_Slime myEntity;

        protected float attackCool = 1.0f;
        [SerializeField]
        protected float attackCurr = 1.0f;

        float xDir;
        float zDir;


        bool findPlayer = false;

        private void Awake()
        {
            myEntity = GetComponent<Entity_Monster_Slime>();
        }
        void Update()
        {
            if (myEntity.isDead == true) return;

            if (DebugMod == true) GetDebug();

            timer();
            Action();
        }


        //플레이어 발견 못헀을때 아무 방향으로나 이동함
        float waitRandomMove; // 랜덤한 시간만큼 이동하도록 하는 float 변수

        private void Action()
        {
            if (findPlayer == false && waitRandomMove <= 0)
            {
                findPlayer = FindPlayer(detectDist, detectMinus);
                RandomMove();
            }
            else if (findPlayer == true)
            {
                TracePlayer();
            }

            if (entity_Player == null) myEntity.Move(0.1f);
            else myEntity.Move();

            myEntity.Rotation(xDir, zDir);
        }

        void timer()
        {
            if (waitRandomMove > 0)
            {
                waitRandomMove -= Time.deltaTime;
            }

            if (attackCurr <= attackCool)
            {
                attackCurr += Time.deltaTime;
            }
        }

        void RandomMove()
        {
            if (Random.Range(0, 2) == 0)
            {
                xDir = 0;
                zDir = 0;
            }
            else
            {
                xDir = Random.Range(-1f, 1f);
                zDir = Random.Range(-1f, 1f);
            }
            waitRandomMove = Random.Range(1.5f, 5.0f);
        }

        float attackDist = 1.0f;
        float attackMinus = 25.0f;
        float detectDist = 8.0f;
        float detectMinus = 25.0f;

        // 플레이어를 감지, 처음 플레이어 추적, 공격거리 감지에 사용
        private bool FindPlayer(float _dist, float _degreeMinus)
        {
            Collider[] colls;

            colls = Physics.OverlapSphere(transform.position, _dist);
            foreach (Collider co in colls)
            {
                if (co.gameObject.CompareTag("Player"))
                {
                    float dot = Vector3.Dot(this.gameObject.transform.forward, (co.gameObject.transform.position - this.transform.position).normalized);
                    dot -= Mathf.Deg2Rad * _degreeMinus;
                    if (dot > 0)
                    {
                        entity_Player = co.GetComponent<Entity_Player>();
                        return true;
                    }
                }
            }
            return false;
        }

        // 플레이어를 발견후 추적 및 공격

        bool TracePlayer()
        {
            if (entity_Player == null || findPlayer == false)
            {
                return false;
            }

            Vector3 tempVec = ((entity_Player.transform.position - this.transform.position).normalized);

            xDir = tempVec.x;
            zDir = tempVec.z;

            if(attackCount < 3)
            {
                Attack();
            }
            else
            {
                RushAttack();
            }
            myEntity.Move();
            return true;

        }

        int attackCount = 0;
        bool Attack()
        {
            if (myEntity.Attack() == true)
            {
                attackCount += 1;
                attackCurr = attackCool;
                return true;
            }

            return false;
        }

        bool RushAttack()
        {
            attackCount = 0;

            // 플레이어 위치 - 자신의 위치로 지속이동
            // 벽에 닿으면 반사각을 구해서 반사
            // 총 5회 반사되면 패턴중지

            return true;
        }

        // 씬화면에서 보이는 기즈모 디버깅용, 인스펙터에서 디버그모드 bool 켜야 보임
        [SerializeField]
        private bool DebugMod;
        private bool debugFound;
        private bool debugAttack;
        private void GetDebug()
        {
            Collider[] colls;
            Collider[] colls2;
            colls = Physics.OverlapSphere(transform.position, detectDist);
            colls2 = Physics.OverlapSphere(transform.position, attackDist);
            foreach (Collider co in colls)
            {
                if (co.gameObject.CompareTag("Player"))
                {
                    float dot = Vector3.Dot(this.gameObject.transform.forward, (co.gameObject.transform.position - this.transform.position).normalized);
                    Debug.Log(dot);
                    dot -= Mathf.Deg2Rad * detectMinus;
                    if (dot > 0)
                    {
                        debugFound = true;
                    }
                }
            }
            foreach (Collider co in colls2)
            {
                if (co.gameObject.CompareTag("Player"))
                {
                    float dot = Vector3.Dot(this.gameObject.transform.forward, (co.gameObject.transform.position - this.transform.position).normalized);
                    Debug.Log(dot);
                    dot -= Mathf.Deg2Rad * attackMinus;
                    if (dot > 0)
                    {
                        debugAttack = true;
                    }
                }
            }

        }

        // 기즈모 그리기
        private void OnDrawGizmos()
        {
            if (DebugMod == false) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackDist);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, detectDist);


            if (debugAttack == true)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, Entity_Player.entity_Player.transform.position);
                debugAttack = false;
                return;
            }
            if (debugFound == true)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, Entity_Player.entity_Player.transform.position);
                debugFound = false;
                return;
            }

        }
    }

}