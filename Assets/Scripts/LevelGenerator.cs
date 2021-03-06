﻿using UnityEngine;
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

    public int enemyFlyerNumber;

    private float gridHeight;
    private float gridWidth;

    private float lavaHeight;

    private List<GameObject> platforms;

    private GameObject playerSpawnPlatform;

    void Start()
    {
        Camera camera = Camera.main;
        float frustrumHeight = 2.0f * camera.orthographicSize;
        float frustrumWidth = frustrumHeight * camera.aspect;
        gridHeight = frustrumHeight;
        gridWidth = frustrumWidth;

        lavaHeight = -3.0f;

        platforms = new List<GameObject>();

        GeneratePlatforms();
        StartCoroutine(CheckObjectsAreStill());
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Enemies"), true);
    }

    // Generates a number of platforms to be used in the level.
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

    // Determines when the sphere colliders of the platforms are at rest and the level generation can continue.
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

    // Removes platforms that are outside the boundaries and enabled the necessary platform components.
    private void CullPlatformElements()
    {
        List<GameObject> plats = new List<GameObject>();
        for (int i = 0; i < platforms.Count; i++) {
            float platX = platforms[i].GetComponent<Transform>().transform.position.x;
            float platY = platforms[i].GetComponent<Transform>().transform.position.y;
            float width = gridWidth * 0.5f;
            float height = gridHeight * 0.5f;

            // Destroy platforms outside of the bounds or in the lava.
            if (platX >= width || platX <= -width || platY >= maxPlatformHeight || platY <= -height || (PlayerPrefs.GetString("Level") == "Lava" && platY <= lavaHeight)) {
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

    // After platform placement enable the level boundaries.
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

        index = Random.Range(0, platforms.Count);
        float platX = platforms[index].transform.position.x;
        float platY = platforms[index].transform.position.y;

        playerSpawnPlatform = platforms[index];
        Instantiate(playerPrefab, new Vector3(platX, platY, 0), Quaternion.identity);
    }

    public float spawnThreshold = 0.2f;
    public float doubleSpawnThreshold = 0.8f;

    // Spawns enemies dependant on enemy type.
    private void spawnEnemies(string enemyType)
    {
        int enemies = 0;
        int platformLength = 1;

        // Charger.
        if (enemyType == "Charger") {
            for (int i = 0; i < platforms.Count; i++) {
                // Skip the player platform.
                if (platforms[i] == playerSpawnPlatform) {
                    continue;
                }

                float x = platforms[i].transform.position.x;
                float y = platforms[i].transform.position.y;

                // Do not spawn enemies on platforms that overlap the screen boundary.
                if (x - platformLength <= -gridWidth * 0.5f || x + platformLength >= gridWidth * 0.5f) {
                    continue;
                }

                // Determine the number of enemies to spawn for this platform.
                float rand = Random.value;
                if (rand >= spawnThreshold || enemies == 0) {
                    if (rand >= doubleSpawnThreshold) {
                        Instantiate(chargerPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        Instantiate(chargerPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        enemies += 2;
                    } else {
                        Instantiate(chargerPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        enemies++;
                    }
                }
            }
        }

        // Spitter.
        if (enemyType == "Spitter") {
            for (int i = 0; i < platforms.Count; i++) {
                // Skip the player platform.
                if (platforms[i] == playerSpawnPlatform) {
                    continue;
                }

                float x = platforms[i].transform.position.x;
                float y = platforms[i].transform.position.y;

                // Do not spawn enemies on platforms that overlap the screen boundary.
                if (x - platformLength <= -gridWidth * 0.5f || x + platformLength >= gridWidth * 0.5f) {
                    continue;
                }

                // Determine the number of enemies to spawn for this platform.
                float rand = Random.value;
                if (rand >= spawnThreshold || enemies == 0) {
                    if (rand >= doubleSpawnThreshold) {
                        Instantiate(spitterPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        Instantiate(spitterPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        enemies += 2;
                    } else {
                        Instantiate(spitterPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        enemies++;
                    }
                }
            }
        }

        // Flyer.
        if (enemyType == "Flyer") {
            for (int i = 0; i < platforms.Count; i++) {
                // Skip the player platform.
                if (platforms[i] == playerSpawnPlatform) {
                    continue;
                }

                // Determine the number of enemies to spawn for this platform.
                float offset = 0.5f;
                float rand = Random.value;
                if (rand >= spawnThreshold || enemies == 0) {
                    float x = Random.Range(-gridWidth * 0.5f + offset, gridWidth * 0.5f - offset);
                    float y = Random.Range(maxPlatformHeight, gridHeight * 0.5f - offset);
                        Instantiate(flyerPrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        enemies++;
                }
            }
        }
    }
}
