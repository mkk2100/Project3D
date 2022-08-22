using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using EntitySpace;

public class DoorTrigger : Door
{
    private void Start()
    {
        GetLobbyGenerator();
        SetDoor();
        SetTargetPoint();
    }

    private void Update()
    {
        CheckDoorState();
    }

    private void CheckDoorState()
    {
        if(isOpen) MoveUpward();
        if(!isOpen)
        {
            if(inputDoor.transform.position.y <= 0.0f) return;
            MoveDownward();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Entity_Player player = other.GetComponent<Entity_Player>();
        if(player == null) return;

        // 캐릭터 이동 제한, 강제로 로비 가운데로 이동 후 대기

        // 뒷 문이 닫힘 (isOpen) = false;

        // 이전 맵 삭제 후 다음 맵 로드

        // 앞 문이 열림 (isOpen) = true;

        // 캐릭터가 강제로 다음 맵으로 이동
    }

    private void OnTriggerExit(Collider other)
    {
        Entity_Player player = other.GetComponent<Entity_Player>();
        if(player == null) return;

        // 캐릭터가 다음 맵으로 이동했으면 앞 문 앞에서 대기.

        // 앞 문이 닫힘(isOpen) = false
        
        // 캐릭터 제어권을 플레이어에게 넘김 (게임 시작)
    }
}
