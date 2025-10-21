using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugadores : MonoBehaviour
{

    public bool jugador1;
    public float velocidad = 10;
    public Rigidbody rb;
    public float limSuperior;
    public float limInferior;
    public float limIzquierdo;
    public float limDerecho;
    

    public GameObject gameManager;
    public GameObject bola;
    public GameObject gol;

    private float movHorizontal;
    private float movVertical;
    private Vector3 posInicial;

    private bool pausa;
    private bool android;
    private bool ia;

    public Joystick joystickP1;
    public Joystick joystickP2;

    void Start()
    {
        ia = PlayerPrefs.GetInt("maquina", 0) == 0 ? false : true;
        posInicial = transform.position;
        android = false;
        if (Application.platform == RuntimePlatform.Android)
            android = true;
        joystickP1.gameObject.SetActive(android);
        joystickP2.gameObject.SetActive(android);
        StartCoroutine(Pausa());
    }

    public IEnumerator Pausa()
    {
        pausa = true;
        yield return new WaitForSeconds(
            gameManager.GetComponent<GameManager>().GetCuentaAtras() + 1);
        pausa = false;
    }

    void Update()
    {
        if (!pausa)
        {
            if (!android)
            {
                
                if (jugador1)
                {
                    movVertical = Input.GetAxis("VerticalP1");
                    movHorizontal = Input.GetAxis("HorizontalP1");
                }
                else
                {
                    if (ia)
                    {
                        Vector2 mov = MaquinaP2();
                        movVertical = mov.y;
                        movHorizontal = mov.x;
                    }
                    else
                    {
                        movVertical = Input.GetAxis("VerticalP2");
                        movHorizontal = Input.GetAxis("HorizontalP2");
                    }
                }
            }
            else if(android)
            {   // movimientos de joystick
                if (jugador1)
                {
                    movVertical = joystickP1.Vertical;
                    movHorizontal = joystickP1.Horizontal;
                }
                else
                {
                    if (ia)
                    {
                        Vector2 mov = MaquinaP2();
                        movVertical = mov.y;
                        movHorizontal = mov.x;
                    }
                    else
                    {
                        movVertical = joystickP2.Vertical;
                        movHorizontal = joystickP2.Horizontal;
                    }
                }
            }
            

            if (
                (rb.transform.position.x < limIzquierdo && movHorizontal < 0)
                || (rb.transform.position.x > limDerecho && movHorizontal > 0))
            {
                movHorizontal = 0;
            }

            if (
                (rb.transform.position.y < limInferior && movVertical < 0)
                || (rb.transform.position.y > limSuperior && movVertical > 0))
                movVertical = 0;

            rb.velocity = new Vector3(
                movHorizontal * velocidad,
                movVertical * velocidad,
                0f);
        }
    }

    public void SetVelocidad(float vel)
    {
        velocidad = vel;
    }

    public void Reset()
    {
        rb.velocity = Vector3.zero;
        transform.position = posInicial;
        StartCoroutine(Pausa());
    }

    private Vector2 MaquinaP2()
    {
        Vector2 movimiento = Vector2.zero;
        if (bola != null)
        {
            Vector3 posicionBola = bola.transform.position;
            movimiento.y = Mathf.Clamp(posicionBola.y - transform.position.y, -1f, 1f);
            float targetX = posicionBola.x;
            if (posicionBola.x > transform.position.x)
                targetX += Random.Range(0.2f, 0.5f);
            else
                targetX -= Random.Range(0.2f, 0.5f);
            movimiento.x = Mathf.Clamp(targetX - transform.position.x, -1f, 1f);
        }
        return movimiento;
    }

}
