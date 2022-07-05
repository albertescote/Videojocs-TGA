using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpSpeed, movementSpeed, movement, timer;
    bool jump, down, onGround;

    Animator anim;
    public Transform graphicsTransform;
    Cinemachine.CinemachineCollisionImpulseSource screenShaker;

    void Awake()
    {
        jumpSpeed = 10f;
        movementSpeed = 5f;
        movement = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        screenShaker = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(timer > 0)
        //{
        //    timer -= Time.deltaTime;
        //}


    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);

        if (jump)
        {
            down = false;
            jump = false;
            //timer = 1.5f;
            rb.velocity += Vector2.up * jumpSpeed;
        }

        if (onGround)
        {
            anim.SetBool("onJump", false);
            if (movement > 0)
            {
                graphicsTransform.rotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("onWalk", true);
            }
            else if (movement < 0)
            {
                graphicsTransform.rotation = Quaternion.Euler(0, 180, 0);
                anim.SetBool("onWalk", true);
            }
            else
            {
                anim.SetBool("onWalk", false);
            }
        }
        else
        {
            anim.SetBool("onDown", false);
            down = false;
            movementSpeed = 5f;
            anim.SetBool("onJump", true);
        }

        if (down)
        {
            anim.SetBool("onDown", true);
        }
        else
        {
            anim.SetBool("onDown", false);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        //if (ctx.performed && onGround && timer <= 0)
        if (ctx.performed && onGround)
        {
            jump = true;
        }
    }

    public void OnDown(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            movementSpeed = 1.5f;
            down = true;
        }
        if (ctx.canceled)
        {
            movementSpeed = 5f;
            down = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<float>();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
        if (collision.gameObject.tag == "Death")
        {
            anim.SetBool("onDie", true);
            screenShaker.GenerateImpulse();
            StartCoroutine(Wait());
            collision.otherCollider.transform.position = GameObject.Find("Respawn").transform.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
        if (collision.gameObject.tag == "Death")
        {
            anim.SetBool("onDie", false);
        }
    }
}
