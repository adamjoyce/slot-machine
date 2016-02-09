using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

  public GameObject platformPrefab;

  // Note some may be culled.
  public int estimatePlatformNumber;
  public float maxPlatformHeight;

  private float gridHeight;
  private float gridWidth;

  private List<GameObject> platformColliders;

  void Start() {
    Camera camera = Camera.main;
    float frustrumHeight = 2.0f * camera.orthographicSize;
    float frustrumWidth = frustrumHeight * camera.aspect;
    gridHeight = frustrumHeight;
    gridWidth = frustrumWidth;

    platformColliders = new List<GameObject>();

    GeneratePlatforms();
    StartCoroutine(CheckObjectsAreStill());
  }

  private void GeneratePlatforms() {
    float width = gridWidth * 0.5f;
    float height = gridHeight * 0.5f;
    float platX = 0;
    float platY = 0;

    float radius = platformPrefab.GetComponent<CircleCollider2D>().radius;
    width -= radius;
    height -= radius;

    // Instantiate platform colliders with random positions.
    for (int i = 0; i < estimatePlatformNumber; i++) {
      platX = Random.Range(-width, width);
      platY = Random.Range(-height, height);
      GameObject platformCollider = Instantiate(platformPrefab, new Vector3(platX, platY, 0), Quaternion.identity) as GameObject;
      //Debug.Log(i + ": x" + platX + " y" + platY);

      platformColliders.Add(platformCollider);
    }
  }

  IEnumerator CheckObjectsAreStill() {
    Debug.Log("Checking");
    bool allAsleep = false;
    while (!allAsleep) {
      allAsleep = true;
      Debug.Log("NOT SLEEPING");
      for (int i = 0; i < platformColliders.Count; i++) {
        if (!platformColliders[i].GetComponent<Rigidbody2D>().IsSleeping()) {
          allAsleep = false;
          yield return null;
          break;
        }
      }
    }
    Debug.Log("Asleep");
    CullPlatformElements();
  }

  private void CullPlatformElements() {
    for (int i = 0; i < platformColliders.Count; i++) {
      //if (platformColliders[i].GetComponent<Transform>().transform.position.y > maxPlatformHeight) {
        //Destroy(platformColliders[i]);
      //} else {
        //platformColliders[i].GetComponent<Rigidbody2D>().isKinematic = true;
        Destroy(platformColliders[i].GetComponent<Rigidbody2D>());
        platformColliders[i].GetComponent<CircleCollider2D>().enabled = false;
        platformColliders[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
      //}
    }
    Debug.Log("Fixed");
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
  
