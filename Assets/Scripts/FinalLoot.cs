using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLoot : MonoBehaviour
{

    Animator anim;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void setKey()
    {
        anim.SetBool("hasKey", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.getKey())
            {
                anim.SetBool("onOpen", true);
            }
        }
    }
}
