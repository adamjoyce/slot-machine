using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

  public GameObject platformPrefab;
  public GameObject playerPrefab;
  public GameObject chargerPrefab;
  public GameObject spitterPrefab;
  public GameObject flyerPrefab;

  // Note some may be culled.
  public int estimatePlatformNumber;
  public float maxPlatformHeight;

  public int enemyNumber;

  private float gridHeight;
  private float gridWidth;

  private List<GameObject> platforms;

  private GameObject playerSpawnPlatform;  

  void Start() {
    Camera camera = Camera.main;
    float frustrumHeight = 2.0f * camera.orthographicSize;
    float frustrumWidth = frustrumHeight * camera.aspect;
    gridHeight = frustrumHeight;
    gridWidth = frustrumWidth;

    platforms = new List<GameObject>();

    GeneratePlatforms();
    StartCoroutine(CheckObjectsAreStill());
  }

  private void GeneratePlatforms() {
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

  IEnumerator CheckObjectsAreStill() {
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
    spawnPlayer();
    spawnEnemies("Flyer");
  }

  private void CullPlatformElements() {
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

  // Spawn the player on a platform.
  private void spawnPlayer() {
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
  private void spawnEnemies(string enemyType) {
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

    // Flyer.
    if (enemyType == "Flyer") {
      for (int i = 0; i < enemyNumber; i++) {
        float x = Random.Range(-gridWidth * 0.5f, gridWidth * 0.5f);
        float y = Random.Range(maxPlatformHeight, gridHeight * 0.5f);
        Instantiate(flyerPrefab, new Vector3(x, y, 0), Quaternion.identity);
      }
    }
  }

    //StartCoroutine("PhysicsStep");

    //for (int i = 0; i < largePlatforms.Count; i++) {
    //largePlatforms[i].GetComponent<Rigidbody>().isKinematic = true;
    //}
  }

  //IEnumerator PhysicsStep() {
    //yield return new WaitForFixedUpdate();
  //}

  // Check that one platform's area does not intersect another platform's area.
  /*private bool PlatformsIntersect(GameObject platform) {
    BoxCollider collider1 = platform.transform.GetChild(0).GetComponent<BoxCollider>();

    // Check intersection with large platforms.
    for (int i = 0; i < largePlatforms.Count; i++) {
      BoxCollider collider2 = largePlatforms[i].transform.GetChild(0).GetComponent<BoxCollider>();
      if (collider1.bounds.Intersects(collider2.bounds)) {
        return true;
      }
    }*/

    /*public int noLargePlatforms;
    public int noMediumPlatforms;
    public int noSmallPlatforms;

    public GameObject largePlatformPrefab;
    public Transform mediumPlatformPrefab;
    public Transform smallPlatformPrefab;

    private SlotMachine slotMachine;
    private string[] slotResults;

    private float gridHeight;
    private float gridWidth;

    private List<GameObject> largePlatforms;

    // Use this for initialization
    void Start () {
      slotMachine = this.GetComponent<SlotMachine>();

      float frustrumHeight = 2.0f * -Camera.main.transform.position.z * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
      float frustrumWidth = frustrumHeight * Camera.main.aspect;
      gridHeight = frustrumHeight;
      gridWidth = frustrumWidth;

      largePlatforms = new List<GameObject>();
    }

    // Update is called once per frame
    void Update () {
      if (Input.GetButtonDown("Fire1")) {
        // Spin the reels and record the results.
        //slotMachine.SpinSlots();
        //slotResults = slotMachine.GetSlotOutput();

        // Load the correct level.
        LoadFireLevel();

        // For debugging.
        for (int i = 0; i < slotResults.Length; i++) {
          Debug.Log(slotResults[i] + " ");
        }
        Debug.Log("\n");
      }
    }

    // Load fire level.
    private void LoadFireLevel() {
      GeneratePlatforms(largePlatformPrefab);
    }

    // Load water level.
    private void LoadWaterLevel() {

    }

    // Load platform level.
    private void LoadPlatformLevel() {

    }

    // Generate random platform positions.
    private void GeneratePlatforms(GameObject platformPrefab) {
      float width = gridWidth / 2;
      float height = gridHeight / 2;
      float platX = 0;
      float platY = 0;

      // Large platforms.
      for (int i = 0; i < noLargePlatforms; i++) {
        width -= platformPrefab.transform.localScale.x;
        height -= platformPrefab.transform.localScale.y;

        platX = Random.Range(-width, width);
        platY = Random.Range(-height, height);
        GameObject platform = Instantiate(largePlatformPrefab, new Vector3(platX, platY, 0), Quaternion.identity) as GameObject;

        bool intersects = true;
        while (intersects) {
          if (largePlatforms.Count == 0) {
            intersects = false;
          } else {
            if (PlatformsIntersect(platform)) {
              // Delete platform and recreate at different coords.
              Destroy(platform);
              platX = Random.Range(-width, width);
              platY = Random.Range(-height, height);
              platform = Instantiate(largePlatformPrefab, new Vector3(platX, platY, 0), Quaternion.identity) as GameObject;
            } else {
              intersects = false;
            }
          }
        }

        largePlatforms.Add(platform);
      }
    }

    // Check that one platform's area does not intersect another platform's area.
    private bool PlatformsIntersect(GameObject platform) {
      BoxCollider collider1 = platform.transform.GetChild(0).GetComponent<BoxCollider>();

      // Check intersection with large platforms.
      for (int i = 0; i < largePlatforms.Count; i++) {
        BoxCollider collider2 = largePlatforms[i].transform.GetChild(0).GetComponent<BoxCollider>();
        if (collider1.bounds.Intersects(collider2.bounds)) {
          return true;
        }
      }

      return false;
    }*/
  
