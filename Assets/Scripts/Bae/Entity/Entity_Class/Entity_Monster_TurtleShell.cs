using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written by Baejinesok
 * 보스몬스터 터틀쉘의 기능 스크립트, 슬라임 스크립트를 상속 받았다.
 * 때문에 이동, 공격같은 대부분의 기능들은 유사하게 작동하나 돌진공격이 추가되었음.
*/
namespace EntitySpace
{
    public class Entity_Monster_TurtleShell : Entity_Monster_Slime
    {

        Vector3 lastVelocity; // 벽에 부딪히기 직전의 rigidbody.velocity값을 저장하는 변수
        Vector3 rushDir; // 돌진의 방향
        float rushSpeed; // 돌진의 스피드를 설정, 벽에 부딪힐때마다 점점 가속이 붙는다
        readonly float rushSpeedSet = 5.0f;
        bool isRush = false; // 돌진이 작동중인지 여부
        /*
         * 돌진패턴중 벽에 부딪힐때마다 1씩 감소하는 변수, 벽에 5회 부딪히면 패턴을 종료한다
         * collisionCnt = 0 // 돌진 패턴 종료
         * collisionCnt = 1 ~ collisionCntSet // 돌진중
         */
        int collisionCnt = 0;
        int collisionCntSet = 5;
        /* 
        * 돌진중 플레이어와 부딪혔는지 여부.
        * 벽에 한번 부딪히면 초기화.
        * 1회의 돌진에 플레이어가 여러번 부딪히지 않게 하기 위함. 
        */
        bool rushHit;
        /*
        * 현재 벽과 닿아있는지의 여부.
        * 복수의 벽에 충돌했을경우 충돌 카운트가 여러개 올라가 패턴이 빠르게 끝나버리기에 한번의 충돌에 한번만 동작하도록 하기 위함
        */
        bool hitWall = false;

        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;

        [SerializeField]
        GameObject slimePrf;
        Camera_Controller camera_Controller;
        void Initialize()
        {
            rushHit = false;
            rushSpeed = rushSpeedSet;
            turnSpeed = 10;
            entityStatus = new Entity_Status(20, 2, 1.0f, 100);
        }
        void ComponentGet()
        {
            animator = GetComponent<Animator>();
            rigidbody = gameObject.GetComponent<Rigidbody>();
            colli = GetComponent<Collider>();
            camera_Controller = Camera.main.GetComponent<Camera_Controller>();
        }
        private void Update()
        {
            if (collisionCnt == 0) return;
            lastVelocity = rigidbody.velocity; // 돌진패턴이 작동중이면 계속해서 직전의 이동속도를 저장해둔다.
        }
        private void Awake()
        {
            ComponentGet();
            Initialize();
            GetGroundBounds();
        }

        /*
         * 바닥 콜라이더를 기반으로 맵 사이즈를 대강 얻어오는 함수
         * 맵이 사각형 형태가 아니면 완전히 정확한 사이즈를 얻어올수는 없겠지만 맵이 사각형이라 상관없을듯.
         */
        private void GetGroundBounds()
        {
            GameObject[] goTemp = GameObject.FindGameObjectsWithTag("Ground");

            foreach (GameObject go in goTemp)
            {
                try
                {
                    float tempF;
                    Collider co = go.GetComponent<Collider>();

                    tempF = co.transform.position.x - co.bounds.size.x / 2;
                    if (minX > tempF) minX = tempF;

                    tempF = co.transform.position.x + co.bounds.size.x / 2;
                    if (maxX < tempF) maxX = tempF;

                    tempF = co.transform.position.z - co.bounds.size.z / 2;
                    if (minZ > tempF) minZ = tempF;

                    tempF = co.transform.position.z + co.bounds.size.z / 2;
                    if (maxZ < tempF) maxZ = tempF;
                }
                catch (System.Exception e)
                {
                    Debug.Log("Entity_Monster_TurtleShell : Collider Not Found");
                }
            }
        }

        // 벽에 부딪힐때마다 슬라임을 맵에 랜덤소환
        void SpawnSlime()
        {
            RaycastHit hit;
            Vector3 ve;

            ve = new Vector3(Random.Range(minX, maxX), 10, Random.Range(minZ, maxZ)); // GetGroundBounds 함수에서 측정한 맵 사이즈를 기반으로 범위 랜덤지정

            for (int i = 0; i < 3; i++) // 만약 랜덤좌표에 바닥이 없으면 랜덤좌표를 다시 얻어옴, 3번 얻어오고도 맞는 좌표가 없으면 스킵함
            {
                if (Physics.Raycast(ve, Vector3.down, out hit, 1000, LayerMask.GetMask("Ground")))
                {
                    Instantiate(slimePrf, ve, this.transform.rotation);
                    return;
                }
                else
                {
                    ve = new Vector3(Random.Range(minX, maxX), 10, Random.Range(minZ, maxZ));
                }
            }
            
        }

        public void SetRushDir(Vector3 _dir)
        {
            rushDir = new Vector3(_dir.x, 0, _dir.z);
        }

        public void RushSet(bool _isRush)
        {
            /* 
             * 플레이어와 몬스터가 돌진 경로 차단하는걸 막기위해 플레이어와 몬스터와의 충돌하지 않는 레이어로 변경,
             * 콜리전 카운트를 1로 세팅해 패턴이 시작됨을 알림.
            */
            if(_isRush == true)
            {
                gameObject.layer = LayerMask.NameToLayer("MonsterRush");
                collisionCnt = collisionCntSet;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Monster");
                rushSpeed = rushSpeedSet;
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(rushDir * 200 + Vector3.up * 300);
            }
            isRush = _isRush;
        }

        public int RushAttack()
        {
            if (isRush == false) RushSet(true);
            if (collisionCnt > 0) // 패턴 실행
            {
                if(rushHit == false)
                {
                    Collider[] colls = Physics.OverlapBox(colli.bounds.center, colli.bounds.size, this.transform.rotation, LayerMask.GetMask("Player"), QueryTriggerInteraction.UseGlobal);
                    if (colls.Length > 0)
                    {
                        foreach (Collider co in colls)
                        {
                            entity_Player = co.GetComponent<Entity_Player>();
                            rushHit = true; // 한번의 돌진에는 플레이어를 한번만 떄리도록 함
                            entity_Player.Damaged(this.entityStatus.Atk * 1.5f);
                            entity_Player.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 400 + rushDir * 400);
                            // 돌진중에 플레이어가 접촉하면 데미지를 입고 튕겨나간다
                        }
                    }
                }
                Rotation(rushDir.x, rushDir.z);
                rigidbody.velocity = rushDir * rushSpeed; 
            }
            if (collisionCnt <= 0) RushSet(false);
            return collisionCnt;
        }
        public override void Damaged(float _damage)
        {
            if (isRush == true) return; // 돌진 중 무적

            entityStatus.Hp -= _damage;
            if (entityStatus.Hp <= 0)
            {
                Destroyed();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collisionCnt <= 0) return; // 충돌 카운트가 0이하면 작동하지 않음
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall") && hitWall == false)
            {
                hitWall = true;
                rushHit = false; // 플레이어 타격여부 초기화
                rushDir = Vector3.Reflect(new Vector3(lastVelocity.x, 0, lastVelocity.z).normalized, new Vector3(collision.contacts[0].normal.x,0,collision.contacts[0].normal.z));
                collisionCnt--;
                rushSpeed += 3.0f;
                SpawnSlime();
                camera_Controller.CameraShake();
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                hitWall = false; // 벽 타격여부 초기화
            }
        }
    }
}