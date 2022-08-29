using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] BGMLibrary;
    public AudioClip[] SFXLibrary;

    private AudioSource audioSource1;
    private AudioSource[] audioSource2;

    private void Awake()
    {
        Initialize();
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

        string title = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if(title == "MainTitle")
        {
            gameObject.AddComponent<AudioSource>();
            audioSource2 = GetComponents<AudioSource>();
            BGMPlayOnlyTitle(BGMLibrary, true, 1);
        }
        else audioSource1 = GetComponent<AudioSource>();
    }

    public void SFXPlay(string sfxName, AudioClip audioClip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource _audioSource = go.AddComponent<AudioSource>();
        _audioSource.clip = audioClip;
        _audioSource.Play();
        Destroy(go, audioClip.length);
    }

    public void BGMPlay(AudioClip audioClip, bool isLoop, float volumeValue = 1.0f)
    {
        audioSource1.clip = audioClip;
        audioSource1.loop = isLoop;
        audioSource1.volume = volumeValue;
        audioSource1.Play();
    }

    // Warning : Not optimized yet.
    private void BGMPlayOnlyTitle(AudioClip[] audioClip, bool isLoop, float volumeValue = 1.0f)
    {
        int i = 0;
        foreach(AudioSource audioSource in audioSource2)
        {
            audioSource.clip = audioClip[i];
            audioSource.loop = isLoop;
            audioSource.volume = (i == 0) ? volumeValue : 0;
            audioSource.Play();
            i++;
        }
    }

    // For button
    public void IncreaseVolume()
    {
        StartCoroutine(SetVolume());
    }

    IEnumerator SetVolume()
    {
        while(audioSource2[1].volume < 1)
        {
            audioSource2[1].volume += 0.01f;
            yield return new WaitForEndOfFrame();
        }
    }
}
