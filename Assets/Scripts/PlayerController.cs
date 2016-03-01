using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public float jumpSpeed;
    private bool facingRight;
    public string weaponName;
    public float bulletCD = 1.0f;
    public float rocketLauncherCD = 2.0f;
    private float nextBullet;
    public float bulletVelocity = 5.0f;

    public int HP = 50;

    private Rigidbody2D rb;
    private JumpStatus jumpStatus;
    private enum JumpStatus
    {
        GROUND,
        JUMPING,
        DOUBLEJUMPING
    };
    private GameObject wallColl;
    public float KBDuration;
    private float currentKB;
    private bool usingKnife;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpStatus = JumpStatus.GROUND;
        currentKB = -1;
        facingRight = true;
        nextBullet = 0;
        usingKnife = false;
        weaponName = PlayerPrefs.GetString("Weapon");



        GameObject weapon = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Weapon").transform.Find(weaponName).gameObject, this.transform.position, new Quaternion());
        weapon.transform.parent = this.transform;
    }

    void Update()
    {
        if (nextBullet > 0)
            nextBullet -= Time.deltaTime;

        if (this.HP > 0)
        {
            if (this.jumpStatus != JumpStatus.GROUND)
            {
                GetComponent<Animator>().Play("Jump");
            }
            else if (Input.GetAxis("Horizontal") != 0)
            {
                GetComponent<Animator>().Play("Run");
            }
            else
            {
                GetComponent<Animator>().Play("Idle");
            }
        }
        else
            return;

        bool keyDown = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && wallColl != null)
        {
            float direction = GetComponent<Transform>().position.x - wallColl.GetComponent<Transform>().position.x / Mathf.Abs(GetComponent<Transform>().position.x - wallColl.GetComponent<Transform>().position.x);
            rb.velocity = new Vector3(0, jumpSpeed, 0);
            rb.AddForce(new Vector2(direction * maxSpeed, 0), ForceMode2D.Impulse);
            currentKB = 0;

        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && (jumpStatus != JumpStatus.DOUBLEJUMPING))
        {
            if (jumpStatus == JumpStatus.JUMPING)
                jumpStatus = JumpStatus.DOUBLEJUMPING;
            else
                jumpStatus = JumpStatus.JUMPING;
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (weaponName == "Knife")
            {
                Transform knife = transform.Find("Knife(Clone)");
                if (knife != null)
                {
                    knife.GetComponent<BoxCollider2D>().enabled = true;
                    knife.GetComponent<Knife>().enemiesHit = new System.Collections.Generic.List<GameObject>();
                    knife.localPosition = new Vector3(0.2f, 0.27f, 0.0f);
                    knife.eulerAngles = new Vector3(0, 0, 0);
                    usingKnife = true;
                }
                StartCoroutine(DisableDagger(0.5f));
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Vector2 currentposition = new Vector2(transform.position.x, transform.position.y);
            int direction = 1;
            if (!facingRight) direction *= -1;
            if (weaponName == "Gun" && nextBullet <= 0)
            {
                GameObject newBullet = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), this.transform.position + new Vector3(direction *0.4f,0.5f,0), new Quaternion());
                newBullet.transform.parent = GameObject.Find("BulletHolder").transform;
                Vector2 dir = (mouseposition - currentposition);
                if (dir.x < 0) bulletVelocity *= -1;
                newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletVelocity, 0);
                nextBullet = bulletCD;
            }
            else if (weaponName == "RocketLauncher" && nextBullet <= 0)
            {
                GameObject newBullet = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Rocket"), this.transform.position + new Vector3(0,0.5f,0), new Quaternion());
                newBullet.transform.parent = GameObject.Find("BulletHolder").transform;
                float bulletVel = 12.0f;
                Vector2 dir = (mouseposition - currentposition);
                dir.Normalize();
                newBullet.GetComponent<Rigidbody2D>().velocity = dir * bulletVel;
                nextBullet = rocketLauncherCD;
            }



            if((facingRight && mouseposition.x < currentposition.x) || (!facingRight && mouseposition.x > currentposition.x))
            {
                facingRight = !facingRight;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }

    }

    IEnumerator DisableDagger(float time)
    {
        yield return new WaitForSeconds(time);
        this.transform.Find("Knife(Clone)").GetComponent<BoxCollider2D>().enabled = false;
        usingKnife = false;
    }

    void FixedUpdate()
    {
        if (currentKB >= 0)
            currentKB += Time.deltaTime;
        if(currentKB == -1 || currentKB > KBDuration)
        {
            currentKB = -1; 
            float moveHorizontal = Input.GetAxis("Horizontal");

            if (Input.GetAxis("Horizontal") != 0)
            {
                rb.velocity = new Vector3(moveHorizontal * maxSpeed, rb.velocity.y, 0);
                if (Input.GetAxis("Horizontal") < 0)
                    facingRight = false;
                else
                    facingRight = true;



                if (wallColl == null)
                {
                    if ((facingRight && transform.localScale.x < 0) ||
                        !facingRight && transform.localScale.x > 0)
                    {
                        Vector3 theScale = transform.localScale;
                        theScale.x *= -1;
                        transform.localScale = theScale;
                    }
                }
                Transform knife = transform.Find("Knife(Clone)");
                if (!usingKnife && knife != null)
                {
                    knife.localPosition = new Vector3(0.3f, 0.8f, 0);
                    knife.eulerAngles = new Vector3(0, 0, 90);
                }
                else if (knife != null)
                {
                    knife.localPosition = new Vector3(0.5f, 0.65f, 0.0f);
                    knife.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            else if(!usingKnife && jumpStatus == JumpStatus.GROUND)
            {
                Transform knife = transform.Find("Knife(Clone)");
                if (knife != null)
                {
                    knife.localPosition = new Vector3(0.2f, 0.27f, 0.0f);
                    knife.eulerAngles = new Vector3(0, 0, 0);
                }
            }
        }

    }

    public void inflictDamage(int dmg, Vector3 incPos)
    {
        HP -= dmg;
        Vector3 direction = this.transform.position - incPos;
        currentKB = 0;
        rb.velocity = new Vector3(0, 0, 0);
        direction.Normalize();
        rb.AddForce(direction * maxSpeed, ForceMode2D.Impulse);
        if (HP <= 0)
            StartCoroutine(KillPlayer());
    }


    IEnumerator KillPlayer()
    {
        GetComponent<Animator>().Play("Death");
        yield return new WaitForSeconds(3.2f);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            wallColl = collision.gameObject;
        }
        else if(collision.gameObject.name == "Ground" || collision.gameObject.name == "Platform" || collision.gameObject.tag == "Platform")
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