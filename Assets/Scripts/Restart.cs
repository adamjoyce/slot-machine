using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

    private bool enemiesSpawned = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	      if (Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("_Scenes/Slot Machine");
        }
        StartCoroutine(CheckFinishLevel());
	}
    IEnumerator CheckFinishLevel()
    {
        if (enemiesSpawned && (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 || GameObject.FindGameObjectsWithTag("Player").Length == 0))
        {
            yield return new WaitForSeconds(1.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("_Scenes/Slot Machine");
        }
        else if(!enemiesSpawned && GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            enemiesSpawned = true;
        }
    }

}
