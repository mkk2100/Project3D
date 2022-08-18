using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private LobbyGenerator lobbyGenerator;
    private Vector3 movePoint;
    private Vector3 defaultTargetPoint;
    private Vector3 targetPoint;
    public bool isOpen;
    private int speed = 2;

    protected GameObject inputDoor;

    protected void GetLobbyGenerator()
    {
        lobbyGenerator = FindObjectOfType<LobbyGenerator>();
    }

    protected void SetDoor()
    {
        inputDoor = lobbyGenerator.GetDoor();        
    }

    protected void SetTargetPoint()
    {
        movePoint = new Vector3(0, 3.0f, 0);
        defaultTargetPoint = inputDoor.transform.position;
        targetPoint = inputDoor.transform.position + movePoint;   
    }

    protected void OpenDoor()
    {
        isOpen = true;
    }

    protected void CloseDoor()
    {
        isOpen = false;
    }

    protected void MoveUpward()
    {
        inputDoor.transform.position = Vector3.MoveTowards(inputDoor.transform.position, targetPoint, speed * Time.deltaTime);
    }

    protected void MoveDownward()
    {
        inputDoor.transform.position = Vector3.MoveTowards(inputDoor.transform.position, defaultTargetPoint, speed * Time.deltaTime);
    }
}
