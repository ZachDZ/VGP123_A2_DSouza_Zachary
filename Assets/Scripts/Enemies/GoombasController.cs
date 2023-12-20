using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class GoombasController : MonoBehaviour
{
    // Points
    public GameObject pointA;
    public GameObject pointB;

    // Component References
    Rigidbody2D rb;
    Animator anim;

    Transform currentPoint;
    public static Transform playerGC;

    // Goomba //
    public float goombaSpeed = 2.0f;
    public int goombaHealth = 1;
    public static bool squish = false;
    public bool isGoombaMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        squish = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        playerGC = PlayerController.groundCheck;
        anim.SetBool("isMoving", isGoombaMoving);
        anim.SetBool("isDead", squish);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(goombaSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-goombaSpeed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }

        if (squish)
        {
            goombaHealth--;

            if (goombaHealth <= 0)
            {
                anim.SetBool("isDead", squish);
                goombaSpeed = 0.0f;
                Destroy(gameObject, 2);
            }
        }
    }
}
