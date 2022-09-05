// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 터틀쉘의 AI 스크립트.
 * entity 스크립트로 어떤 동작을 수행하는지 지시하는 역할을 수행.
 * 실질적인 동작은 entity 스크립트에서 수행한다.
 */

namespace EntitySpace
{
    public class Monster_TurtleShell_Controller : MonoBehaviour
    {

        Entity_Player entity_Player;
        Entity_Monster_TurtleShell myEntity;

        protected float attackCurr; // 공격의 쿨타임
        readonly protected float attackCool = 1.0f;

        protected float rushReadyCurr; // 러쉬전 약간의 선딜레이를 위한 변수
        readonly protected float rushReadySet = 3.0f;

        /*
         * 이 시간이 0이 되면 러쉬가 작동함. 
         * 플레이어와 본인과의 거리에 비례하여 더 멀리 떨어질수록 빠르게 감소되며 플레이어를 공격해도 조금씩 감소함
         */
        protected float rushTimer;
        readonly protected float rushTimerSet = 40.0f;

        //회전 방향
        float xDir;
        float zDir;

        enum MyState // 현재 어떤 상태인지를 나타내는 열거형
        {
            Normal,
            Trace,
            RushReady,
            Rush
        }

        MyState myState;

        /*
         * 공격 거리와 각도, 플레이어 감지 거리, 전방에서 180도 + AddDeg만큼 감지
         */
        float attackDist = 1.0f;
        float attackAddDeg = -25.0f;
        float detectDist = 8.0f;
        float detectAddDeg = 180.0f;

        void Initialize()
        {
            xDir = 0;
            zDir = 0;
            attackCurr = attackCool;
            rushReadyCurr = rushReadySet;
            rushTimer = rushTimerSet;
            myState = MyState.Normal;
        }

        private void Awake()
        {
            Initialize();
            myEntity = GetComponent<Entity_Monster_TurtleShell>();
        }
        void Update()
        {
            if (DebugMod == true) GetDebug();
            StateSet();
            timer();
        }
        void StateSet()
        {
            switch (myState)
            {
                case MyState.Normal:
                    DirSet(ref xDir, ref zDir);
                    Move();
                    if (FindPlayer(detectDist, detectAddDeg) == true) myState = MyState.Trace;
                    if (myEntity.entityStatus.FullHp > myEntity.entityStatus.Hp) myState = MyState.Trace;
                    break;
                case MyState.Trace:
                    DirSet(ref xDir, ref zDir);
                    Move();
                    Attack();
                    if (rushTimer <= 0) // 러쉬타이머가 다되면 러쉬준비
                    {
                        rushReadyCurr = rushReadySet;
                        myState = MyState.RushReady;
                    }
                    break;
                case MyState.RushReady:
                    if (rushReadyCurr <= 0) // 러쉬전 딜레이 시간동안 대기
                    {
                        myState = MyState.Rush;
                        rushReadyCurr = rushReadySet;
                        rushTimer = rushTimerSet;
                        myEntity.SetRushDir((entity_Player.transform.position - this.transform.position).normalized);
                    }
                    break;
                case MyState.Rush:
                    if (myEntity.RushAttack() <= 0)
                    {
                        myState = MyState.Trace; // 러쉬를 끝마치면 다시 추적태세로 변환
                        rushTimer = rushTimerSet;
                    }
                    break;
            }
        }
        void timer()
        {
            if (waitRandomMove >= 0)
            {
                waitRandomMove -= Time.deltaTime;
            }

            if (attackCurr <= attackCool)
            {
                attackCurr += Time.deltaTime;
            }

            if (rushReadyCurr >= 0 && myState == MyState.RushReady)
            {
                rushReadyCurr -= Time.deltaTime;
            }

            if (rushTimer >= 0 && myState == MyState.Trace && entity_Player != null)
            {
                rushTimer -= Time.deltaTime * Vector3.Distance(entity_Player.transform.position, this.transform.position);
            }
        }

        //플레이어 발견 못헀을때 아무 방향으로나 이동함
        
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

        private void Move()
        {
            if (myState != MyState.Rush && myState != MyState.RushReady) // 돌진상태가 아니면 계속해서 이동, 회전함
            {
                myEntity.Rotation(xDir, zDir);
                if (entity_Player == null) myEntity.Move(0.5f); // 플레이어를 발견하지 못했으면 느린 속도로 이동
                else myEntity.Move();
            }
        }
        // 플레이어를 발견후 추적 및 공격

        float waitRandomMove; // 랜덤한 시간만큼 이동하도록 하는 float 변수
        void DirSet(ref float _xDir, ref float _zDir)
        {
            if(myState == MyState.Normal) // 랜덤방향
            {
                if (waitRandomMove > 0) return;

                else
                {
                    _xDir = Random.Range(-1f, 1f);
                    _zDir = Random.Range(-1f, 1f);
                }
                waitRandomMove = Random.Range(0.5f, 3.0f);
            }
            if(myState == MyState.Trace) // 플레이어 추적
            {

                if (entity_Player == null) return;

                Vector3 tempVec = ((entity_Player.transform.position - this.transform.position).normalized);

                _xDir = tempVec.x;
                _zDir = tempVec.z;
            }
        }
        
        void Attack()
        {
            if (entity_Player == null || myState != MyState.Trace || attackCool >= attackCurr) return;

            if (myEntity.Attack() == 1)
            {
                rushTimer -= 5.0f;
                attackCurr = 0;
            }
        }

        // 씬화면에서 보이는 기즈모 보이게 만드는용도, 인스펙터에서 디버그모드 bool 켜야 작동됨, 성능이 나쁘기 때문에 평소엔 꺼둘것
        [SerializeField]
        private bool DebugMod;
        enum DebugState
        {
            Noraml,
            Found,
            RushReady,
        }
        DebugState debugState = DebugState.Noraml;

        private void GetDebug()
        {
            bool find = FindPlayer(detectDist, detectAddDeg);
            if (find == true && debugState == DebugState.Noraml) debugState = DebugState.Found;
            else if (find == false && debugState == DebugState.Found) debugState = DebugState.Noraml;

            if (myState == MyState.RushReady) debugState = DebugState.RushReady;
            else if (debugState == DebugState.RushReady) debugState = DebugState.Noraml;
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
            if (debugState == DebugState.RushReady)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, Entity_Player.entity_Player.transform.position);
                return;
            }

        }
    }
}