using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {


    public int bulletDamage = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
