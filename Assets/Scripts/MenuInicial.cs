using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        if(!AudioManager.instance.Reproduciendo())
            AudioManager.instance.PlayBackgroundMusic(AudioManager.instance.introMusic);
    }

    public void Jugar()
    {
        PlayerPrefs.SetInt("maquina", 0);
        SceneManager.LoadScene("SAMPLESCENE");
    }

    public void JugarMaquina()
    {
        PlayerPrefs.SetInt("maquina", 1);
        SceneManager.LoadScene("SAMPLESCENE");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("OPCIONES");
    }
    
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
