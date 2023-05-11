using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Move")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [HideInInspector] public float moveX;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveX*speed, rigid.velocity.y);

        if (moveX != 0)
            sprite.flipX = moveX < 0 ? true : false;

        anim.SetBool("Run", moveX != 0);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector2.up * jumpPower;

        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }

}
