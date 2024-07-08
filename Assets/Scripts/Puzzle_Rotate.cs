using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private void OnMouseDown()
    {
        transform.Rotate(0, 0, 90f);
    }
}
