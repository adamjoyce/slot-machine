using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

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
      slotMachine.SpinSlots();
      slotResults = slotMachine.GetSlotOutput();

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
}
