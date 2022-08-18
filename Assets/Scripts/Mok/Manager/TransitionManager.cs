using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public bool hasEffect;    
    private CanvasGroup canvasGroup;
    private const float EFFECTDELAY = 0.01f;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = (hasEffect) ? 1.0f : 0.0f;
    }

    private void Update()
    {
        if(!hasEffect) return;
        StartCoroutine(PlayScreenEffect());  
    }

    private IEnumerator PlayScreenEffect()
    {
        while(canvasGroup.alpha >= 0)
        {
            canvasGroup.alpha -= EFFECTDELAY;

            yield return new WaitForSeconds(EFFECTDELAY * 10.0f);
        }
        yield return null;
    }
}
