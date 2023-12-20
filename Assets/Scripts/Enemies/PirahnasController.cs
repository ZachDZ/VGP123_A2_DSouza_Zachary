using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PirahnasController : MonoBehaviour
{
    // Points
    public GameObject pointA;
    public GameObject pointB;

    // Component References
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    Transform currentPoint;
    
    // Piranha //
    public float piranhaSpeed = 0.0f;
    public bool isMovingUp = false;
    public bool isMovingDown = false;

    public bool range = false;

    public bool fire = false;

    public float pauseTime = 1.60f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        range = PiranhaRange.inRange;

        anim.SetBool("isMoving", isMovingUp);

        anim.SetBool("FireUp", fire);

        if (!range)
        {
            fire = false;
            isMovingUp = true;
        }
        else
        {
            isMovingUp = false;
            fire = true;
        }

        /*if (!isMovingDown)
        {
            isMovingUp = true;
        }
        else
        {
            isMovingUp = false;
        }

        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform && isMovingUp)
        {
            rb.velocity = new Vector2(0, piranhaSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, -piranhaSpeed);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
            StartCoroutine(MoveDown());
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            StartCoroutine(MoveUp());
        }*/
    }

    public IEnumerator MoveUp()
    {
        piranhaSpeed = 0.0f;

        yield return new WaitForSeconds(pauseTime);

        isMovingDown = false;
        isMovingUp = true;

        piranhaSpeed = 1.0f;
    }

    public IEnumerator MoveDown()
    {
        piranhaSpeed = 0.0f;

        anim.SetBool("FireUp", true);

        yield return new WaitForSeconds(pauseTime);

        anim.SetBool("FireUp", false);

        isMovingUp = false;
        isMovingDown = true;

        if (range)
        {
            piranhaSpeed = 1.0f;
        }
        else
        {
            piranhaSpeed = 0.0f;
        } 
    }

    public void Shoot()
    {
        anim.SetBool("FireUp", true);

        new WaitForSeconds(pauseTime);

        anim.SetBool("FireUp", false);
    }
}