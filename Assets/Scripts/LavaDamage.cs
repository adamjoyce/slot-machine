using UnityEngine;
using System.Collections;

public class LavaDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  // Kill anything that touches the lava.
  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.tag == "Player") {
      coll.gameObject.GetComponent<PlayerController>().inflictDamage(50, gameObject.GetComponent<Transform>().position);
    }
  }
}
