using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemigo : MonoBehaviour
{
    public float Vida = 100f;
    public float TiempoDeRutina = 1;
    public float VelocidadCaminar = 2;
    public float VelocidadCorrer = 4;
    public GameObject Vision;

    private int Rutina;
    private int Dirección;
    private float Cronometro = 0;
    private bool Alertado;
    private Transform Jugador;

    private Rigidbody2D Rigidbody2D;
    private Vector2 Move;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Cronometro +=Time.deltaTime;

        if (Cronometro >= TiempoDeRutina)
        {
            Cronometro = 0;
            Rutina = Random.Range(0, 3);
        }
    }
    public void FixedUpdate()
    {
        Comportamiento();
        Giro();
    }

    public void Comportamiento()
    {
        if (!Alertado) 
        { 
            switch (Rutina)
            {
                case 0:
                    Move = new Vector2(0, 0);
                    break;
                case 1:
                    Dirección = Random.Range(0, 2);
                    Rutina++;
                    break;
                case 2:
                    switch (Dirección)
                    {
                       case 0:
                           Move = new Vector2(1, 0);
                           break;
                       case 1:
                          Move = new Vector2(-1, 0);
                          break;
                    }
                    break;
            }

            Rigidbody2D.MovePosition(Rigidbody2D.position + Move * VelocidadCaminar * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 playerPosition = new Vector2(Jugador.position.x, Jugador.position.y);
            float distancia = Vector2.Distance(Rigidbody2D.position, playerPosition);

            if (distancia > 2)
            {
                Vector2 direccion = (playerPosition - Rigidbody2D.position).normalized;
                Move = direccion * VelocidadCorrer * Time.fixedDeltaTime;
                Rigidbody2D.MovePosition(Rigidbody2D.position + Move);
            }
        }
    }

    private void Giro()
    {
        if (Move.x < 0)
        {
            Vision.transform.localPosition = new Vector2(-2f, 0f);
        }
        else if (Move.x > 0)
        {
            Vision.transform.localPosition = new Vector2(4f, 0f);
        }
    }

    public void TomarDaño(float Daño)
    {
        Vida -= Daño;
        if (Vida <=0)
        {
            Destroy(gameObject);
        }
        if (!Alertado)
        {
            Rutina = 2;
            if (Dirección == 0)
            {
                Dirección = 1;
            }
            else
            {

                Dirección = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Jugador = collision.transform;
            Alertado = true;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Jugador = null;
    //        Alertado = false;
    //    }
    //}

}
