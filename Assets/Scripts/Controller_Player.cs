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
    public int BalasE;
    public int BalasEGuardadas;
    public bool HachaConseguida;
    public bool PistolaConseguida;
    public bool EscopetaConseguida;

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

    public AudioClip Disparo;
    public AudioClip DisparoE;
    public AudioClip Reload;
    public AudioClip Pickup;
    public AudioClip Swing;
    public AudioClip Empty;

    public Transform Hitbox;
    public Transform PuntoDisparo;
    public BarraDeVida barraDeVida;

    public GameObject Puzzle;
    public GameObject HachaCanvas;
    public GameObject PistolaCanvas;
    public GameObject EscopetaCanvas;
    public TextMeshProUGUI BalasText;
    public TextMeshProUGUI BalasEText;

    private void Awake()
    {
        Vida = Player_Data.instance.VidaData;
        TipoArma = Player_Data.instance.TipoArmaData;
        Balas = Player_Data.instance.BalasData;
        BalasGuardadas = Player_Data.instance.BalasGuardadasData;
        BalasE = Player_Data.instance.BalasEData;
        BalasEGuardadas = Player_Data.instance.BalasEGuardadasData;
        HachaConseguida = Player_Data.instance.HachaConseguidaData;
        PistolaConseguida = Player_Data.instance.PistolaConseguidaData;
        EscopetaConseguida = Player_Data.instance.EscopetaConseguidaData;

        ActualizarUI();
        StartCoroutine(Iniciar());
    }

    void Update()
    {
        ActualizarUI();
        ActualizarData();

        if (Input.GetButton("Fire2") && TipoArma == 1 || Input.GetButton("Fire2") && TipoArma == 2)
        {
            MoveInput = new Vector2(0, 0);
            Apuntando = true;
            SePuedeMover = false;

            if (Input.GetButtonDown("Fire1") && !Disparando)
            {
                switch (TipoArma)
                {
                    case 1:
                        if (Balas > 0)
                        {
                            Disparando = true;
                            Ataque();
                            Balas--;
                            StartCoroutine(ShootWait());
                            StartCoroutine(ShootAnimationDelay());
                        }
                        else
                        {
                            audioSource.PlayOneShot(Empty);
                        }
                        break;
                    case 2:
                        if (BalasE > 0)
                        {
                            Disparando = true;
                            Ataque();
                            BalasE--;
                            StartCoroutine(ShootWait());
                            StartCoroutine(ShootAnimationDelay());
                        }
                        else
                        {
                            audioSource.PlayOneShot(Empty);
                        }
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && !Recargando && !Disparando)
            {
                switch (TipoArma)
                {
                    case 1:
                        if (BalasGuardadas > 0 && Balas < 7)
                        {
                            Recargando = true;
                            audioSource.PlayOneShot(Reload);
                            StartCoroutine(Recarga());
                        }
                        break;
                    case 2:
                        if (BalasEGuardadas > 0 && BalasE < 2)
                        {
                            Recargando = true;
                            audioSource.PlayOneShot(Reload);
                            StartCoroutine(Recarga());
                        }
                        break;
                }
            }
        }
        else if (Input.GetButtonDown("Fire1") && TipoArma == 0 && !Disparando)
        {
            MoveInput = new Vector2(0, 0);
            SePuedeMover = false;
            Disparando = true;
            StartCoroutine(ShootAnimationDelay());
            Ataque();
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
                    switch (TipoArma)
                    {
                        case 0:
                            if (PistolaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 1;
                                StartCoroutine(ChangeWait());
                            }
                            else if (EscopetaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 2;
                                StartCoroutine(ChangeWait());
                            }
                            break;
                        case 1:
                            if (EscopetaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 2;
                                StartCoroutine(ChangeWait());
                            }
                            else if (HachaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 0;
                                StartCoroutine(ChangeWait());
                            }
                            break;
                        case 2:
                            if (HachaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 0;
                                StartCoroutine(ChangeWait());
                            }
                            else if (PistolaConseguida)
                            {
                                MoveInput = new Vector2(0, 0);
                                SePuedeMover = false;
                                Cambiando = true;
                                TipoArma = 1;
                                StartCoroutine(ChangeWait());
                            }
                            break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && HachaEnContacto != null)
        {
            HachaConseguida = true;
            Destroy(HachaEnContacto);
            HachaEnContacto = null;
            audioSource.PlayOneShot(Pickup);
        }
        if (Input.GetKeyDown(KeyCode.E) && BalasEnContacto != null)
        {
            BalasGuardadas += 5;
            Destroy(BalasEnContacto);
            BalasEnContacto = null;
            audioSource.PlayOneShot(Pickup);
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
            audioSource.PlayOneShot(Pickup);
        }
        if (Input.GetKeyDown(KeyCode.E) && MaletinEnContacto != null)
        {
            Time.timeScale = 0f;
            Puzzle.SetActive(true);
            audioSource.PlayOneShot(Pickup);
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
        Player_Data.instance.BalasEData = BalasE;
        Player_Data.instance.BalasEGuardadasData = BalasEGuardadas;
        Player_Data.instance.HachaConseguidaData = HachaConseguida;
        Player_Data.instance.PistolaConseguidaData = PistolaConseguida;
        Player_Data.instance.EscopetaConseguidaData = EscopetaConseguida;
    }

    private void ReiniciarData()
    {
        Player_Data.instance.VidaData = 100;
        Player_Data.instance.TipoArmaData = 0;
        Player_Data.instance.BalasData = 7;
        Player_Data.instance.BalasGuardadasData = 0;
        Player_Data.instance.BalasEData = 2;
        Player_Data.instance.BalasEGuardadasData = 6;
        Player_Data.instance.HachaConseguidaData = false;
        Player_Data.instance.PistolaConseguidaData = true;
        Player_Data.instance.EscopetaConseguidaData = true;
    }

    private void ActualizarUI()
    {
        switch (TipoArma)
        {
            case 0:
                HachaCanvas.SetActive(true);
                PistolaCanvas.SetActive(false);
                EscopetaCanvas.SetActive(false);
                break;
            case 1:
                PistolaCanvas.SetActive(true);
                HachaCanvas.SetActive(false);
                EscopetaCanvas.SetActive(false);
                BalasText.text = Balas + "/" + BalasGuardadas;
                break;
            case 2:
                EscopetaCanvas.SetActive(true);
                PistolaCanvas.SetActive(false);
                HachaCanvas.SetActive(false);
                BalasEText.text = BalasE + "/" + BalasEGuardadas;
                break;
        }
    }

    private void Ataque()
    {
        switch (TipoArma)
        {
            case 0:
                StartCoroutine(Attack());
                audioSource.PlayOneShot(Swing);
                break;
            case 1:
                RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, RangoDisparo);
                audioSource.PlayOneShot(Disparo);

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
                break;
            case 2:
                RaycastHit2D raycastHit2D2 = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, RangoDisparo);
                audioSource.PlayOneShot(DisparoE);

                if (raycastHit2D2 && raycastHit2D2.transform.CompareTag("Enemigo"))
                {
                    Transform hitTransform = raycastHit2D2.transform;
                    Controller_Enemigo Enemigo = hitTransform.GetComponentInParent<Controller_Enemigo>();

                    string ParteImpactada = raycastHit2D2.collider.gameObject.name;

                    if (ParteImpactada == "Cabeza")
                    {
                        Enemigo.TomarDaño(100);
                    }
                    else if (ParteImpactada == "Piernas")
                    {
                        Enemigo.TomarDaño(30);
                    }
                    else
                    {
                        Enemigo.TomarDaño(50);
                    }
                    Instantiate(EfectoImpacto, raycastHit2D2.point, Quaternion.identity);
                }
                break;
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

        if (MoveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (MoveInput.x > 0)
        {
            spriteRenderer.flipX = false;
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

        if (Dañado == true)
        {
            Animator.SetBool("Daño", true);
        }
        else
        {
            Animator.SetBool("Daño", false);
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

    private IEnumerator ShootAnimationDelay()
    {
        Animator.SetBool("Disparando", true);
        yield return new WaitForSeconds(.15f);
        Animator.SetBool("Disparando", false);
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

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.6f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(PuntoDisparo.position, PuntoDisparo.right, 1);

        if (raycastHit2D && raycastHit2D.transform.CompareTag("Enemigo"))
        {
            Transform hitTransform = raycastHit2D.transform;
            Controller_Enemigo Enemigo = hitTransform.GetComponentInParent<Controller_Enemigo>();

            Enemigo.TomarDaño(20);

            Instantiate(EfectoImpacto, raycastHit2D.point, Quaternion.identity);
        }

        SePuedeMover = true;
        Disparando = false;
    }

    private IEnumerator ChangeWait()
    {
        yield return new WaitForSeconds(.8f);
        switch (TipoArma)
        {
            case 0:
                CambioDeCapa(0);
                break;
            case 1:
                CambioDeCapa(1);
                break;
            case 2:
                CambioDeCapa(2);
                break;
        }
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
        ReiniciarData();
        SceneManager.LoadScene("Level 1");
    }

    private IEnumerator Recarga()
    {
        yield return new WaitForSeconds(0.8f);
        switch (TipoArma)
        {
            case 1:
                while (Balas < 7 && BalasGuardadas > 0)
                {
                    Balas++;
                    BalasGuardadas--;
                }
                Recargando = false;
                break;
            case 2:
                while (BalasE < 2 && BalasEGuardadas > 0)
                {
                    BalasE++;
                    BalasEGuardadas--;
                }
                Recargando = false;
                break;
        }
        
    }

    private IEnumerator Iniciar()
    {
        yield return new WaitForSeconds(.1f);
        switch (TipoArma)
        {
            case 0:
                CambioDeCapa(0);
                break;
            case 1:
                CambioDeCapa(1);
                break;
            case 2:
                CambioDeCapa(2);
                break;
        }
    }
}
