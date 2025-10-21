using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundMusic;

    public AudioClip introMusic;

    private bool reproduciendo = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        //backgroundMusic.clip = musicClip;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
        reproduciendo = true;
    }

    // Método para detener la música de fondo
    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public bool Reproduciendo()
    {
        return reproduciendo;
    }
}
