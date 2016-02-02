using UnityEngine;
using System.Collections;

public class RocketBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.name == "Player" || collide.name == "Player(Clone)") return;
       
        this.transform.Find("RocketAOE").GetComponent<CircleCollider2D>().enabled = true;
        Destroy(this.GetComponent<Rigidbody2D>());

        //Here do anim explo
        StartCoroutine(KillAfter(0.5f));
    }

    IEnumerator KillAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
