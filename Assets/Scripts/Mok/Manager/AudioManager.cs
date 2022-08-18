using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] BGMLibrary;
    public AudioClip[] SFXLibrary;

    private AudioSource audioSource;

    private void Start()
    {
        Initialize();
        audioSource = GetComponent<AudioSource>();
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
    }

    public void SFXPlay(string sfxName, AudioClip audioClip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource _audioSource = go.AddComponent<AudioSource>();
        _audioSource.Play();
        Destroy(go, audioClip.length);
    }

    public void BGMPlay(AudioClip audioClip, bool isLoop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    
}
