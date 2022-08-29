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
    void Start()
    {
        pos = transform.position;
    }

    // Physics랑 관련된 레이캐스트 때문에 FixedUpdate에서 처리하도록 함.
    private void FixedUpdate()
    {
        try
        {
            if (target == null) return;
            target = EntitySpace.Entity_Player.entity_Player.transform.position + new Vector3(0, 2, 0);

            pos = target + Vector3.forward * -radius;
        }
        catch (Exception e)
        {
            Debug.Log("Camera_Contoller.cs : Player Not Found");
            return;
        }
        mouseX += Input.GetAxis("Mouse X");
        mouseY = Mathf.Clamp(mouseY += Input.GetAxis("Mouse Y") / 2, -0.5f, 1.35f);

        pos = new Vector3(target.x + radius * Mathf.Sin(mouseX) * Mathf.Cos(mouseY), target.y + radius * Mathf.Sin(mouseY), target.z + radius * Mathf.Cos(mouseX) * Mathf.Cos(mouseY));


        // 카메라가 벽이나 바닥을 뚫고 지나가지 않도록 막아주는 기능
        RaycastHit ray;
        Debug.DrawRay(target, (pos - target).normalized * Vector3.Distance(target, pos), Color.red);
        if (Physics.Raycast(target, (pos - target).normalized, out ray, Vector3.Distance(target, pos)))
        {
            if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Wall") || ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                pos = ray.point;
            }
                
        }

    }
    void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5f);
        this.transform.LookAt(target);
    }
}

