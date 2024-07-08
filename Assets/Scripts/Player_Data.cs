using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data instance;

    public float VidaData = 100;
    public int TipoArmaData = 0;
    public int BalasData = 7;
    public int BalasGuardadasData = 0;
    public bool HachaConseguidaData = false;
    public bool PistolaConseguidaData = false;

    private void Awake()
    {
        if (Player_Data.instance == null)
        {
            Player_Data.instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
