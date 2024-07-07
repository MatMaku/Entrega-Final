using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class Controller_Player : Controller_Character
{
    public float RangoDisparo = 8f;
    public int TipoArma = 0;
    public int Balas = 7;
    public int BalasGuardadas = 14;

    private bool Apuntando = false;
    private bool Disparando = false;
    private bool Dañado = false;
    private bool Muerto = false;
    private bool Recargando = false;
    private bool SePuedeMover = true;
    private bool Cambiando = false;

    public Transform Hitbox;
    public Transform PuntoDisparo;

    void Update()
    {
        if (Input.GetButton("Fire2") && TipoArma != 0)
        {
            MoveInput = new Vector2(0, 0);
            Apuntando = true;
            SePuedeMover = false;

            if (Input.GetButtonDown("Fire1") && !Disparando && Balas > 0)
            {
                Disparando = true;
                Ataque();
                StartCoroutine(AttackWait());
                if (TipoArma == 1)
                {
                    Balas--;
                    StartCoroutine(ShootWait());
                }
                else if (TipoArma == 2)
                {
                    StartCoroutine(AttackWait());
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && BalasGuardadas > 0 && Balas < 7 && !Recargando && !Disparando && TipoArma == 1)
            {
                Recargando = true;
                StartCoroutine(Recarga());
            }
        }
        else if (!Recargando)
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

                if (Input.GetKeyDown(KeyCode.Tab) && !Cambiando)
                {
                    MoveInput = new Vector2(0, 0);
                    SePuedeMover = false;
                    Cambiando = true;
                    if (TipoArma == 2)
                    {
                        TipoArma = 1;
                    }
                    else
                    {
                        TipoArma = 2;
                    }
                    StartCoroutine(ChangeWait());
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Movimiento(MoveInput);
        Animación();
        Giro();
    }

    private void Ataque()
    {
        if (TipoArma == 1)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, RangoDisparo);

            if (raycastHit2D && raycastHit2D.transform.CompareTag("Enemigo"))
            {
                Transform hitTransform = raycastHit2D.transform;
                Controller_Enemigo Enemigo = hitTransform.GetComponentInParent<Controller_Enemigo>();

                string ParteImpactada = raycastHit2D.collider.gameObject.name;

                if (ParteImpactada == "Cabeza")
                {
                    Enemigo.TomarDaño(30);
                }
                else if (ParteImpactada == "Piernas")
                {
                    Enemigo.TomarDaño(10);
                }
                else
                {
                    Enemigo.TomarDaño(20);
                }
                Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
            }
        }
        else if (TipoArma == 2)
        {
            StartCoroutine(Attack());
            
        }
        
    }

    private void Giro()
    {
        if (MoveInput.x < 0)
        {
            Hitbox.localPosition = new Vector2(0.1f, 0f);

            PuntoDisparo.localPosition = new Vector2(-1.8f, 1.1f);
            PuntoDisparo.localRotation = new Quaternion(0, 180, 0, 0);
        }
        else if (MoveInput.x > 0)
        {
            Hitbox.localPosition = new Vector2(-0.2f, 0f);

            PuntoDisparo.localPosition = new Vector2(1.7f, 1.1f);
            PuntoDisparo.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public override void TomarDaño(float Daño)
    {
        if (!Dañado)
        {
            MoveInput = new Vector2(0, 0);
            Dañado = true;
            SePuedeMover = false;
            base.TomarDaño(Daño);
            if (Vida <= 0)
            {
                Muerto = true;
                Dañado = false;
            }
            else
            {
                StartCoroutine(DamageCooldown());
            }
        }
    }

    public override void Animación()
    {

        Animator.SetInteger("TipoArma", TipoArma);

        switch (TipoArma)
        {
            case 0:
                if (MoveInput.x < 0)
                {
                    CambioDeCapa(1);
                }
                else if (MoveInput.x > 0)
                {
                    CambioDeCapa(0);
                }
                break;

            case 1:
                if (MoveInput.x < 0)
                {
                    CambioDeCapa(3);
                }
                else if (MoveInput.x > 0)
                {
                    CambioDeCapa(2);
                }
                break;

            case 2:
                if (MoveInput.x < 0)
                {
                    CambioDeCapa(5);
                }
                else if (MoveInput.x > 0)
                {
                    CambioDeCapa(4);
                }
                break;
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

        if (Dañado == true)
        {
            Animator.SetBool("Daño", true);
        }
        else
        {
            Animator.SetBool("Daño", false);
        }

        if (Recargando == true)
        {
            Animator.SetBool("Recargando", true);
        }
        else
        {
            Animator.SetBool("Recargando", false);
        }

        if (Cambiando == true)
        {
            Animator.SetBool("Cambiando", true);
        }
        else
        {
            Animator.SetBool("Cambiando", false);
        }

        if (Muerto == true)
        {
            Animator.SetBool("Muerte", true);
        }
    }

    private void CambioDeCapa(int Capa)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            if (i == Capa)
            {
                Animator.SetLayerWeight(i, 1);
            }
            else
            {
                Animator.SetLayerWeight(i, 0);
            }
        }
    }

    private IEnumerator AimWait()
    {
        yield return new WaitForSeconds(.4f);
        SePuedeMover = true;
    }

    private IEnumerator ShootWait()
    {
        yield return new WaitForSeconds(.5f);
        Disparando = false;
    }

    private IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(.4f);
        Disparando = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, 1);

        if (raycastHit2D && raycastHit2D.transform.CompareTag("Enemigo"))
        {
            Transform hitTransform = raycastHit2D.transform;
            Controller_Enemigo Enemigo = hitTransform.GetComponentInParent<Controller_Enemigo>();

            Enemigo.TomarDaño(20);

            Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
        }
    }

    private IEnumerator ChangeWait()
    {
        yield return new WaitForSeconds(1f);
        SePuedeMover = true;
        Cambiando = false;
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        Dañado = false;
        SePuedeMover = true;
    }

    private IEnumerator Recarga()
    {
        yield return new WaitForSeconds(0.8f);
        while (Balas < 7 && BalasGuardadas > 0)
        {
            Balas++;
            BalasGuardadas--;
        }
        Recargando = false;
    }
}
