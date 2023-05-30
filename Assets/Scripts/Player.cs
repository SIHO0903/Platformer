using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float tmp1, tmp2,tmp3;
    [Header("# Move")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] int jumpCount;
    [SerializeField] Transform groundCheck;
    float moveX;
    bool isGround;
    bool isLongJump;

    [Header("# KnockBack")]
    [SerializeField] float knockbackPower;
    float dirX;
    bool isKnockBack;

    [Header("# Wall Slide")]
    [SerializeField] Transform wallCheck;
    [SerializeField] bool isWallSlide;
    [SerializeField] float wallSlideSpeed;

    [Header("# Wall Jump")]
    [SerializeField] bool isWallJump;
    [SerializeField] float wallJumpTime;
    [SerializeField] float curWallJumpTime;
    [SerializeField] Vector2 wallJumpVec;

    [Header("# Etc")]
    [SerializeField] bool isEnemy;
    [SerializeField] float killBounce;
    [SerializeField] float killJumpTime;
    [SerializeField] float curKillJumpTime;







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
        isGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.9f, 0.2f), 0, LayerMask.GetMask("Ground"));
        isWallSlide = Physics2D.Raycast(wallCheck.position, transform.right, 1.1f * moveX, LayerMask.GetMask("Ground"));
        if (isKnockBack)
            StartCoroutine(KnockBack());
        else
        {
            Move();
            JumpCheck();
        }
        WallJumpTime();
        if (isEnemy)
            curKillJumpTime -= Time.deltaTime;

        if (curKillJumpTime <= 0)
        {
            isEnemy = false;
            curKillJumpTime = killJumpTime;
        }
    }

    private void WallJumpTime()
    {
        if (isWallSlide && !isGround && Input.GetKeyDown(KeyCode.Space))
        {
            isWallJump = true;
        }

        if (isWallJump)
            curWallJumpTime -= Time.deltaTime;

        if (curWallJumpTime <= 0)
        {
            isWallJump = false;
            curWallJumpTime = wallJumpTime;
        }
    }

    private void FixedUpdate()
    {
        WallSlideANDJump();

        //Hit
        if(isEnemy)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, killBounce);
        }
    }

    private void WallSlideANDJump()
    {
        if (isWallSlide && !isGround)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -wallSlideSpeed);
        }
        if (isWallJump)
        {
            rigid.velocity = new Vector2(wallJumpVec.x * -moveX, wallJumpVec.y);
        }
    }


    IEnumerator KnockBack()
    {
        rigid.velocity = new Vector2(dirX * knockbackPower, knockbackPower);

        yield return kncokBackTime;

        isKnockBack= false;
    }
    private void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveX * speed, rigid.velocity.y);

        if (moveX != 0)
            sprite.flipX = moveX < 0 ? true : false;


        anim.SetBool("Run", moveX != 0);
    }

    void JumpCheck()
    {

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
            AudioManager.instance.SFXPlayer("Jump1");
        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }

    void SpareJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rigid.velocity = Vector2.up * jumpPower*0.8f;
            jumpCount--;
            AudioManager.instance.SFXPlayer("Jump2");
        }
        anim.SetInteger("Jump", Mathf.RoundToInt(rigid.velocity.y));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.SFXPlayer("Hit");
            isEnemy =true;
            collision.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            AudioManager.instance.SFXPlayer("Coin");
            collision.gameObject.SetActive(false);
            GameManager.instance.coin++;
        }

        if (collision.CompareTag("End"))
        {
            AudioManager.instance.SFXPlayer("Clear");
            GameManager.instance.NextStage();
        }

        if (collision.CompareTag("Enemy"))
        {
            AudioManager.instance.SFXPlayer("Damaged");
            isKnockBack = true;
            Vector3 knockbackVec = (collision.transform.position - transform.position);
            dirX = knockbackVec.x < 0 ? dirX = 1 : dirX = -1;
        }
    }
}
