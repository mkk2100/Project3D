using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleMenu : Title
{
    private void Start()
    {
        title.text = titleText;
        startGameButton.text = startGameButtonText;
        exitGameButton.text = exitGameButtonText;
        credit.text = creditText;
    }
}
