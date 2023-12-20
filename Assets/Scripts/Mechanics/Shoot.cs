using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;

    public float projectileSpeed;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public Projectile projectilePrefab;

    public GameObject player;

    public bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        player = GameObject.FindWithTag("Player");

        if (projectileSpeed <= 0)
        {
            projectileSpeed = 2.5f;
        }

        if (!spawnPointLeft || !spawnPointRight || projectilePrefab)
        {
            //Debug.Log("Please set default values on " + gameObject.name);
        }
    }

    void Update()
    {
        if (transform.position.x > player.transform.position.x && facingRight)
        {
            Flip();
            facingRight = false;
        }
        else if (transform.position.x < player.transform.position.x && !facingRight)
        {
            Flip();
            facingRight = true;
        }
    }

    public void Flip()
    {
        Vector3 scaleFactor = transform.localScale;

        scaleFactor.x *= -1;

        transform.localScale = scaleFactor;
    }

    public void Fire()
    {
        if (!facingRight)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            curProjectile.speed = -projectileSpeed;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.speed = projectileSpeed;
        }
    }
}
