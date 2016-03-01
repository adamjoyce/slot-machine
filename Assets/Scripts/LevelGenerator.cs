using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

  public GameObject platformPrefab;
  public GameObject playerPrefab;
  public GameObject chargerPrefab;
  public GameObject spitterPrefab;
  public GameObject flyerPrefab;

  public GameObject floor;
  public GameObject leftWall;
  public GameObject rightWall;
  public GameObject ceiling;

  // Note some may be culled.
  public int estimatePlatformNumber;
  public float maxPlatformHeight;

  public int enemyNumber;

  private float gridHeight;
  private float gridWidth;

  private List<GameObject> platforms;

  private GameObject playerSpawnPlatform;

  void Start()
  {
    Camera camera = Camera.main;
    float frustrumHeight = 2.0f * camera.orthographicSize;
    float frustrumWidth = frustrumHeight * camera.aspect;
    gridHeight = frustrumHeight;
    gridWidth = frustrumWidth;

    platforms = new List<GameObject>();

    GeneratePlatforms();
    StartCoroutine(CheckObjectsAreStill());
  }

  private void GeneratePlatforms()
  {
    float width = gridWidth * 0.5f;
    float height = gridHeight * 0.5f;
    float platX = 0.0f;
    float platY = 0.0f;

    float radius = platformPrefab.GetComponent<CircleCollider2D>().radius;
    width -= radius;
    height -= radius;

    // Instantiate platform colliders with random positions.
    for (int i = 0; i < estimatePlatformNumber; i++) {
      platX = Random.Range(-width, width);
      platY = Random.Range(-height, height);
      GameObject platformCollider = Instantiate(platformPrefab, new Vector3(platX, platY, 0), Quaternion.identity) as GameObject;
      platforms.Add(platformCollider);
    }
  }

  IEnumerator CheckObjectsAreStill()
  {
    Debug.Log("Checking");
    bool allAsleep = false;
    while (!allAsleep) {
      allAsleep = true;
      Debug.Log("NOT SLEEPING");
      for (int i = 0; i < platforms.Count; i++) {
        if (!platforms[i].GetComponent<Rigidbody2D>().IsSleeping()) {
          allAsleep = false;
          yield return null;
          break;
        }
      }
    }
    Debug.Log("Asleep");

    CullPlatformElements();
    enableBoundaries();
    spawnPlayer();
        spawnEnemies(PlayerPrefs.GetString("Enemy"));
  }

  private void CullPlatformElements()
  {
    List<GameObject> plats = new List<GameObject>();
    for (int i = 0; i < platforms.Count; i++) {
      float platX = platforms[i].GetComponent<Transform>().transform.position.x;
      float platY = platforms[i].GetComponent<Transform>().transform.position.y;
      float width = gridWidth * 0.5f;
      float height = gridHeight * 0.5f;

      // Destroy platforms outside of the bounds.
      if (platX >= width || platX <= -width || platY >= maxPlatformHeight || platY <= -height) {
        Destroy(platforms[i]);
      } else {
        Destroy(platforms[i].GetComponent<Rigidbody2D>());
        platforms[i].GetComponent<CircleCollider2D>().enabled = false;
        platforms[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
        plats.Add(platforms[i]);
      }
    }
    Debug.Log("Fixed");
    platforms = plats;
  }

  //
  private void enableBoundaries()
  {
    floor.SetActive(true);
    leftWall.SetActive(true);
    rightWall.SetActive(true);
    ceiling.SetActive(true);
  }

  // Spawn the player on a platform.
  private void spawnPlayer()
  {
    int index = 0;
    //do {
    index = Random.Range(0, platforms.Count);
    float platX = platforms[index].transform.position.x;
    float platY = platforms[index].transform.position.y;
    //} while ();
    playerSpawnPlatform = platforms[index];
    Instantiate(playerPrefab, new Vector3(platX, platY, 0), Quaternion.identity);
  }

  //
  private void spawnEnemies(string enemyType)
  {
    // Charger.
    if (enemyType == "Charger") {
      int index = 0;
      for (int i = 0; i < enemyNumber; i++) {
        if (index == platforms.Count) {
          index = 0;
        }

        // Do not spawn enemies on the player's starting platform.
        if (platforms[index] == playerSpawnPlatform) {
          // Keep the index value for the enemy the same.
          i--;
          index++;
          continue;
        }

        float x = platforms[index].transform.position.x;
        float y = platforms[index].transform.position.y;
        Instantiate(chargerPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
        index++;
      }
    }

    // Spitter.
    if (enemyType == "Spitter") {
      int index = 0;
      for (int i = 0; i < enemyNumber; i++) {
        if (index == platforms.Count) {
          index = 0;
        }

        // Do not spawn enemies on the player's starting platform.
        if (platforms[index] == playerSpawnPlatform) {
          // Keep the index value for the enemy the same.
          i--;
          index++;
          continue;
        }

        float x = platforms[index].transform.position.x;
        float y = platforms[index].transform.position.y;
        Instantiate(spitterPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
        index++;
      }
    }

    // Flyer.
    if (enemyType == "Flyer") {
      for (int i = 0; i < enemyNumber; i++) {
        float x = Random.Range(-gridWidth * 0.5f, gridWidth * 0.5f);
        float y = Random.Range(maxPlatformHeight, gridHeight * 0.5f);
        Instantiate(flyerPrefab, new Vector3(x, y, 0), Quaternion.identity);
      }
    }
  }
}
