using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Move")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] int jumpCount;
    float moveX;
    bool isGround;
    bool isLongJump;

    [Header("# KnockBack")]
    [SerializeField] float knockbackPower;
    float dirX;
    bool isKnockBack;


    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    WaitForSeconds kncokBackTime;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        kncokBackTime = new WaitForSeconds(0.15f);
    }

    void Update()
    {
        if (isKnockBack)
            StartCoroutine(KnockBack());
        else
        {
            Move();
            JumpCheck();
        }
    }
    IEnumerator KnockBack()
    {
        //rad = deg * Mathf.Deg2Rad;
        //rigid.velocity = new Vector2(dirX * Mathf.Cos(rad), Mathf.Sin(rad)) * knockbackPower;
        //Debug.Log(Mathf.Cos(rad) + " / " + Mathf.Sin(rad));
        rigid.velocity = new Vector2(dirX * knockbackPower, knockbackPower);
        //rigid.AddForce(new Vector2(dirX * knockbackPower, knockbackPower), ForceMode2D.Impulse);
        yield return kncokBackTime;

        isKnockBack= false;
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
        Debug.DrawRay(transform.position-new Vector3(0.5f,2f,0), Vector2.right);
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, 2.1f, LayerMask.GetMask("Ground"));

        isGround = ground.collider == null ? false : true;

        if (isGround)
        {
            jumpCount = 1; //임시코드 수정해야함
            Jump();
        }
        else
        {
            SpareJump();
        }

        //점프 세기 조절
        if (Input.GetKeyUp(KeyCode.Space))
            isLongJump = false;

        if (isLongJump)
        {
            rigid.gravityScale = 5;
        }
        else
        {
            rigid.gravityScale = 8;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLongJump=true;
            rigid.velocity = Vector2.up * jumpPower;
            //rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }
    void SpareJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rigid.velocity = Vector2.up * jumpPower;
            jumpCount--;
        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.coin++;
        }

        if (collision.CompareTag("End"))
        {
            GameManager.Instance.NextStage();
        }

        if (collision.CompareTag("Enemy"))
        {
            isKnockBack= true;
            Vector3 knockbackVec = (collision.transform.position - transform.position);
            dirX = knockbackVec.x < 0 ? dirX = 1 : dirX = -1;
        }
    }
}
