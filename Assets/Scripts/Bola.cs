using System;
using System.Collections;
using UnityEngine;
//using static UnityEditor.ShaderData;

public class Bola : MonoBehaviour
{

    // variables
    public float vel = 7;
    public float maxVel = 35;
    public Rigidbody rb;
    public float velRotacion = 100;
    public float aumentoVelocidad = 2;
    public float intervalo = 30;
    public float desvio = 3;
    public float limSuperior;
    public float limInferior;
    public float limIzquierdo;
    public float limDerecho;
    public GameObject gameManager;

    private Vector2 posInicial;
    private float tiempoTranscurrido = 0;
    private BoxCollider[] collidersJugador;

    private AudioSource[] sonidos;


    void Start()
    {
        posInicial = transform.position;
        sonidos = GetComponents<AudioSource>();
        StartCoroutine(Lanzador(0));
    }
    

    public void Reset(int sentido)
    {
        transform.position = posInicial;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = Vector3.zero;
        StartCoroutine(Lanzador(sentido));
    }

    public IEnumerator Lanzador(int sentido)
    {
        yield return new WaitForSeconds(
            gameManager.GetComponent<GameManager>().GetCuentaAtras()+1);
        
        float x = 0;
        if (sentido == 0)
            x = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        else
            x = sentido;

        float y = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        rb.AddTorque(Vector3.up * velRotacion);
        rb.velocity = new Vector2(vel * x, vel * y);
    }

    void Update()
    {
        // Aumento de la velocidad cada cierto tiempo
        tiempoTranscurrido += Time.deltaTime;
        if(tiempoTranscurrido >= intervalo && rb.velocity.magnitude < maxVel)
        {
            rb.velocity += rb.velocity.normalized * aumentoVelocidad;
            tiempoTranscurrido = 0;
        }
    }

    void OnCollisionEnter(Collision colision)
    {
        Vector3 direccionRebote = Vector3.Reflect(transform.forward, colision.contacts[0].normal);
        Quaternion rotacionRebote = Quaternion.LookRotation(direccionRebote, Vector3.up);
        transform.rotation = rotacionRebote;
        transform.Rotate(Vector3.up, velRotacion * Time.deltaTime);
        if (colision.gameObject.CompareTag("borde"))
            sonidos[0].Play();
        else if (colision.gameObject.tag.Contains("jugador"))
            sonidos[1].Play();
        else if (colision.gameObject.tag.Contains("gol"))
            sonidos[2].Play();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("gol1"))
        {
            gameManager.GetComponent<GameManager>().GolJugador2();
            sonidos[2].Play();
        }
        else if (collider.CompareTag("gol2"))
        {
            gameManager.GetComponent<GameManager>().GolJugador1();
            sonidos[2].Play();
        }
        else if (collider.CompareTag("jugador2"))
        {
            Vector3 direccionRelativa = collider.transform.position - transform.position;
            if (Vector3.Dot(direccionRelativa, collider.transform.right) < 0)
            {
                GameObject jugador = GameObject.FindGameObjectWithTag("jugador2");
                collidersJugador = jugador.GetComponents<BoxCollider>();

                if(jugador.transform.position.y > limInferior
                    && jugador.transform.position.y < limSuperior
                    && jugador.transform.position.x > limIzquierdo
                    && jugador.transform.position.x < limDerecho )
            
                    collidersJugador[1].enabled = false;
            }
        }
        else if (collider.CompareTag("jugador1"))
        {
            Vector3 direccionRelativa = collider.transform.position - transform.position;
            if (Vector3.Dot(direccionRelativa, collider.transform.right) > 0)
            {
                GameObject jugador = GameObject.FindGameObjectWithTag("jugador1");
                collidersJugador = jugador.GetComponents<BoxCollider>();
                collidersJugador[1].enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collidersJugador != null && !collidersJugador[1].enabled && rb != null && collider.tag.Contains("jugador"))
        {
            collidersJugador[1].enabled = true;
            Vector3 direccionRelativa = collider.transform.position - transform.position;
            if ((
                collider.CompareTag("jugador2")
                && (Vector3.Dot(direccionRelativa, collider.transform.right) > 0)
                )||(
                collider.CompareTag("jugador1")
                && (Vector3.Dot(direccionRelativa, collider.transform.right) < 0)
                ))
            {

                Vector3 velocidadActual = rb.velocity;
                Vector3 direccionDesvio = UnityEngine.Random.onUnitSphere.normalized;
                Vector3 fuerzaDesvio = direccionDesvio * desvio;
                rb.velocity += fuerzaDesvio;
                rb.velocity = rb.velocity.normalized * velocidadActual.magnitude;
            }
            
        }
        
    }
}
