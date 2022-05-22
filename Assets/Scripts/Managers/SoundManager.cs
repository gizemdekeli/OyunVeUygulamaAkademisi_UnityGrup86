using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [SerializeField] private AudioSource _musicSource, _effectSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySoundEffect(AudioClip audioClip)
    {
        _effectSource.PlayOneShot(audioClip);
    }
    public void PlayMusic()
    {
        _musicSource.Play();
    }
    public void PauseMusic()
    {
        _musicSource.Pause();
    }
}
