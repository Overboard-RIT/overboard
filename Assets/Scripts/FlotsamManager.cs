using System;
using System.Collections;
using UnityEngine;

public class FlotsamManager : MonoBehaviour
{
    public GameObject[] flotsamPrefabs; // Array of flotsam prefabs
    public float spawnIntervalMin = 3f; // Time between spawns
    public float spawnIntervalMax = 7f;
    public float spawnRadius = 15f; // Radius around the player to spawn flotsam
    public float offRadiusChance = 0.1f; // Chance for flotsam to spawn outside the radius
    public float offRadiusMaxDistance = 30f; // Max distance for off-radius spawn
    public LayerMask flotsamLayer; // Layer to check for existing flotsam
    public float spawnY = -5f; // Initial spawn height (below water)
    public Transform playerTransform; // Reference to the player's transform

    [SerializeField]
    private Vector3 minGlobalBoundary; // Minimum (x, z) boundary for spawn area
    [SerializeField]
    private Vector3 maxGlobalBoundary; // Maximum (x, z) boundary for spawn area
    public Vector3 MinGlobalBoundary
    {
        get => minGlobalBoundary;
        set
        {
            minGlobalBoundary = new Vector3(value.x, 0, value.z);
            boundsManager.BoundsMin = minGlobalBoundary;
        }
    }
    public Vector3 MaxGlobalBoundary
    {
        get => maxGlobalBoundary;
        set
        {
            maxGlobalBoundary = new Vector3(value.x, 0, value.z);
            boundsManager.BoundsMax = maxGlobalBoundary;
        }
    }

    public GameManager gameManager; // Reference to GameManager script
    public BoundsManager boundsManager; // Reference to BoundsManager script

    private bool stopWorking = false;

    void OnValidate()
    {
        MinGlobalBoundary = minGlobalBoundary;
        MaxGlobalBoundary = maxGlobalBoundary;
    }
    private void Start()
    {
        boundsManager.BoundsMin = minGlobalBoundary;
        boundsManager.BoundsMax = maxGlobalBoundary;
        StartCoroutine(SpawnFlotsamRoutine());
    }

    public void Stop()
    {
        stopWorking = true;
    }

    private IEnumerator SpawnFlotsamRoutine()
    {
        while (!stopWorking)
        {
            while (!gameManager.gameStarted)
            {
                yield return new WaitForSeconds(0.1f); // Wait until the game starts
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax));
            SpawnFlotsam();
        }
    }

    private void SpawnFlotsam()
    {
        if (flotsamPrefabs.Length != 0)
        {

            while (true)
            {
                // Decide whether to spawn within the radius or outside
                Vector3 spawnPosition;
                if (UnityEngine.Random.value < offRadiusChance)
                {
                    spawnPosition = GetRandomPositionOutsideRadius();
                }
                else
                {
                    spawnPosition = GetRandomPositionAroundPlayer();
                }

                GameObject flotsamPrefab = flotsamPrefabs[UnityEngine.Random.Range(0, flotsamPrefabs.Length)];

                // Ensure there's no overlap with existing flotsam
                if (IsPositionWithinGlobalBoundary(spawnPosition) && !IsPositionOccupied(spawnPosition, flotsamPrefab))
                {
                    spawnPosition.y = -3f;
                    Instantiate(flotsamPrefab, spawnPosition, Quaternion.identity);
                    return;
                }
            }

        }

    }

    // Check if the spawn position is within the global boundaries
    private bool IsPositionWithinGlobalBoundary(Vector3 position)
    {
        // Ensure that the position is within the x and z boundaries (ignoring y-axis)
        if (position.x >= minGlobalBoundary.x && position.x <= maxGlobalBoundary.x &&
            position.z >= minGlobalBoundary.z && position.z <= maxGlobalBoundary.z)
        {
            return true;
        }
        return false;
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        // Generate a random position within the radius around the player
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, spawnY, randomCircle.y) + playerTransform.position;
        return spawnPosition;
    }

    private Vector3 GetRandomPositionOutsideRadius()
    {
        // Generate a random position outside the radius
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * offRadiusMaxDistance;
        Vector3 spawnPosition = new Vector3(randomCircle.x, spawnY, randomCircle.y) + playerTransform.position;
        return spawnPosition;
    }

    private bool IsPositionOccupied(Vector3 position, GameObject flotsamPrefab)
    {
        BoxCollider flotsamCollider = flotsamPrefab.GetComponent<BoxCollider>();

        // Calculate the half extents of the box based on the collider size and the prefab's scale
        Vector3 halfExtents = Vector3.Scale(flotsamCollider.size * 0.6f, flotsamPrefab.transform.localScale);

        // Project the position to the XZ plane
        Vector3 projectedPosition = new Vector3(position.x, 0.5f, position.z);

        // Check for collisions with existing flotsam (using a box overlap check)
        Collider[] colliders = Physics.OverlapBox(projectedPosition, halfExtents, Quaternion.identity, flotsamLayer);
        return colliders.Length > 0;
    }
}
