using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpSpeed, movementSpeed, movement, damage;
    bool jump, down, onGround, onLava, paused, dead;
    public bool key;
    string respawn;

    Animator anim;
    public CinemachineImpulseSource source;
    public Transform graphicsTransform;
    public Slider slider;
    public GameObject PauseMenu;

    void Awake()
    {
        jumpSpeed = 12f;
        movementSpeed = 5f;
        movement = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawn = "Respawn";
        damage = 1f;
        onLava = false;
        paused = false;
        dead = false;
        PauseMenu.SetActive(false);
        key = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        
        if (jump)
        {
            down = false;
            jump = false;
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

        if (onLava)
        {
            if(slider.value > 0)
            {
                slider.value -= damage;
            }
            else
            {
                if (!dead)
                {
                    dead = true;
                    die();
                }
            }
        }
    }

    public void die()
    {
        
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        anim.SetBool("onDie", true);
        yield return new WaitForSeconds((float)0.5);
        anim.SetBool("onDie", false);
        transform.position = GameObject.Find(respawn).transform.position;
        dead = false;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
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

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            paused = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
        if (collision.gameObject.tag == "Death")
        {
            onLava = true;
            source.GenerateImpulse();
        }
        if (collision.gameObject.tag == "Respawn")
        {
            respawn = collision.gameObject.name;
            Destroy(collision.gameObject);
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
            onLava = false;
            if (dead)
            {
                slider.value = 100f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CastleDoor")
        {
            StartCoroutine(Wait1Second());
        }
        if (collision.gameObject.tag == "DoorLevel2")
        {
            if (key)
            {
                SceneManager.LoadScene("Level3");
            }
        }
    }

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds((float)0.5);
        SceneManager.LoadScene("Level2");
    }

    public void setKey()
    {
        key = true;
    }
    public bool getKey()
    {
        return key;
    }
}
