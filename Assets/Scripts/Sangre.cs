using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sangre : MonoBehaviour
{
    private float TiempoDeVida = 0;
    public float TiempoDeVidaMax = 1;

    void Update()
    {
        TiempoDeVida += Time.deltaTime;
        if (TiempoDeVida > TiempoDeVidaMax)
        {
            Destroy(gameObject);
        }
    }
}
