using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Move")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    bool canJump;
    float moveX;
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
        JumpCheck();
    }

    private void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveX*speed, rigid.velocity.y);

        if (moveX != 0)
            sprite.flipX = moveX < 0 ? true : false;

        anim.SetBool("Run", moveX != 0);
    }

    void JumpCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down*2.1f);
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, 2.1f, LayerMask.GetMask("Ground"));
        if(ground.collider == null)
        {
            canJump= false;
        }
        else
        {
            canJump= true;
            Jump();
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigid.velocity = Vector2.up * jumpPower;
        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }

}
