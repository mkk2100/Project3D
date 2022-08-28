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
    private bool isQuit;

    public GameObject[] buttons;
    public RectTransform[] buttonRects;
    private float desiredDuration = 0.1f;
    private float elapsedTime;
    private float percentageComplete;

    private int idx = 0;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    private void Update()
    {
        if(isQuit) return;
        if(Input.GetAxisRaw("Horizontal") != 0  && !isArrowKeyPressed)
        {
            isArrowKeyPressed = true;
            EventSystem.current.SetSelectedGameObject(null);
            movePos = new Vector3(1280 * Input.GetAxisRaw("Horizontal"),80,0);
            StartCoroutine(SlidingMoveEffect());
            AudioManager.instance.SFXPlay("Select ", AudioManager.instance.SFXLibrary[1]);
        }
    }

    private IEnumerator SlidingMoveEffect()
    {
        int inputDir = (int) Input.GetAxisRaw("Horizontal");

        Vector3 temp = buttonRects[idx].anchoredPosition3D;
        while(percentageComplete <= 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            buttonRects[idx].anchoredPosition3D = Vector3.Lerp(new Vector3(0, 80, 0), movePos, percentageComplete);
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
            yield return new WaitForEndOfFrame();
        }
        percentageComplete = 0.0f;
        elapsedTime = 0.0f;
        EventSystem.current.SetSelectedGameObject(buttons[idx]);
        isArrowKeyPressed = false;
    }

    // For Button
    public void StartGameButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        AudioManager.instance.SFXPlay("Start ", AudioManager.instance.SFXLibrary[0]);
    }
    
    // For Button
    public void ExitGameButton()
    {
        isQuit = true;
        EventSystem.current.SetSelectedGameObject(null);
        AudioManager.instance.SFXPlay("Exit ", AudioManager.instance.SFXLibrary[2]);
    }
}
