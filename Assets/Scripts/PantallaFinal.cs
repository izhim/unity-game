using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaFinal : MonoBehaviour
{
    public GameObject ganador;
    public int tiempo = 10;
    private string prefsNombre = "nombre";


    void Start()
    {
        ganador.GetComponent<TextMeshProUGUI>().text =
            PlayerPrefs.GetString(prefsNombre);
        //StartCoroutine(Reinicio());
        Invoke("CongelarPantalla", 0.2f);
        //SceneManager.LoadScene("PANTALLAINICIAL");
    }


    private void CongelarPantalla()
    {
        Time.timeScale = 0f;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < tiempo) { }
        //SceneManager.LoadScene("PANTALLAINICIAL");
    }
    IEnumerator Reinicio()
    {
        yield return new WaitForSeconds(tiempo);
    }

    public void Volver()
    {
        SceneManager.LoadScene("PANTALLAINICIAL");
    }
}
