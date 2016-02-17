using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knife : MonoBehaviour {

    public int knifeDamage = 15;
    public List<GameObject> enemiesHit; 
	// Use this for initialization
	void Start () {
        enemiesHit = new List<GameObject>();
        this.transform.position = transform.parent.transform.position + new Vector3(0.2f, 0.27f, 0.0f);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collide)
    {
        if(collide.tag == "Enemy" && !enemiesHit.Contains(collide.gameObject))
        {
            Debug.Log("Inflict Damage");
            collide.gameObject.GetComponent<Enemy>().inflictDamage(knifeDamage);
            enemiesHit.Add(collide.gameObject);
        }
    }
}
