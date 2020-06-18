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
    
    
   
 
    private enum State  {idle , running,jumping,falling,hurt }
    private State state = State.idle;
    private Collider2D coll;
   [SerializeField] private LayerMask ground;
   [SerializeField] private float speed = 5f;
   [SerializeField] private float jumpForce = 10f; 
    [SerializeField] private float hurtForce = 10f; 
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

         //PernamentUI.perm.healthAmount.text = PernamentUI.perm.health.ToString();
         
       
        

        
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable") 
        {
 

            cherry.Play();
            Destroy(collision.gameObject);
            PernamentUI.perm.cherries += 1;
            PernamentUI.perm.cherryText.text = PernamentUI.perm.cherries.ToString();
        }
        if(collision.tag =="PowerUp")
        {
            Destroy(collision.gameObject);
            jumpForce = 20f;
            GetComponent<SpriteRenderer>().color = Color.magenta;
            StartCoroutine(ResetPower()); 
            

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                enemy.JumpedOn();
               
                Jump();
            }
            else
            {
                state = State.hurt;
                HandleHealth();

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                     
                }
                else
                {

                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);

                }

            }

        }
        
    }

    private void HandleHealth()
    {
        PernamentUI.perm.health -= 1;
        PernamentUI.perm.healthAmount.text = PernamentUI.perm.health.ToString();
        if (PernamentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
 
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
           
        }
        else
        {
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState ()
    {



         if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }

        }
        else if( state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
         else if ( state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle; 
            }
        }
        
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            
            state = State.running; 
        }
         else
        {
            state = State.idle; 
        }


    }

    private void FootStep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 15;
        GetComponent<SpriteRenderer>().color = Color.white; 
    }
}
   