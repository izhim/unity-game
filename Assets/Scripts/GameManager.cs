using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject bola;
    
    // jugadores
    public GameObject delantero1;
    public GameObject delantero2;
    public GameObject portero1;
    public GameObject portero2;

    // líneas de gol
    public GameObject gol1;
    public GameObject gol2;

    // textos del ganador
    public GameObject marcador1;
    public GameObject marcador2;
    public GameObject ganador;

    // materiales
    public GameObject goCampo;
    public GameObject goBola;

    // parámetros varios
    public int goles = 5;
    public float tiempoMensaje = 3;
    public int cuentaAtras = 3;

    // variables locales
    private int puntuacion1;
    private int puntuacion2;
    private string prefsNombre = "nombre";
    private AudioSource sonidoGanar;
    private string nomJugador1;
    private string nomJugador2;



    private void Start()
    {
        puntuacion1 = 0;
        puntuacion2 = 0;
        if (AudioManager.instance != null)
            AudioManager.instance.StopBackgroundMusic();
        Destroy(AudioManager.instance);
        EstablecerPlayerPrefs();
        sonidoGanar = GetComponent<AudioSource>();
        StartCoroutine(Cuenta());
    }

    IEnumerator Cuenta()
    {
        for (int i = cuentaAtras; i > 0; i--)
        {
            ganador.GetComponent<TextMeshProUGUI>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        ganador.GetComponent<TextMeshProUGUI>().text = "VAMOS!!";
        yield return new WaitForSeconds(1);
        ganador.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void Pausa(int tiempo)
    {
        Time.timeScale = 0f;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < tiempo) { }
    }

    public int GetCuentaAtras()
    {
        return cuentaAtras;
    }


    public void GolJugador1()
    {
        puntuacion1 ++;
        marcador1.GetComponent<TextMeshProUGUI>().text = puntuacion1.ToString();
        if (puntuacion1 == goles)
            Ganador(nomJugador1, true);
        else
            Resetear(-1, false);
    }

    public void GolJugador2()
    {
        puntuacion2 ++;
        marcador2.GetComponent<TextMeshProUGUI>().text = puntuacion2.ToString();
        if (puntuacion2 == goles)
            Ganador(nomJugador2, false);
        else
            Resetear(1, false);
    }

    private void Resetear(int sentido, bool hayGanador)
    {
        bola.GetComponent<Bola>().Reset(sentido);
        delantero1.GetComponent<Jugadores>().Reset();
        delantero2.GetComponent<Jugadores>().Reset();
        portero1.GetComponent<Jugadores>().Reset();
        portero2.GetComponent<Jugadores>().Reset();
        if(!hayGanador)
            StartCoroutine(Cuenta());
    }

    

    private void Ganador(string nombre, bool jugador1)
    {
        // guardamos el nombre del ganador para mostrarlo en
        // la pantalla final
        PlayerPrefs.SetString(prefsNombre, nombre);
        sonidoGanar.Play();
        ganador.GetComponent<TextMeshProUGUI>().color =
            jugador1 ?
            marcador1.GetComponent<TextMeshProUGUI>().color :
            marcador2.GetComponent<TextMeshProUGUI>().color;
        ganador.GetComponent<TextMeshProUGUI>().text =
            "GANADOR\n" +
            nombre;
        Resetear(0, true);
        StopAllCoroutines();
        Invoke("CongelarPantalla", 0.2f);
    }

    private void CongelarPantalla()
    {
        Time.timeScale = 0f;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < tiempoMensaje) { }
        SceneManager.LoadScene("PANTALLAFINAL");
    }

    private void EstablecerPlayerPrefs()
    {
        int index = 0;
        string nomMaterial;
        Material material;
        Renderer renderer;
        Vector3 escala;

        nomJugador1 = PlayerPrefs.GetString("nomJugador1", "JUGADOR 1");
        nomJugador2 = PlayerPrefs.GetString("nomJugador2", "JUGADOR 2");
        goles = PlayerPrefs.GetInt("numGoles", 5);

        // material del campo
        index = PlayerPrefs.GetInt("indexMatCampo", 0);
        nomMaterial = string.Format("{0}{1}", "Campo", index);
        material = Resources.Load<Material>("Materials/" + nomMaterial);
        renderer = goCampo.GetComponent<Renderer>();
        renderer.material = material;

        // material de la bola
        index = PlayerPrefs.GetInt("indexMatBola", 0);
        nomMaterial = string.Format("{0}{1}", "Bola", index);
        material = Resources.Load<Material>("Materials/" + nomMaterial);
        renderer = goBola.GetComponent<Renderer>();
        renderer.material = material;

        // velocidad de las palas
        float vel = PlayerPrefs.GetFloat("velPalas", 20);
        delantero1.GetComponent<Jugadores>().SetVelocidad(vel);
        delantero2.GetComponent<Jugadores>().SetVelocidad(vel);
        portero1.GetComponent<Jugadores>().SetVelocidad(vel);
        portero2.GetComponent<Jugadores>().SetVelocidad(vel);

        // dimensión de las palas
        
        escala = delantero1.transform.localScale;
        delantero1.transform.localScale = new Vector3(
            escala.x,
            PlayerPrefs.GetFloat("dimPalas", 1.2f),
            escala.z);
        delantero2.transform.localScale = new Vector3(
            escala.x,
            PlayerPrefs.GetFloat("dimPalas", 1.2f),
            escala.z);
        portero1.transform.localScale = new Vector3(
            escala.x,
            PlayerPrefs.GetFloat("dimPalas", 1.2f),
            escala.z);
        portero2.transform.localScale = new Vector3(
            escala.x,
            PlayerPrefs.GetFloat("dimPalas", 1.2f),
            escala.z);
        
        // dimensión de la bola
        escala = goBola.transform.localScale;
        float dim = PlayerPrefs.GetFloat("dimBola", 1.2f);
        goBola.transform.localScale = new Vector3(dim, dim, dim);
    }
}
