using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleMenu : Title
{
    public static TitleState titleState;

    private void Start()
    {
        titleState = TitleState.PressEnterToStartState;
        title.text = titleText;
        startGameButton.text = startGameButtonText;
        exitGameButton.text = exitGameButtonText;
        credit.text = creditText;
        pressEnterToStart.text = pressEnterToStartText;
    }

    public void ChangeState(int idx)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == idx);
        }        
    }

    private void Update()
    {
        switch(titleState)
        {
            case TitleState.PressEnterToStartState:
                EnablePressEnterToStartState(0);
                break;
            case TitleState.SelectButtonState:
                EnableSelectButtonState(1);
                break;
            case TitleState.TutorialState:
                EnableTutorialState(2);
                break;
        }
    }

    private void EnablePressEnterToStartState(int idx)
    {
        for(int i = 0; i < transform.childCount; i++)
        transform.GetChild(i).gameObject.SetActive(i == idx);
    }

    private void EnableSelectButtonState(int idx)
    {
        for(int i = 0; i < transform.childCount; i++)
        transform.GetChild(i).gameObject.SetActive(i == idx);
    }

    private void EnableTutorialState(int idx)
    {
        for(int i = 0; i < transform.childCount; i++)
        transform.GetChild(i).gameObject.SetActive(i == idx);
    }
}
