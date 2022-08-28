using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 텍스트 입력
public class Title : MonoBehaviour
{
    [Header("Text Object")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI startGameButton;
    public TextMeshProUGUI exitGameButton;
    public TextMeshProUGUI credit;
    public TextMeshProUGUI pressEnterToStart;
    public TextMeshProUGUI load;

    [Header("Input Text for Buttons")]
    public string titleText;    
    public string startGameButtonText;
    public string exitGameButtonText;
    public string creditText;
    public string pressEnterToStartText;
    public string loadText;

    [Header("State Holder")]
    public GameObject pressEnterToStartState;
    public GameObject selectButtonState;
}
