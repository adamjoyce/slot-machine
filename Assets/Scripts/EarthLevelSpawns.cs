using UnityEngine;
using System.Collections;

public class EarthLevelSpawns : MonoBehaviour {

    public GameObject chargerBossPrefab;
    public GameObject spitterBossPrefab;
    public GameObject flyerBossPrefab;

    // Use this for initialization
    void Start() {
        spawnBoss(PlayerPrefs.GetString("Enemy"));
    }

    //
    private void spawnBoss(string enemyType)
    {
        if (enemyType == "Charger") {
            Instantiate(chargerBossPrefab, new Vector3(2.0f, 0, 0), Quaternion.identity);
        } else if (enemyType == "Spitter") {
            Instantiate(spitterBossPrefab, new Vector3(2.0f, 0, 0), Quaternion.identity);
        } else {
            // Flyer.
            Instantiate(flyerBossPrefab, new Vector3(2.0f, 2.0f, 0), Quaternion.identity);
        }
    }
}
