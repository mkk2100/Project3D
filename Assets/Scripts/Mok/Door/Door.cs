using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public LobbyGenerator lobbyGenerator;
    public Vector3 movePoint;
    private Vector3 targetPoint;
    public bool isOpen;
    public int speed;

    private GameObject inputDoor;    

    private void Start()
    {
        SetDoor();
        SetTargetPoint();
    }

    private void SetDoor()
    {
        inputDoor = lobbyGenerator.GetDoor();        
    }

    private void SetTargetPoint()
    {
        targetPoint = inputDoor.transform.position + movePoint;
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    private void Update()
    {
        if(isOpen) MoveUpward();
    }

    private void MoveUpward()
    {
        inputDoor.transform.position = Vector3.MoveTowards(inputDoor.transform.position, targetPoint, speed * Time.deltaTime);
    }

    private void MoveDownward()
    {

    }
}
