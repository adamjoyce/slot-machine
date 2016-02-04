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
        if(collide.name == "Enemy" || collide.name == "Enemy(Clone)")
        {
            collide.gameObject.GetComponent<Enemy>().inflictDamage(this.transform.parent.GetComponent<RocketBehaviour>().RocketDamage);
        }
    }

}
