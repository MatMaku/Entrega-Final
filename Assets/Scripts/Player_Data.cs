using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data instance;

    public float VidaData;
    public int TipoArmaData;
    public int BalasData;
    public int BalasGuardadasData;
    public bool HachaConseguidaData;
    public bool PistolaConseguidaData;

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
