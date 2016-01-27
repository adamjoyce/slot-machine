using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float speed;
  private Rigidbody rb;

  // Called on the frame that the script is enabled, just before update functions.
  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  // Called before performing physics calculations - place physics code here.
  void FixedUpdate() {
    float moveHorizontal = Input.GetAxis("Horizontal");

    Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
    rb.AddForce(movement * speed);
  }
}
