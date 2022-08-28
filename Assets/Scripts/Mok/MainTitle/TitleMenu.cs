using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleMenu : Title
{
    public static TitleState titleState;

    [Header("GameObjects")]
    public GameObject titleTextGameObject;
    public GameObject creditTextGameObject;
    public GameObject loadingTextGameObject;

    [Header("Load Text")]
    public CanvasGroup canvasGroup;

    private const float EFFECTDELAY = 0.01f;

    private void Start()
    {
        titleState = TitleState.PressEnterToStartState;
        title.text = titleText;
        startGameButton.text = startGameButtonText;
        exitGameButton.text = exitGameButtonText;
        credit.text = creditText;
        pressEnterToStart.text = pressEnterToStartText;
        load.text = loadText;
    }


    private void Update()
    {
        switch(titleState)
        {
            case TitleState.PressEnterToStartState:
                ChangeState(0);
                break;
            case TitleState.SelectButtonState:
                ChangeState(1);
                break;
            case TitleState.NoneState:
                UnenableAllState();
                break;

        }
    }

    private void ChangeState(int idx)
    {
        for(int i = 0; i < transform.childCount; i++)
        transform.GetChild(i).gameObject.SetActive(i == idx);
    }

    private void UnenableAllState()
    {
        for(int i = 0; i < transform.childCount; i++)
        transform.GetChild(i).gameObject.SetActive(false);
    }

    // Start Buttons
    public void StartGameButton()
    {
        titleTextGameObject.SetActive(false);
        creditTextGameObject.SetActive(false);
        LoadLoadingText();
        titleState = TitleState.NoneState;
        StartCoroutine(ShowsLoadingText());
    }

    public void ExitGameButton()
    {
        Invoke("QuitGame", 1.0f);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void LoadLoadingText()
    {
        loadingTextGameObject.SetActive(true);
    }

    IEnumerator ShowsLoadingText()
    {
        while(canvasGroup.alpha <= 1)
        {
            canvasGroup.alpha += EFFECTDELAY;
            yield return new WaitForEndOfFrame();
        }        
    }
}
