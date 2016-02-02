using UnityEngine;
using System.Collections;

public class SlotMachine : MonoBehaviour {

  private static readonly string[] levels = { "Fire", "Water", "Plat" };
  private static readonly string[] weapons = { "Knife", "Gun", "Rocket" };
  private static readonly string[] enemies = { "Golem", "Duck", "Bird" };

  private GameObject[] levelObjects;
  private GameObject[] weaponObjects;
  private GameObject[] enemyObjects;

  private const int numberOfSlots = 3;

  private bool slotsAnimation = false;

  private int slotVisibleIndex0, slotVisibleIndex1, slotVisibleIndex2;

  private float slotsTimer = 0.0f;
  private float previousAnimationTime = 0.0f;

  private const float slowTime = 7.0f;
  private const float interval = 0.05f;
  private float stopTime0, stopTime1, stopTime2;

  private bool animateSlot0 = true;
  private bool animateSlot1 = true;

  // Initilisation / assignment.
  private void Start() {
    levelObjects = new GameObject[levels.Length];
    weaponObjects = new GameObject[weapons.Length];
    enemyObjects = new GameObject[enemies.Length];

    // To randomly assign the visible item for each slot.
    int randSlot0 = Random.Range(0, levels.Length);
    int randSlot1 = Random.Range(0, weapons.Length);
    int randSlot2 = Random.Range(0, enemies.Length);

    // Level setup.
    for (int i = 0; i < levels.Length; i++) {
      levelObjects[i] = GameObject.Find(levels[i]);
      if (i == randSlot0) {
        levelObjects[i].GetComponent<SpriteRenderer>().enabled = true;
        slotVisibleIndex0 = i;
      } else {
        levelObjects[i].GetComponent<SpriteRenderer>().enabled = false;
      }
    }

    // Weapons setup.
    for (int i = 0; i < weapons.Length; i++) {
      weaponObjects[i] = GameObject.Find(weapons[i]);
      if (i == randSlot1) {
        weaponObjects[i].GetComponent<SpriteRenderer>().enabled = true;
        slotVisibleIndex1 = i;
      } else {
        weaponObjects[i].GetComponent<SpriteRenderer>().enabled = false;
      }
    }

    // Enemy setup.
    for (int i = 0; i < enemies.Length; i++) {
      enemyObjects[i] = GameObject.Find(enemies[i]);
      if (i == randSlot2) {
        enemyObjects[i].GetComponent<SpriteRenderer>().enabled = true;
        slotVisibleIndex2 = i;
      } else {
        enemyObjects[i].GetComponent<SpriteRenderer>().enabled = false;
      }
    }
  }

  // Update is called once per frame.
  private void Update() {
    // Detect when the lever is pulled.
    if (Input.GetMouseButtonDown(0)) {
      RaycastHit hit;
      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
        if (hit.collider.tag == "Lever") {
          // Lever animation...
          Debug.Log("Lever Animation");

          // Reset state variables.
          slotsTimer = 0.0f;
          previousAnimationTime = 0.0f;
          animateSlot0 = true;
          animateSlot1 = true;

          // Determine random incremental stopping times for the reels.
          float min = 10.0f;
          float max = 13.0f;
          float increment = 3.0f;
          stopTime0 = Random.Range(min, max);
          min += increment;
          max += increment;
          stopTime1 = Random.Range(min, max);
          min += increment;
          max += increment;
          stopTime2 = Random.Range(min, max);

          // Allows the slots animation to play.
          slotsAnimation = true;
        }
      }
    }

    // Simulate the slots animation.
    if (slotsAnimation) {
      slotsTimer += Time.deltaTime;

      if (slotsTimer >= stopTime2) {
        slotsAnimation = false;
        // Detect results and load level.
        string[] results = getSlotResults();
        Application.LoadLevel("_Scenes/" + "Level " + results[0]);
      } else if (slotsTimer >= stopTime1) {
        animateSlot0 = false;
        animateSlot1 = false;
      } else if (slotsTimer >= stopTime0) {
        animateSlot0 = false;
      }

      // Simulates the time between animations.
      if (slotsTimer - previousAnimationTime >= interval) {
        // Animate the first slot.
        if (animateSlot0) {
          AnimateSlots(levelObjects, ref slotVisibleIndex0);
        }
        // Animate the second slot.
        if (animateSlot1) {
          AnimateSlots(weaponObjects, ref slotVisibleIndex1);
        }
        // Animate the third slot.
        AnimateSlots(enemyObjects, ref slotVisibleIndex2);

        previousAnimationTime = slotsTimer;
      }
    }
  }

  // Animation logic.
  private void AnimateSlots(GameObject[] slot, ref int slotVisibleIndex) {
    int nextIndex = 0;
    if (slot[slotVisibleIndex].GetComponent<SpriteRenderer>().enabled) {
      // Check for index wrapping.
      if (slotVisibleIndex == slot.Length - 1)
        nextIndex = 0;
      else
        nextIndex = slotVisibleIndex + 1;
      slot[slotVisibleIndex].GetComponent<SpriteRenderer>().enabled = false;
      slot[nextIndex].GetComponent<SpriteRenderer>().enabled = true;
    }
    slotVisibleIndex = nextIndex;
  }

  // Get slot results.
  private string[] getSlotResults() {
    return new string[] { levels[slotVisibleIndex0], weapons[slotVisibleIndex1], enemies[slotVisibleIndex2] };
  }
}
