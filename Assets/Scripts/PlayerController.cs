using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    [SerializeField] private bool PopingUp;
    [SerializeField] private GameObject PopUp;    
    [SerializeField] private GameObject PopUp2;
    [SerializeField] private GameObject PopUp3;

    [SerializeField] private GameObject Answer1;
    [SerializeField] private GameObject Answer2;
    [SerializeField] private GameObject Answer3;


    public int coins = 0;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask pipasalah;
    [SerializeField] private LayerMask pipabenar;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpforce = 7f;
    [SerializeField] private float hurtforce = 7f;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float respawnDelay;


    private enum State { idle, running, jumping, falling, hurt, squat, masuk }
    private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //popup = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(hDirection * speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(hDirection * speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetButtonDown("Jump") && (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(pipabenar) || coll.IsTouchingLayers(pipasalah)))
        {
            Jump();
        }
        if (Input.GetKey("down") && (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(pipabenar) || coll.IsTouchingLayers(pipasalah)))
        {
            rb.velocity = new Vector2(rb.velocity.x, .1f);
            state = State.squat;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(pipabenar) || coll.IsTouchingLayers(pipasalah))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt) 
        { 
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else if (state == State.squat)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.idle;
            }
            if (coll.IsTouchingLayers(pipabenar) || coll.IsTouchingLayers(pipasalah))
            {
                //state = State.masuk;
            }
        }
        else
        {
            state = State.idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "book")
        {
            PopUp.SetActive(PopingUp);
            Answer1.SetActive(PopingUp);
            Destroy(collision.gameObject);            
            
        }
        if (collision.tag == "book2")
        {
            PopUp2.SetActive(PopingUp);
            Destroy(collision.gameObject);

        }
        if (collision.tag == "book3")
        {
            PopUp3.SetActive(PopingUp);
            Destroy(collision.gameObject);

        }
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            coins += 1;
            coinText.text = coins.ToString();
        }        
    }

    //private IEnumerator ResetPopUp()
    //{
    //    yield return new WaitForSeconds(5);
    //    PopingUp = false;
    //}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                Destroy(other.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x < transform.position.x)
                {
                    rb.velocity = new Vector2(hurtforce + 1, 5);
                }
                else
                {
                    rb.velocity = new Vector2(- hurtforce - 2, 5);
                }
                loseCoins();
            }
        }
    }

    private void loseCoins()
    {
        coins -= 3;
        coinText.text = coins.ToString();   
        if (coins <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}