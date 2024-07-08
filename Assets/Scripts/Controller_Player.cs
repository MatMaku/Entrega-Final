using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class Controller_Player : Controller_Character
{
    public float RangoDisparo = 8f;
    public int TipoArma;
    public int Balas;
    public int BalasGuardadas;
    public bool HachaConseguida;
    public bool PistolaConseguida;

    private bool Apuntando = false;
    private bool Disparando = false;
    private bool Dañado = false;
    private bool Muerto = false;
    private bool Recargando = false;
    private bool SePuedeMover = true;
    private bool Cambiando = false;

    private GameObject HachaEnContacto;
    private GameObject MaletinEnContacto;
    private GameObject BalasEnContacto;
    private GameObject CuraEnContacto;

    public Transform Hitbox;
    public Transform PuntoDisparo;
    public BarraDeVida barraDeVida;

    public GameObject HachaCanvas;
    public GameObject PistolaCanvas;
    public TextMeshProUGUI BalasText;

    private void Awake()
    {
        Vida = Player_Data.instance.VidaData;
        TipoArma = Player_Data.instance.TipoArmaData;
        Balas = Player_Data.instance.BalasData;
        BalasGuardadas = Player_Data.instance.BalasGuardadasData;
        HachaConseguida = Player_Data.instance.HachaConseguidaData;
        PistolaConseguida = Player_Data.instance.PistolaConseguidaData;

        ActualizarUI();
    }

    void Update()
    {
        ActualizarUI();
        ActualizarData();

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

                if (Input.GetKeyDown(KeyCode.Tab) && !Cambiando && HachaConseguida && TipoArma < 2)
                {
                    MoveInput = new Vector2(0, 0);
                    SePuedeMover = false;
                    Cambiando = true;
                    TipoArma = 2;
                    StartCoroutine(ChangeWait());
                }
                else if (Input.GetKeyDown(KeyCode.Tab) && !Cambiando && PistolaConseguida && TipoArma == 2)
                {
                    MoveInput = new Vector2(0, 0);
                    SePuedeMover = false;
                    Cambiando = true;
                    TipoArma = 1;
                    StartCoroutine(ChangeWait());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && HachaEnContacto != null)
        {
            HachaConseguida = true;
            Destroy(HachaEnContacto);
            HachaEnContacto = null;
        }
        if (Input.GetKeyDown(KeyCode.E) && BalasEnContacto != null)
        {
            BalasGuardadas += 5;
            Destroy(BalasEnContacto);
            BalasEnContacto = null;
        }
        if (Input.GetKeyDown(KeyCode.E) && CuraEnContacto != null)
        {
            Vida += 40;
            if (Vida > 100)
            {
                Vida = 100;
            }
            barraDeVida.CambiarVida(Vida);
            Destroy(CuraEnContacto);
            CuraEnContacto = null;
        }
    }

    private void FixedUpdate()
    {
        Movimiento(MoveInput);
        Animación();
        Giro();
    }

    private void ActualizarData()
    {
        Player_Data.instance.VidaData = Vida;
        Player_Data.instance.TipoArmaData = TipoArma;
        Player_Data.instance.BalasData = Balas;
        Player_Data.instance.BalasGuardadasData = BalasGuardadas;
        Player_Data.instance.HachaConseguidaData = HachaConseguida;
        Player_Data.instance.PistolaConseguidaData = PistolaConseguida;
    }

    private void ActualizarUI()
    {
        if (TipoArma == 1)
        {
            HachaCanvas.SetActive(false);
            PistolaCanvas.SetActive(true);
            BalasText.text = Balas + "/" + BalasGuardadas;
        }
        else if (TipoArma == 2)
        {
            HachaCanvas.SetActive(true);
            PistolaCanvas.SetActive(false);
        }
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
            barraDeVida.CambiarVida(Vida);
            if (Vida <= 0)
            {
                Muerto = true;
                Dañado = false;
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hacha"))
        {
            HachaEnContacto = other.gameObject;
        }
        if (other.CompareTag("Maletin"))
        {
            MaletinEnContacto = other.gameObject;
        }
        if (other.CompareTag("Balas"))
        {
            BalasEnContacto = other.gameObject;
        }
        if (other.CompareTag("Cura"))
        {
            CuraEnContacto = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hacha"))
        {
            HachaEnContacto = null;
        }
        if (other.CompareTag("Maletin"))
        {
            MaletinEnContacto = null;
        }
        if (other.CompareTag("Balas"))
        {
            BalasEnContacto = null;
        }
        if (other.CompareTag("Cura"))
        {
            CuraEnContacto = null;
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

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Level 1");
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
