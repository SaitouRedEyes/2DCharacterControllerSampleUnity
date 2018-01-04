using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    private float maxSpeed, groundRadius, jumpForce;
    private bool faceRight, grounded, doubleJump;
    private Animator anim;

    public Transform groundCheck;
    public LayerMask whatIsGround;

    void Start()
    {
        maxSpeed = 5.0f;
        faceRight = true;
        anim = GetComponent<Animator>();
        grounded = false;
        groundRadius = 0.2f;
        jumpForce = 500.0f;
        doubleJump = true;
    }

    //Transformações usando física
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(
            groundCheck.position, 
            groundRadius, 
            whatIsGround);
        
        anim.SetBool("ground", grounded);
        anim.SetFloat("jumpSpeed", GetComponent<Rigidbody2D>().velocity.y);

        //Pulo duplo
        if (grounded) doubleJump = false;

        //Pulo sem mudança de direção no ar
        //if (!grounded) return;

        //pegando a quantidade de movimento (de -1 à 1 em x).
        float move = Input.GetAxis("Horizontal");
        
        anim.SetFloat("speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        //Se o movimento for para direita e o personagem não estiver olhando para direita e vice-versa - Flip
        if (move > 0 && !faceRight) Flip();
        else if (move < 0 && faceRight) Flip();
    }

    //É mais preciso trabalhar com Inputs em Update
    void Update()
    {
        
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));

            //Pulo duplo
            if (!doubleJump && !grounded) doubleJump = true;
        }
    }

    private void Flip()
    {
        faceRight = !faceRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}