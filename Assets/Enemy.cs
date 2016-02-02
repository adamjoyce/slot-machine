using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int HP = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int inflictDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            Destroy(this.gameObject);
        return damage;
    }
}
