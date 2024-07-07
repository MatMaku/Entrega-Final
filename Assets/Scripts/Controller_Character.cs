using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller_Character : MonoBehaviour
{
    public float Vida = 100f;
    public float Velocidad = 1f;

    public GameObject EfectoImpacto;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;
    protected Vector2 MoveInput;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    public virtual void TomarDaño(float Daño)
    {
        Vida -= Daño;
    }

    public virtual void Animación(){}

    public void Movimiento(Vector2 Move)
    {
        Rigidbody2D.MovePosition(Rigidbody2D.position + Move * Velocidad * Time.fixedDeltaTime);
    }
}
