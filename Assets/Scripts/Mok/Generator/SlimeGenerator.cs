using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGenerator : MonoBehaviour
{
    public GameObject slime;
    public int count;
    public int row;
    public int column;

    private Vector3 respawnMaxRange;
    private List<int> rowList;
    private List<int> columnList;


    private void OnEnable()
    {
        Initialize();
        for(int i = 0; i < count; i++) RespawnSlime(SetRandomPosition(rowList, columnList));
        RemoveListData();
    }

    private void Initialize()
    {
        rowList = new List<int>();
        columnList = new List<int>();
        respawnMaxRange = new Vector3(row, 0, column);
    }

    // 중복 없는 랜덤 추출
    private Vector3 SetRandomPosition(List<int> xList, List<int> zList)
    {
        int x = Random.Range((int)this.transform.position.x, row);
        int z = Random.Range((int)this.transform.position.z, column);
        for(int i = 0; i < row; i++)
        {
            if(xList.Contains(x))
                x = Random.Range((int)this.transform.position.x, row);
            else
            {
                xList.Add(x);
                break;
            }
        }
    
        for(int j = 0; j < column; j++)
        {
            if(zList.Contains(z))
                z = Random.Range((int)this.transform.position.z, column);
            else
            {
                zList.Add(z);
                break;
            }
        }        
        return new Vector3(x,0,z);
    }

    private void RespawnSlime(Vector3 position)
    {
        Instantiate(slime, position, Quaternion.identity);
    }

    private void RemoveListData()
    {
        rowList.RemoveRange(0, rowList.Count);
        columnList.RemoveRange(0, columnList.Count);
    }
}
