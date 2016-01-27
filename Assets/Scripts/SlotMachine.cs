using UnityEngine;
using System.Collections;

public class SlotMachine : MonoBehaviour {

  public float animationSpeed = 1.0f;

  private static readonly string[] levels = { "Fire", "Water", "Plat" };
  private static readonly string[] weapons = { "Knife", "Gun", "Rocket" };
  private static readonly string[] enemies = { "Golem", "Duck", "Bird" };

  private static readonly string[][] slots = { levels, weapons, enemies };

  private GameObject[] levelObjects;
  private GameObject[] weaponObjects;
  private GameObject[] enemyObjects;

  private const int numberOfSlots = 3;

  private bool slotsAnimation = false;

  private int slotVisibleIndex = 0;

  private int frameCount = 0;
  public int frameInterval = 10;

  private float slotsTimer = 0.0f;
  private const float slowTime = 7.0f;
  private const float slowTimeInterval = 3.0f;
  private const float stopTIme = 18.0f;

  // Initilisation / assignment.
  private void Start() {
    levelObjects = new GameObject[levels.Length];
    weaponObjects = new GameObject[weapons.Length];
    enemyObjects = new GameObject[enemies.Length];

    //int randSlot1 = Random.Range(0, levels.Length);
    //int randSlot2 = Random.Range(0, weapons.Length);
    //int randSlot3 = Random.Range(0, enemies.Length);

    /*for (int i = 0; i < levels.Length; i++) {
      levelObjects[i] = GameObject.Find(levels[i]);
      Debug.Log(levels[i]);
      if (i == 0)
        levelObjects[i].GetComponent<SpriteRenderer>().enabled = true;
      else
        levelObjects[i].GetComponent<SpriteRenderer>().enabled = false;
    }*/

    for (int i = 0; i < weapons.Length; i++) {
      weaponObjects[i] = GameObject.Find(weapons[i]);
      if (i == 0)
        weaponObjects[i].GetComponent<SpriteRenderer>().enabled = true;
      else
        weaponObjects[i].GetComponent<SpriteRenderer>().enabled = false;
    }

    /*for (int i = 0; i < enemies.Length; i++) {
      enemyObjects[i] = GameObject.Find(enemies[i]);
      if (i == 0)
        enemyObjects[i].GetComponent<SpriteRenderer>().enabled = true;
      else
        enemyObjects[i].GetComponent<SpriteRenderer>().enabled = false;
    }*/
  }

  // Update is called once per frame.
  private void Update() {
    if (Input.GetMouseButtonDown(0)) {
      RaycastHit hit;
      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
        if (hit.collider.tag == "Lever") {
          // Lever animation...
          Debug.Log("Lever Animation");

          // Spin Slots.
          string[] results = SpinSlots();

          // Slots animation...
          slotsAnimation = true;
        }
      }
    }

    if (slotsAnimation) {
      slotsTimer += Time.deltaTime;

      if (slotsTimer >= slowTime && slotsTimer < slowTime + slowTimeInterval) {
        frameInterval = 15;
      } else if (slotsTimer >= slowTime + slowTimeInterval && slotsTimer < slowTime + slowTimeInterval * 2) {
        frameInterval = 20;
      } else if (slotsTimer >= slowTime + slowTimeInterval * 2) {
        frameInterval = 25;
      }

      if (slotsTimer >= stopTIme) {
        slotsTimer = 0.0f;
        slotsAnimation = false;
        ShowResults();
      }

      if (frameCount % frameInterval == 0) {
        AnimateSlots();
      }

      frameCount++;
    }
  }

  // 
  private string[] SpinSlots() {
    string[] slotsResults = new string[slots.Length];

    int rand;
    for (int i = 0; i < slots.Length; i++) {
      rand = Random.Range(0, slots[i].Length - 1);
      slotsResults[i] = slots[i][rand];
    }

    //for (int i = 0; i < slotsResults.Length; i++) {
    //Debug.Log(slotsResults[i] + " ");
    //}
    //Debug.Log("\n");

    return slotsResults;
  }

  // Show results of the spin.
  private void ShowResults() {

  }

  // Animation logic.
  private void AnimateSlots() {
    int nextIndex = slotVisibleIndex + 1;
    if (weaponObjects[slotVisibleIndex].GetComponent<SpriteRenderer>().enabled) {
      if (slotVisibleIndex == weaponObjects.Length - 1) {
        nextIndex = 0;
      }
      weaponObjects[slotVisibleIndex].GetComponent<SpriteRenderer>().enabled = false;
      weaponObjects[nextIndex].GetComponent<SpriteRenderer>().enabled = true;
    }

    if (slotVisibleIndex == weaponObjects.Length - 1)
      slotVisibleIndex = 0;
    else
      slotVisibleIndex++;
  }

  IEnumerator Wait() {
    yield return 0;
  }
}
