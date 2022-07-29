using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGenerator : MonoBehaviour
{
    public GameObject lobby;
    public List<GameObject> doors;
    private int randomIdx;
    private Transform lobbyTransform;

    private void Start()
    {
        SetRandomNumber();
        SetLobby(randomIdx);
    }

    private void SetRandomNumber()
    {
        randomIdx = Random.Range(0, doors.Count);
    }

    private void SetLobby(int randomNumber)
    {
        switch(randomNumber)
        {
            case 0:
                Instantiate(lobby, doors[0].transform.position + new Vector3(0,0,-2), Quaternion.Euler(0,90f,0));
                return;
            case 1:
                Instantiate(lobby, doors[1].transform.position + new Vector3(0,0,-2), Quaternion.Euler(0,90f,0));
                return;
            case 2:
                Instantiate(lobby, doors[2].transform.position + new Vector3(0,0,2), Quaternion.Euler(0,-90f,0));
                return;
            case 3:
                Instantiate(lobby, doors[3].transform.position + new Vector3(0,0,2), Quaternion.Euler(0,-90f,0));
                return;
            case 4:
                Instantiate(lobby, doors[4].transform.position + new Vector3(2,0,0), Quaternion.identity);
                return;
            case 5:
                Instantiate(lobby, doors[5].transform.position + new Vector3(2,0,0), Quaternion.identity);
                return;
            case 6:
                Instantiate(lobby, doors[6].transform.position + new Vector3(-2,0,0), Quaternion.Euler(0,180f,0));
                return;
            case 7:
                Instantiate(lobby, doors[7].transform.position + new Vector3(-2,0,0), Quaternion.Euler(0,180f,0));
                return;
            default:
                return;
        }
    }
}
