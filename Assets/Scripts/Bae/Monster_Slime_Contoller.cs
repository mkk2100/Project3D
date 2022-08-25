// Written by Baejinseok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float attackMinus = 25.0f;
        float detectDist = 8.0f;
        float detectMinus = 25.0f;

        bool findPlayer = false;

        private void Awake()
        {
            myEntity = GetComponent<Entity_Monster_Slime>();
        }
        void Update()
        {
            if (myEntity.isDead == true) return;
            
            if (attackCurr <= attackCool)
            {
                attackCurr += Time.deltaTime;
            }

            if (entity_Player == null) myEntity.Move(0.1f);
            else myEntity.Move();
            myEntity.Rotation(xDir, zDir);

            if (DebugMod == true) GetDebug();

            if (findPlayer == false)
            {
                findPlayer = FindPlayer(detectDist, detectMinus);
                RandomMove();
            }
            else
            {
                TracePlayer();
            }
        }

        float waitRandomMove;
        //플레이어 발견 못헀을때 아무 방향으로나 이동함
        bool RandomMove()
        {
            if (waitRandomMove > 0)
            {
                waitRandomMove -= Time.deltaTime;
                return false;    
            }

            if (Random.Range(0,2) == 0)
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
            return true;
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

            if (FindPlayer(attackDist, attackMinus))
            {
                Attack();
                return true;
            }
            else
            {
                myEntity.Move();
                return true;
            }

        }

        // 플레이어를 감지
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
        bool Attack() 
        {
            if (attackCurr <= attackCool) return false;
            return myEntity.Attack();
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