using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTrailEffect : MonoBehaviour
{
    private bool isMoved;

    private float desiredDuration = 0.3f;
    private float elapsedTime;
    private float percentageComplete;
    private RectTransform rectTransform;

    public Vector2 startPos;
    public Vector2 endPos;
    public bool isUI;

    private void OnEnable()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if(!isUI)
        this.transform.position = startPos;
        else
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = startPos;
        }

    }

    private void Update()
    {
        if (isUI && !isMoved) StartCoroutine(Move2());
        if(!Input.GetKeyDown(KeyCode.Return)) return;
        if(!isUI && !isMoved)
        {
            StartCoroutine(Move1());        
        }
    }

    IEnumerator Move1()
    {
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
        isMoved = true;
    }
    IEnumerator Move2()
    {
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, endPos, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
        isMoved = true;
    }    
    
}
