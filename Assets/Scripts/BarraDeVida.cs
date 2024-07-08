using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.minValue = 0;
        slider.value = 100;
    }

    public void CambiarVida(float Vida)
    {
        slider.value = Vida;
    }
}
