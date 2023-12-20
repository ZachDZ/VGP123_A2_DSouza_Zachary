using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    AudioSourceManager asm;

    // Movement Variables
    public float speed = 5.0f;

    // Jump Variables
    public float jumpForce = 625.0f;
    public float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;

    // GroundCheck Stuff
    public bool isGrounded;
    public static Transform groundCheck;
    public LayerMask isGroundLayer;
    public static float groundCheckRadius = 0.02f;

    // PowerUp Values //
    public bool super = false;

    // Pause
    public bool pause = false;
    public float pauseTime = 1.1f;

    // Falling
    public float FallingThreshold = -1f;
    public bool falling = false;

    // Audio
    public AudioClip jump;
    public AudioClip stomp;
    public AudioClip coin;
    public AudioClip redMushroom;

    // Start is called before the first frame update
    void Start()
    {
        // Getting Our Component References
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        asm = GetComponent<AudioSourceManager>();

        // Checking Variables For Dirty Data
        if (rb == null)
            Debug.Log("No Rigidbody Reference");
        if (sr == null)
            Debug.Log("No Sprite Renderer Reference");
        if (anim == null)
            Debug.Log("No Animator Reference");
        if (asm == null)
            Debug.Log("No Audio Source Manager Reference");

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            Debug.Log("GroundCheck set to default value");
        }

        if (speed <= 0)
        {
            speed = 5.0f;
            Debug.Log("Speed set to default value");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 3.00f;
            Debug.Log("jumpForce set to default value");
        }

        if (groundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float hInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (isGrounded)
        {
            rb.gravityScale = 1;
        }

        if (pause)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else
        {
            Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
            rb.velocity = moveDirection;
            rb.gravityScale = 3;
        }

        // Jump Event
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //isJumping = true;
            //jumpTimeCounter = jumpTime;
            rb.AddForce(Vector2.up * jumpForce);
            asm.PlayOneShot(jump, false);
        }

        /*if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }*/

        /*if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }*/

        // Fall Event
        if (rb.velocity.y < FallingThreshold)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

        // Attack Events
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("fire");
        }

        // Flip Event
        if (hInput != 0)
        {
            sr.flipX = (hInput > 0);
        }


        // Animation Variables
        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("super", super);
        anim.SetBool("falling", falling);
        anim.SetBool("pause", pause);

        // Health Events
        if (super && (GameManager.Instance.health < 2))
        {
            super = false;
        }
    }

    public IEnumerator Change()
    {
        yield return new WaitForSeconds(pauseTime);
        pause = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // DeathBox
        if (collision.gameObject.CompareTag("DeathBox"))
        {
            GameManager.Instance.health = 0;
        }

        if (collision.gameObject.CompareTag("Fireball"))
        {
            GameManager.Instance.health--;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Coins
        if (collision.gameObject.CompareTag("Coin"))
        {
            asm.PlayOneShot(coin, false);
            GameManager.Instance.score += 100;
            Destroy(collision.gameObject);
        }

        // Power-ups
        if (collision.gameObject.CompareTag("PowerUp_RedMushroom") && !super)
        {
            pauseTime = 1.1f;
            pause = true;
            super = true;
            asm.PlayOneShot(redMushroom, false);
            GameManager.Instance.health = 2;
            StartCoroutine(Change());

            if (super)
            {
                GameManager.Instance.score += 100;
            }

            Destroy(collision.gameObject);
        }

        // Enemies
        if (collision.gameObject.CompareTag("Goomba") && !collision.gameObject.CompareTag("Squish") && !GoombasController.squish)
        {
            GameManager.Instance.health--;
        }

        if (collision.gameObject.CompareTag("Squish"))
        {
            GoombasController.squish = true;
            asm.PlayOneShot(stomp, false);
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}