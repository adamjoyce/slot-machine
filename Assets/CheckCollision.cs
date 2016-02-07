using UnityEngine;
using System.Collections;

public class CheckCollision : MonoBehaviour {

  bool colliding = true;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (!colliding) {
      this.GetComponent<Rigidbody>().isKinematic = true;
    }
    colliding = false;
  }

  // Check if something is colliding with this object.
  void OnCollisionStay(Collision collision) {
    colliding = true;
    Debug.Log("Colliding");
  }
}
