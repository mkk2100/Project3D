using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public bool hasEffect;
    private bool isDoneEffect;   
    private CanvasGroup canvasGroup;
    private const float EFFECTDELAY = 0.05f;
    public Image image;
    public bool isBlack;
    public enum NextZone
    {
        MainTitle,
        Zone1,
        Zone2,
        Zone3
    }
    public NextZone nextZone;
    public static TransitionManager instance;

    private void Start()
    {
        Initialize();
        SetColor();
        if(!hasEffect) return;
        StartCoroutine(ScreenEffectOn());          
    }

    private void Initialize()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(this.gameObject);
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = (hasEffect) ? 1.0f : 0.0f;
    }    

    public void SetColor()
    {
        image.color = (isBlack) ? new Color(0,0,0,1) : new Color(1,1,1,1);
    }

    public IEnumerator ScreenEffectOn()
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= EFFECTDELAY;
            yield return new WaitForEndOfFrame();
        }
        this.gameObject.SetActive(false);
    }

    public IEnumerator ScreenEffectOff()
    {
        this.gameObject.SetActive(true);
        while(canvasGroup.alpha <= 1)
        {
            canvasGroup.alpha += EFFECTDELAY;
            yield return new WaitForEndOfFrame();
        }
        this.gameObject.SetActive(false);
    }

    public void LoadNextZone(NextZone nextZone)
    {
        SceneManager.LoadScene(nextZone.ToString());
    }
}

