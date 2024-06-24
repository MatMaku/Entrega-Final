using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller_Player : MonoBehaviour
{
    public float Velocidad = 1f;
    public float RangoDisparo = 10f;

    private bool Apuntando = false;
    private bool Disparando = false;
    private bool SePuedeMover = true;

    private Rigidbody2D Rigidbody2D;
    private SpriteRenderer SpriteRenderer;
    private Animator Animator;
    private Vector2 MoveInput;

    public Transform PuntoDisparo;
    public GameObject EfectoImpacto;

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

            if (Input.GetButtonDown("Fire1") && !Disparando)
            {
                Disparando = true;
                Disparo();
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
        Animaci�n();
    }

    private void Movimiento()
    {
        Rigidbody2D.MovePosition(Rigidbody2D.position + MoveInput * Velocidad * Time.fixedDeltaTime);
    }

    private void Disparo()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, RangoDisparo);

        if (raycastHit2D && raycastHit2D.transform.CompareTag("Enemigo"))
        {
            Transform hitTransform = raycastHit2D.transform;
            Controller_Enemigo Enemigo = hitTransform.GetComponentInParent<Controller_Enemigo>();

            string ParteImpactada = raycastHit2D.collider.gameObject.name;

            if (ParteImpactada == "Cabeza")
            {
                Enemigo.TomarDa�o(50);
            }
            else if (ParteImpactada == "Piernas")
            {
                Enemigo.TomarDa�o(10);
            }
            else
            {
                Enemigo.TomarDa�o(20);
            }
            Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
        }
    }

    private void Animaci�n()
    {
        if (MoveInput.x < 0)
        {
            Animator.SetLayerWeight(0, 0);
            Animator.SetLayerWeight(1, 1);

            PuntoDisparo.localPosition = new Vector2(-1.8f, 1.1f);
            PuntoDisparo.localRotation = new Quaternion(0,180,0,0);
        }
        else if (MoveInput.x > 0)
        {
            Animator.SetLayerWeight(0, 1);
            Animator.SetLayerWeight(1, 0);

            PuntoDisparo.localPosition = new Vector2(1.7f, 1.1f);
            PuntoDisparo.localRotation = new Quaternion(0, 0, 0, 0);
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
        yield return new WaitForSeconds(.3f);
        Disparando = false;
    }
}
