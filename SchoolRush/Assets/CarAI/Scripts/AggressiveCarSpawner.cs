using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveCarSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Player Transform reference")]
    public Transform player;

    [Tooltip("Car prefabs to spawn")]
    public GameObject[] carPrefabs;

    [Tooltip("Spawn interval (seconds)")]
    public float spawnInterval = 10f;

    [Tooltip("Spawn distance from player")]
    public float spawnDistance = 15f;

    [Tooltip("Maximum concurrent spawned cars")]
    public int maxConcurrentCars = 5;

    [Tooltip("Cars will be removed if they get farther than this distance from player")]
    public float despawnDistance = 100f;

    [Header("Spawn Area Settings")]
    [Tooltip("Spawn angle range (degrees, 0 is directly in front of player)")]
    public float spawnAngleRange = 120f;

    [Tooltip("Terrain reference for height sampling")]
    public Terrain terrain;

    [Tooltip("Spawn height offset")]
    public float spawnHeightOffset = 10f;

    private List<GameObject> spawnedCars = new List<GameObject>();
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (player == null)
        {
            // Automatically find player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Automatically find terrain if not assigned
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
            if (terrain == null)
            {
                terrain = FindObjectOfType<Terrain>();
            }
        }

        if (player != null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
        else
        {
            Debug.LogError("Player not found! Please assign player transform.");
        }

        if (terrain == null)
        {
            Debug.LogError("Terrain not found! Please assign terrain reference.");
        }
    }

    void Update()
    {
        // Remove distant cars
        CleanupDistantCars();

        // Debug: Log current spawned cars status
        if (spawnedCars.Count > 0)
        {
            for (int i = 0; i < spawnedCars.Count; i++)
            {
                if (spawnedCars[i] != null)
                {
                    float distance = Vector3.Distance(spawnedCars[i].transform.position, player.position);
                }
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnedCars.Count < maxConcurrentCars && player != null)
            {
                SpawnAggressiveCar();
            }
        }
    }

    void SpawnAggressiveCar()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();

        if (spawnPosition != Vector3.zero)
        {
            // Select random car prefab
            GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            // Spawn car as child of this spawner object
            GameObject spawnedCar = Instantiate(carPrefab, spawnPosition, Quaternion.identity, this.transform);
            // Give it a unique name for easier identification
            spawnedCar.name = $"AggressiveCar_{spawnedCars.Count + 1}_{carPrefab.name}";

            // Verify spawn
            if (spawnedCar != null)
            {
                Renderer[] renderers = spawnedCar.GetComponentsInChildren<Renderer>();
            }
            else
            {
                Debug.LogError("Failed to spawn car!");
                return;
            }

            // Rotate toward player
            Vector3 directionToPlayer = (player.position - spawnPosition).normalized;
            spawnedCar.transform.rotation = Quaternion.LookRotation(directionToPlayer);

            // Add AggressiveCarAI component
            AggressiveCarAI aggressiveAI = spawnedCar.GetComponent<AggressiveCarAI>();
            if (aggressiveAI == null)
            {
                aggressiveAI = spawnedCar.AddComponent<AggressiveCarAI>();
            }

            aggressiveAI.SetTarget(player);
            spawnedCars.Add(spawnedCar);
        }
        else
        {
            Debug.LogError("Could not get valid spawn position!");
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain reference is null!");
            return Vector3.zero;
        }

        for (int attempts = 0; attempts < 10; attempts++)
        {
            // Calculate random position around player
            float randomAngle = Random.Range(-spawnAngleRange / 2f, spawnAngleRange / 2f);
            Vector3 direction = Quaternion.Euler(0, randomAngle, 0) * player.forward;
            Vector3 spawnPos = player.position + direction * spawnDistance;

            // Use terrain height sampling instead of raycast
            float terrainHeight = terrain.SampleHeight(spawnPos);
            Vector3 finalPos = new Vector3(spawnPos.x, terrainHeight + spawnHeightOffset, spawnPos.z);

            // Check if position is within terrain bounds
            Vector3 terrainPos = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;

            if (spawnPos.x >= terrainPos.x && spawnPos.x <= terrainPos.x + terrainData.size.x &&
                spawnPos.z >= terrainPos.z && spawnPos.z <= terrainPos.z + terrainData.size.z)
            {
                // Spawn at higher position to avoid overlap - no need to check sphere overlap
                return finalPos;
            }
        }

        Debug.LogWarning("Could not find valid spawn position within terrain bounds");
        return Vector3.zero;
    }

    void CleanupDistantCars()
    {
        for (int i = spawnedCars.Count - 1; i >= 0; i--)
        {
            if (spawnedCars[i] == null)
            {
                spawnedCars.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(spawnedCars[i].transform.position, player.position);
            if (distance > despawnDistance)
            {
                Destroy(spawnedCars[i]);
                spawnedCars.RemoveAt(i);
            }
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    public void ResumeSpawning()
    {
        if (spawnCoroutine == null && player != null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    void OnDestroy()
    {
        StopSpawning();
    }
}
