// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 슬라임의 AI 스크립트, 어떤 행동을 할지 판단하여 entity 스크립트로 값을 넘기는 역할만 수행하도록 함. 실질적인 동작은 entity 스크립트에서 수행
 */

namespace EntitySpace
{
    public class Monster_Slime_Contoller : MonoBehaviour
    {
        Entity_Player entity_Player;
        Entity_Monster_Slime myEntity;

        protected float attackCool = 1.0f;
        [SerializeField]
        protected float attackCurr = 1.0f;

        float xDir;
        float zDir;

        float attackDist = 1.0f;
        float attackAddDeg = 25.0f;
        float detectDist = 8.0f;
        float detectAddDeg = 25.0f;


        bool findPlayer = false;

        float waitRandomMove; // 랜덤한 시간만큼 이동하도록 하는 float 변수

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


        
        private void Action()
        {
            if (findPlayer == false && waitRandomMove <= 0)
            {
                if (myEntity.entityStatus.FullHp > myEntity.entityStatus.Hp) findPlayer = true;
                findPlayer = FindPlayer(detectDist, detectAddDeg);
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

        
        // 플레이어를 감지, 처음 플레이어 추적, 공격거리 감지에 사용
        private bool FindPlayer(float _dist, float _degreeAdd)
        {
            Collider[] colls;

            colls = Physics.OverlapSphere(transform.position, _dist);
            foreach (Collider co in colls)
            {
                if (co.gameObject.CompareTag("Player"))
                {
                    float dot = Vector3.Dot(this.gameObject.transform.forward, (co.gameObject.transform.position - this.transform.position).normalized);
                    dot += Mathf.Deg2Rad * _degreeAdd;
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
                             
            if(attackCool < attackCurr) Attack();
            myEntity.Move();
            return true;

        }

        void Attack() 
        {
            myEntity.Attack();
            attackCurr = attackCool;
        }


        // 씬화면에서 보이는 기즈모 보이게 만드는용도, 인스펙터에서 디버그모드 bool 켜야 작동됨, 성능이 나쁘기 때문에 평소엔 꺼둘것
        [SerializeField]
        private bool DebugMod;
        enum DebugState
        {
            Noraml,
            Found
        }
        DebugState debugState = DebugState.Noraml;

        private void GetDebug()
        {
            bool find = FindPlayer(detectDist, detectAddDeg);
            if (find == true && debugState == DebugState.Noraml) debugState = DebugState.Found;
            else if (find == false && debugState == DebugState.Found) debugState = DebugState.Noraml;
        }
        // 기즈모 그리기
        private void OnDrawGizmos()
        {
            if (DebugMod == false) return;
            Gizmos.color = Color.white; // 플레이어 감지범위
            Gizmos.DrawWireSphere(transform.position, detectDist);

            if (entity_Player == null) return;

            if (debugState == DebugState.Found)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, Entity_Player.entity_Player.transform.position);
                return;
            }


        }
    }

}