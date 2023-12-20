using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaRange : MonoBehaviour
{
    static public bool inRange;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
