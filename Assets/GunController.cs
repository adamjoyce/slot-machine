using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var holder = new GameObject();
        holder.name = "BulletHolder";
        if (this.name == "Gun(Clone)")
            this.transform.position = transform.parent.transform.position + new Vector3(0.25f, -0.16f, 0.0f);
        if (this.name == "RocketLauncher(Clone)")
        {
            this.transform.position = transform.parent.transform.position + new Vector3(0.1f, -0.07f, 0.0f);
            this.transform.eulerAngles = new Vector3(0,0,12.0f);
        }
    }

    // Update is called once per frame
    void Update () {
	}
}
