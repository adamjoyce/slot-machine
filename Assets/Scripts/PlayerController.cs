using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public float jumpSpeed;

    private Rigidbody2D rb;
    private JumpStatus jumpStatus;
    private enum JumpStatus
    {
        GROUND,
        JUMPING,
        DOUBLEJUMPING
    };
    private GameObject wallColl;
    public float wallJumpCD;
    private float wallJumpcurrentCD;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpStatus = JumpStatus.GROUND;
        wallJumpcurrentCD = -1;
    }

    void Update()
    {
        bool keyDown = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && wallColl != null)
        {
            float direction = GetComponent<Transform>().position.x - wallColl.GetComponent<Transform>().position.x / Mathf.Abs(GetComponent<Transform>().position.x - wallColl.GetComponent<Transform>().position.x);
            rb.velocity = new Vector3(0, jumpSpeed, 0);
            rb.AddForce(new Vector2(direction * maxSpeed, 0), ForceMode2D.Impulse);
            wallJumpcurrentCD = 0;

        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && (jumpStatus != JumpStatus.DOUBLEJUMPING))
        {
            if (jumpStatus == JumpStatus.JUMPING)
                jumpStatus = JumpStatus.DOUBLEJUMPING;
            else
                jumpStatus = JumpStatus.JUMPING;
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);
        }
    }

    void FixedUpdate()
    {
        if (wallJumpcurrentCD >= 0)
            wallJumpcurrentCD += Time.deltaTime;
        if(wallJumpcurrentCD == -1 || wallJumpcurrentCD > wallJumpCD)
        {
            wallJumpcurrentCD = -1; 
            float moveHorizontal = Input.GetAxis("Horizontal");

            if(Input.GetAxis("Horizontal") != 0)
                rb.velocity = new Vector3(moveHorizontal * maxSpeed, rb.velocity.y, 0);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            wallColl = collision.gameObject;
        }
        else if(collision.gameObject.name == "Ground" || collision.gameObject.name == "Platform")
        {
            jumpStatus = JumpStatus.GROUND;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            wallColl = null;
        }
    }
}