using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Written by Baejinseok

/*
 * 카메라 컨트롤 스크립트
 */
public class Camera_Controller : MonoBehaviour
{
    Vector3 target;
    Vector3 pos;
    float radius = 5.0f;
    float mouseX = 0.0f;
    float mouseY = 0.0f;

    void Awake()
    {
        pos = transform.position;
    }

    // Physics랑 관련된 레이캐스트 때문에 FixedUpdate에서 처리하도록 함.
    private void FixedUpdate()
    {
        try
        {
            target = EntitySpace.Entity_Player.entity_Player.transform.position + new Vector3(0, 2, 0);
            // target = new Vector3(target.x, (int)target.y, target.z);
            pos = target + Vector3.forward * -radius;
            if (target == null) return;
        }
        catch (Exception e)
        {
            Debug.Log("Camera_Contoller.cs : Player Not Found");
            return;
        }
        GetMouseInput();
        SetCameraPos();
    }

    void GetMouseInput()
    {
        mouseX += Input.GetAxis("Mouse X") / 10;
        mouseY = Mathf.Clamp(mouseY += Input.GetAxis("Mouse Y") / 20, -0.5f, 1.35f);
    }
    void SetCameraPos()
    {
        pos = new Vector3(target.x + radius * Mathf.Sin(mouseX) * Mathf.Cos(mouseY), target.y + radius * Mathf.Sin(mouseY), target.z + radius * Mathf.Cos(mouseX) * Mathf.Cos(mouseY));


        // 카메라가 벽이나 바닥을 뚫고 지나가지 않도록 막아주는 기능
        RaycastHit ray;
        //Debug.DrawRay(target, (pos - target).normalized * Vector3.Distance(target, pos), Color.red);
        if (Physics.Raycast(target, (pos - target).normalized, out ray, Vector3.Distance(target, pos)))
        {
            if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Wall") || ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                pos = ray.point;
            }
        }
    }
    // 다른 오브젝트들의 이동이 끝난뒤에 카메라가 움직이기 위해 LateUpdate 에서 처리
    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        this.transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5f);
        this.transform.LookAt(target);
    }

    public void CameraShake()
    {
        StartCoroutine("CameraShakeCoru");
    }

    IEnumerator CameraShakeCoru()
    {
        for(int i = 0; i < 10; i++)
        {
            Vector3 ve = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f));
            transform.position += ve;
            yield return new WaitForSeconds(0.01f);
        }
    }

}

