using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controller_Puzzle : MonoBehaviour
{
    public Transform[] Imagenes;

    public Controller_Player Player;
    public GameObject Maletin;

    public Controller_Objetive objectiveManager;
    public string Texto;

    private void Update()
    {
        if (Imagenes[0].rotation.z == 0 &&
            Imagenes[1].rotation.z == 0 &&
            Imagenes[2].rotation.z == 0 &&
            Imagenes[3].rotation.z == 0 &&
            Imagenes[4].rotation.z == 0 &&
            Imagenes[5].rotation.z == 0)
        {
            Player.PistolaConseguida = true;
            Time.timeScale = 1f;
            objectiveManager.ShowObjective(Texto);
            Destroy(Maletin);
            Destroy(this.gameObject);
        }
    }
}
