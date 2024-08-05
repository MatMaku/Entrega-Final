using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller_Enemigo : Controller_Character
{
    public GameObject Vision;
    public Transform PuntoAtaque;

    public AudioClip EnemyRoar;
    public AudioClip Swing;

    public Collider2D mainCollider;
    public Collider2D[] childColliders;

    private bool Alertado = false;
    private bool Da�ado = false;
    private bool Muerto = false;
    private bool Atacando = false;
    private Transform Jugador;

    public void FixedUpdate()
    {
        Movimiento(MoveInput);
        Comportamiento();
        Animaci�n();
        Giro();
    }

    public void Comportamiento()
    {
        if (Alertado && !Da�ado && !Muerto && !Atacando) 
        {
            Vector2 playerPosition = new Vector2(Jugador.position.x, Jugador.position.y);
            float distancia = Vector2.Distance(Rigidbody2D.position, playerPosition);

            if (distancia > 2)
            {
                MoveInput = (playerPosition - Rigidbody2D.position).normalized;
            }
            else if (distancia <= 2 && !Atacando)
            {
                MoveInput = new Vector2(0,0);
                Atacando = true;
                StartCoroutine(Attack());
                StartCoroutine(AttackCooldown());
            }
        }
        else
        {
            MoveInput = new Vector2(0,0);
        }
    }

    private void Golpe()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoAtaque.position, PuntoAtaque.right, 1);
        audioSource.PlayOneShot(Swing);
        if (raycastHit2D && raycastHit2D.transform.CompareTag("Player"))
        {
            Transform hitTransform = raycastHit2D.transform;
            Controller_Player Player = hitTransform.GetComponentInParent<Controller_Player>();

            Player.TomarDa�o(20);

            if (Player.Vida > 0)
            {
                Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
            }
        }
    }

    private void Giro()
    {
        if (MoveInput.x < 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1,1,1);
            PuntoAtaque.localRotation = new Quaternion(0, 180, 0, 0);
        }
        else if (MoveInput.x > 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            PuntoAtaque.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public override void Animaci�n()
    {
        if (MoveInput != new Vector2(0, 0))
        {
            Animator.SetBool("Corriendo", true);
        }
        else
        {
            Animator.SetBool("Corriendo", false);
        }

        if (Atacando == true)
        {
            Animator.SetBool("Atacando", true);
        }
        else
        {
            Animator.SetBool("Atacando", false);
        }

        if (Da�ado == true)
        {
            Animator.SetBool("Da�o", true);
        }
        else
        {
            Animator.SetBool("Da�o", false);
        }

        if (Muerto == true)
        {
            Animator.SetBool("Muerto", true);
        }
        else
        {
            Animator.SetBool("Muerto", false);
        }
    }

    public override void TomarDa�o(float Da�o)
    {
        if (!Muerto)
        {
            Da�ado = true;
            base.TomarDa�o(Da�o);
            if (Vida < 0)
            {
                Muerto = true;
                mainCollider.enabled = false;
                foreach (Collider2D collider in childColliders)
                {
                        collider.enabled = false;
                }
                StartCoroutine(DestroyEnemy());
            }
            StartCoroutine(DamageCooldown());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Alertado == false)
            {
                audioSource.PlayOneShot(EnemyRoar);
            }
            Jugador = collision.transform;
            Alertado = true;
        }
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        Da�ado = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.8f);
        Golpe();
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        Atacando = false;
    }
}
