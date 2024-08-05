using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller_Character : MonoBehaviour
{
    public float Vida;
    public float Velocidad = 1f;

    public GameObject EfectoImpacto;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;
    protected Vector2 MoveInput;
    protected AudioSource audioSource;
    protected SpriteRenderer spriteRenderer;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void TomarDa�o(float Da�o)
    {
        Vida -= Da�o;
    }

    public virtual void Animaci�n(){}

    public void Movimiento(Vector2 Move)
    {
        Rigidbody2D.MovePosition(Rigidbody2D.position + Move * Velocidad * Time.fixedDeltaTime);
    }
}
