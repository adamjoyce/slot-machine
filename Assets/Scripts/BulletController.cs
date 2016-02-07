using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    private float spawnTime;
    public int bulletDamage = 5;
    public int fireballDamage = 15;
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
        if (this.name == "Bullet(Clone)" && (collide.name.Length >= 5 && collide.name.Substring(0,5) == "Enemy"))
        {
            collide.gameObject.GetComponent<Enemy>().inflictDamage(bulletDamage);
            Destroy(this.gameObject);
        }
        else if (this.name == "Fireball(Clone)" && (collide.name == "Player" || collide.name == "Player(Clone)"))
        {
            collide.gameObject.GetComponent<PlayerController>().inflictDamage(fireballDamage, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
