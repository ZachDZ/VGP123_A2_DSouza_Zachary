using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime;

    // This is meant to be modifiied by the object creating the projectile.
    // Eg. the shoot script
    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
        {
            lifetime = 8.0f;

            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
            Destroy(gameObject, lifetime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
