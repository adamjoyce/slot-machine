using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    private float spawnTime;
    public int bulletDamage = 5;
	// Use this for initialization
	void Start () {

       spawnTime = Time.fixedTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (spawnTime + 5 < Time.fixedTime)
            Destroy(this.gameObject);
	}

    void OnTriggerEnter2D(Collider2D collide)
    {
        if(collide.name == "Enemy" || collide.name == "Enemy(Clone)")
        {
            collide.gameObject.GetComponent<Enemy>().inflictDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
