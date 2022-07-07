using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    private float speed = 2f;
    private bool facingLeft = true;

    private void Update()
    {
        anim.SetBool("rolling", true);
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                transform.localScale = new Vector3(1, 1);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                transform.localScale = new Vector3(-1, 1);
                rb.velocity = new Vector2(speed, rb.velocity.y);
                
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
