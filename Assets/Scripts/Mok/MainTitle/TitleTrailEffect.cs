using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTrailEffect : MonoBehaviour
{
    private Vector2 startPos, endPos;
    private bool isMoved;

    private float desiredDuration = 0.3f;
    private float elapsedTime;
    private float percentageComplete;

    private void Start()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        startPos = new Vector2(12, -4);
        endPos = new Vector2(-20, -4);
        this.transform.position = startPos;
    }

    private void Update()
    {
        if(!Input.GetKeyDown(KeyCode.Return)) return;
        if(!isMoved) StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
    }
}
