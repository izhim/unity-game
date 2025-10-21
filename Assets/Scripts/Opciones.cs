using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{

    // referencias a los componentes
    [SerializeField]private TMPro.TMP_InputField jugador1;
    [SerializeField] private TMPro.TMP_InputField jugador2;
    [SerializeField] private TMPro.TMP_InputField numGoles;
    [SerializeField] private TMPro.TMP_Dropdown matCampo;
    [SerializeField] private TMPro.TMP_Dropdown matBola;
    [SerializeField] private Slider velPalas;
    [SerializeField] private Slider dimPalas;
    [SerializeField] private Slider dimBola;

    void Start()
    {
        jugador1.text = PlayerPrefs.GetString("nomJugador1", "JUGADOR 1");
        jugador2.text = PlayerPrefs.GetString("nomJugador2", "JUGADOR 2");
        numGoles.text = PlayerPrefs.GetInt("numGoles", 5).ToString();
        matCampo.value = PlayerPrefs.GetInt("indexMatCampo", 0);
        matBola.value = PlayerPrefs.GetInt("indexMatBola", 0);
        velPalas.value = PlayerPrefs.GetFloat("velPalas", 20);
        dimPalas.value = PlayerPrefs.GetFloat("dimPalas", 1.5f);
        dimBola.value = PlayerPrefs.GetFloat("dimBola", 0.45f);
    }

    // al pulsar Volver guardamos los datos y volvemos al inicio
    public void Volver()
    {
        if (!string.IsNullOrEmpty(jugador1.text))
            PlayerPrefs.SetString("nomJugador1", jugador1.text);
        if (!string.IsNullOrEmpty(jugador2.text))
            PlayerPrefs.SetString("nomJugador2", jugador2.text);
        PlayerPrefs.SetInt("numGoles", int.Parse(numGoles.text));
        PlayerPrefs.SetInt("indexMatCampo", matCampo.value);
        PlayerPrefs.SetInt("indexMatBola", matBola.value);
        PlayerPrefs.SetFloat("velPalas", velPalas.value);
        PlayerPrefs.SetFloat("dimPalas", dimPalas.value);
        PlayerPrefs.SetFloat("dimBola", dimBola.value);
        PlayerPrefs.Save();

        SceneManager.LoadScene("PANTALLAINICIAL");
    }
}
