using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 VelocidadMovimiento;
    public Transform CamaraTransform;

    private Vector2 ultimaPosicion;
    private Vector2 deltaMovimiento;
    private Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        ultimaPosicion = CamaraTransform.position;
    }

    private void Update()
    {
        Vector2 nuevaPosicion = CamaraTransform.position;
        deltaMovimiento = nuevaPosicion - ultimaPosicion;
        Vector2 offset = deltaMovimiento * VelocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
        ultimaPosicion = nuevaPosicion;
    }
}
