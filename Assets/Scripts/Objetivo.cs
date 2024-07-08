using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo : MonoBehaviour
{
    public Controller_Objetive objectiveManager;
    public string Texto;

    private bool Enviado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !Enviado)
        {
            objectiveManager.ShowObjective(Texto);
            Enviado = true;
        }
    }

}
