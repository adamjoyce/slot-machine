using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int HP = 20;
    public float nextFireball;
    public float fireballCD = 0.6f;
    float fireballVelocity = 4.0f;
    private RAIN.Core.AI ai;

    // Use this for initialization
    void Start () {
        ai = transform.GetChild(0).GetComponent<RAIN.Core.AIRig>().AI;
        nextFireball = 0;


        ai.WorkingMemory.SetItem("forceTurn", false);
        int randDirection = Random.Range(0, 2);
        if (randDirection == 0)
            randDirection = -1;
        if (!ai.WorkingMemory.ItemExists("direction"))
            ai.WorkingMemory.SetItem<float>("direction", randDirection);

        if (randDirection == -1)
        {
            ai.Senses.Sensors[1].AngleOffset = new Vector3(ai.Senses.Sensors[1].AngleOffset.x, -ai.Senses.Sensors[1].AngleOffset.y, ai.Senses.Sensors[1].AngleOffset.z);
        }

        var anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
	
	// Update is called once per frame
	void Update () {


            if (this.name.Length >= 15 && this.name.Substring(0, 15) == "Enemy - Spitter")
        {
            if (nextFireball > 0)
                nextFireball -= Time.deltaTime;
            else
            {
                var player = ai.WorkingMemory.GetItem<GameObject>("playerFound");
                if (player != null && Mathf.Abs((player.transform.position - transform.position).magnitude) < 3.0f)
                {
                    Vector3 direction = ((player.transform.position + new Vector3(0, 0.3f, 0)) - transform.position);
                    direction.Normalize();
                    GameObject newBullet = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Fireball"), this.transform.position + new Vector3(direction.x * 0.4f, 0.2f, 0), new Quaternion());
                    newBullet.transform.parent = GameObject.Find("BulletHolder").transform;
                    newBullet.GetComponent<Rigidbody2D>().velocity = direction * fireballVelocity;
                    nextFireball = fireballCD;
                }
            }
        }
	}

    public int inflictDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            StartCoroutine(KillEnemy());
        return damage;
    }

    IEnumerator KillEnemy()
    {
        GetComponent<Animator>().Play("Death");
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Player" || coll.gameObject.name == "Player(Clone)")
        {
            coll.gameObject.GetComponent<PlayerController>().inflictDamage(20, this.transform.position);
        }

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player" || coll.gameObject.name == "Player(Clone)")
        {
            coll.gameObject.GetComponent<PlayerController>().inflictDamage(20, this.transform.position);
        }
        if (coll.gameObject.name == "Right Wall" || coll.gameObject.name == "Left Wall")
        {
            transform.GetChild(0).GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("forceTurn", true);
        }
    }

}
