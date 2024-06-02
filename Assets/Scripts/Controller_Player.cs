using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public float Speed = 1f;

    private bool ContraPared;

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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector2(moveX, moveY).normalized;
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
        if (MoveInput != new Vector2(0, 0))
        {
            Animator.SetBool("EnMovimiento", true);
        }
        else
        {
            Animator.SetBool("EnMovimiento", false);
        }

        if (MoveInput.x < 0)
        {
            SpriteRenderer.flipX = true;
        }
        else if (MoveInput.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
    }
}
