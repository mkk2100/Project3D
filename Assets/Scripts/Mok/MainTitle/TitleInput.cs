using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInput : MonoBehaviour
{
    // 메인 메뉴의 입력 관련
    public GameObject pressEnterToStart;
    private bool hasPressedEnterKey;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) hasPressedEnterKey = true;
    }

    private void FixedUpdate()
    {
        if(hasPressedEnterKey && TitleMenu.titleState == TitleState.PressEnterToStartState) ClearTexts();
    }

    private void ClearTexts()
    {
        pressEnterToStart.SetActive(false);
        hasPressedEnterKey = false;
        TitleMenu.titleState = TitleState.SelectButtonState;
        AudioManager.instance.SFXPlay("Start ", AudioManager.instance.SFXLibrary[0]);
    }
}
