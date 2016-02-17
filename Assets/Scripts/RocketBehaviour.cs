using UnityEngine;
using System.Collections;

public class RocketBehaviour : MonoBehaviour {


    public int RocketDamage = 20;
    private float spawnTime;
    // Use this for initialization
    void Start()
    {

        spawnTime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime + 5 < Time.fixedTime)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.tag == "Enemy") return;
       
        this.transform.Find("RocketAOE").GetComponent<CircleCollider2D>().enabled = true;
        Destroy(this.GetComponent<Rigidbody2D>());

        //Here do anim explo
        StartCoroutine(KillAfter(0.425f));
    }

    IEnumerator KillAfter(float time)
    {
        this.transform.Find("RocketAOE").GetComponent<Animator>().Play("Explosion");
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
