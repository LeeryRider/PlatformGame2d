using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int maxJumps = 2;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool isWallHanging = false;
    private int jumpsRemaining;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if (wallJumpCooldown < 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                isWallHanging = true;
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
                anim.SetBool("Wall", true);
            }
            else
            {
                isWallHanging = false;
                body.gravityScale = 1.6f;
                anim.SetBool("Wall", false);
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jump();
    }

    private void jump()
    {
        if (isGrounded())
        {
            jumpsRemaining = maxJumps;
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 6, 3);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
            anim.SetTrigger("jump");
        }
        else if (isWallHanging) 
        {
            body.velocity = new Vector2(body.velocity.x * speed, jumpPower);
            isWallHanging = false; 
            anim.SetTrigger("jump");
        }
        else if (jumpsRemaining > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            jumpsRemaining--;
            anim.SetTrigger("jump");
        }
        else if (onWall() && jumpsRemaining == 0)
        {
            jumpsRemaining = maxJumps; 
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;

    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
