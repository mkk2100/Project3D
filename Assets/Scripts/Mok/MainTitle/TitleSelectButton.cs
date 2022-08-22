using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TitleSelectButton : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI text;
//    private Vector3 endPos;
//    private Vector3 startPos;
    private Vector3 movePos;
    private bool isArrowKeyPressed;

    public GameObject[] buttons;
    public RectTransform[] buttonRects;
    private float desiredDuration = 0.2f;
    private float elapsedTime;
    private float percentageComplete;

    private int idx = 0;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(buttons[0]);
        // buttonRects = GetComponentsInChildren<RectTransform>(true);
        // startPos = buttonRects[0].anchoredPosition3D;
        // Debug.Log(startPos);
    }
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0  && !isArrowKeyPressed)
        {
            isArrowKeyPressed = true;
            // endPos = startPos + new Vector3(1280 * Input.GetAxisRaw("Horizontal"),0,0);
            movePos = new Vector3(1280 * Input.GetAxisRaw("Horizontal"),80,0);
            // Debug.Log(movePos);
            StartCoroutine(SlidingMoveEffect());
        }
    }

    // protected void SwitchButton(int idx)
    // {
    //     // for(int i = 0; i < transform.childCount; i++) buttons[i].SetActive(i == idx);

    // }

    private IEnumerator SlidingMoveEffect()
    {
        int inputDir = (int) Input.GetAxisRaw("Horizontal");

        Vector3 temp = buttonRects[idx].anchoredPosition3D;
        EventSystem.current.SetSelectedGameObject(null);
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            buttonRects[idx].anchoredPosition3D = Vector3.Lerp(new Vector3(0, 80, 0), movePos, percentageComplete);
            
            // Debug.Log(temp);
            //buttonRects[idx].anchoredPosition3D = Vector3.Lerp(startPos, endPos, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
        buttons[idx].SetActive(false);
        idx += inputDir;
        if(idx < 0) idx = buttons.Length -1;
        else if(idx == buttons.Length) idx = 0;
        buttons[idx].SetActive(true);
        
        percentageComplete = 0.0f;
        elapsedTime = 0.0f;
        temp = buttonRects[idx].anchoredPosition3D;
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            
            buttonRects[idx].anchoredPosition3D = Vector3.Lerp(new Vector3(movePos.x * -1, movePos.y, movePos.z), new Vector3(0, 80, 0) , percentageComplete);
            
            //buttonRects[idx].anchoredPosition3D = Vector3.Lerp(startPos, endPos, percentageComplete);
            yield return new WaitForEndOfFrame();
        }
        
        percentageComplete = 0.0f;
        elapsedTime = 0.0f;
        EventSystem.current.SetSelectedGameObject(buttons[idx]);
        isArrowKeyPressed = false;
    }
}
