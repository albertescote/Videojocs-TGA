using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPlatformMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed, movement;

    void Awake()
    {
        movementSpeed = 2f;
        movement = 1f;
        rb = GetComponent<Rigidbody2D>();
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (movement.Equals(1))
            {
                movement = -1f;
            }
            else
            {
                movement = 1f;
            }
        }
    }
}
