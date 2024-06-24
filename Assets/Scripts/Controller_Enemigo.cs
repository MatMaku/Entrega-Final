using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemigo : MonoBehaviour
{
    public float Vida = 100f;

    public void TomarDaño(float Daño)
    {
        Vida -= Daño;
        if (Vida <=0)
        {
            Muerte();
        }
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }
}
