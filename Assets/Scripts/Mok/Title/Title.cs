using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI startGameButton;
    public TextMeshProUGUI exitGameButton;
    public TextMeshProUGUI credit;

    [Header("Input Text for Buttons")]
    public string titleText;    
    public string startGameButtonText;
    public string exitGameButtonText;
    public string creditText;  


}
