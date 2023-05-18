using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("# Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 applyPosition;
    [SerializeField] Vector2 destination;
    [SerializeField] Vector2 startPosition;

    [Header("# Detect")]
    [SerializeField] float detectRange;
    RaycastHit2D detect;
    Vector3 rayStart;

    Vector3 dirVec;
    Rigidbody2D rigid;
    void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {    
    }
    void Update()
    {
        PlayerDetect();
    }

    private void PlayerDetect()
    {
        rayStart = transform.position - Vector3.left * -10f;

        Debug.DrawRay(rayStart, new Vector2(20, 0));
        detect = Physics2D.Raycast(rayStart, Vector2.right, detectRange, LayerMask.GetMask("Player"));

        if (detect.collider != null)
            GotoPlayer();
        else
            Move();
    }

    private void GotoPlayer()
    {
        dirVec = (GameManager.Instance.player.transform.position - transform.position).normalized;
        //Debug.Log(detect.collider.name);
        rigid.velocity = new Vector2(dirVec.x * moveSpeed ,rigid.velocity.y);
    }

    private void Move()
    {
        if (rigid.position.x <= startPosition.x)
            applyPosition = destination;
        else if (rigid.position.x >= destination.x)
            applyPosition = startPosition;
        //rigid.position = Vector2.Lerp(rigid.position, applyPosition.position, moveSpeed);
        rigid.position = Vector2.MoveTowards(rigid.position, applyPosition, moveSpeed * Time.deltaTime);
    }

}
