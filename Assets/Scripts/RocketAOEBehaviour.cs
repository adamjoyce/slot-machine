using UnityEngine;
using System.Collections;

public class RocketAOEBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D collide)
    {
        Debug.Log("Explo -> " + collide.name);
        if (collide.tag == "Enemy")
        {
            collide.gameObject.GetComponent<Enemy>().inflictDamage(this.transform.parent.GetComponent<RocketBehaviour>().RocketDamage);
        }
        else if (collide.tag == "Player")
        {
            collide.gameObject.GetComponent<PlayerController>().inflictDamage(100, transform.position);
        }
    }

}
