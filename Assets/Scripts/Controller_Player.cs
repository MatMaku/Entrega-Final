using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public float Speed = 1f;

    private bool Apuntando = false;
    private bool Disparando = false;
    private bool SePuedeMover = true;

    private Rigidbody2D Rigidbody2D;
    private SpriteRenderer SpriteRenderer;
    private Animator Animator;
    private Vector2 MoveInput;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            MoveInput = new Vector2(0, 0);
            Apuntando = true;
            SePuedeMover = false;

            if (Input.GetButtonDown("Fire1"))
            {
                Disparando = true;
                StartCoroutine(ShootWait());
            }
        }
        else
        {
            if (Apuntando && !Disparando)
            {
                Apuntando = false;

                StartCoroutine(AimWait());
            }

            if (SePuedeMover)
            {

                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");

                MoveInput = new Vector2(moveX, moveY).normalized;
            }
        }
    }

    private void FixedUpdate()
    {
        Movimiento();
        Animación();
    }

    private void Movimiento()
    {
        Rigidbody2D.MovePosition(Rigidbody2D.position + MoveInput * Speed * Time.fixedDeltaTime);
    }

    private void Animación()
    {
        if (MoveInput.x < 0)
        {
            Animator.SetLayerWeight(0, 0);
            Animator.SetLayerWeight(1, 1);
        }
        else if (MoveInput.x > 0)
        {
            Animator.SetLayerWeight(0, 1);
            Animator.SetLayerWeight(1, 0);
        }

        if (MoveInput != new Vector2(0, 0))
        {
            Animator.SetBool("EnMovimiento", true);
        }
        else
        {
            Animator.SetBool("EnMovimiento", false);
        }   

        if (Apuntando)
        {
            Animator.SetBool("Apuntando", true);
        }
        else
        {
            Animator.SetBool("Apuntando", false);
        }

        if (Disparando)
        {
            Animator.SetBool("Disparando", true);
        }
        else
        {
            Animator.SetBool("Disparando", false);
        }
    }

    private IEnumerator AimWait()
    {
        yield return new WaitForSeconds(.3f);
        SePuedeMover = true;
    }

    private IEnumerator ShootWait()
    {
        yield return new WaitForSeconds(.1f);
        Disparando = false;
    }
}
