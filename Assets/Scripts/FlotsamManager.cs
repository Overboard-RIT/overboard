using System;
using System.Collections;
using UnityEngine;

public class FlotsamManager : MonoBehaviour
{
    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Casual; // Difficulty level
    [Range(0.0f, 1.0f)]
    public float scaleFactor = 1f; // Scale factor for flotsam size
    [Range(1.0f, 2.0f)]
    public float distanceFactor = 1f; // Distance factor for flotsam spawn distance
    [Header("Flotsam Settings")]
    public GameObject[] flotsamPrefabs; // Array of flotsam prefabs
    public float spawnIntervalMin = 3f; // Time between spawns
    public float spawnIntervalMax = 7f;
    public float spawnRadius = 15f; // Radius around the player to spawn flotsam
    public float offRadiusChance = 0.1f; // Chance for flotsam to spawn outside the radius
    public float offRadiusMaxDistance = 30f; // Max distance for off-radius spawn
    public LayerMask flotsamLayer; // Layer to check for existing flotsam
    public float spawnY = -5f; // Initial spawn height (below water)
    public Transform playerTransform; // Reference to the player's transform

    private Vector3 minGlobalBoundary; // Minimum (x, z) boundary for spawn area
    private Vector3 maxGlobalBoundary; // Maximum (x, z) boundary for spawn area
    public Vector3 MinGlobalBoundary
    {
        get { return minGlobalBoundary; }
        set { minGlobalBoundary = value; }
    }
    public Vector3 MaxGlobalBoundary
    {
        get { return maxGlobalBoundary; }
        set { maxGlobalBoundary = value; }
    }

    public enum Difficulty
    {
        Casual,
        Expert
    }

    private bool stopWorking = false;

    private void Start()
    {
        if (difficulty == Difficulty.Expert) {
            offRadiusChance *= 1.3f; // Increase chance for expert difficulty
            spawnIntervalMax *= 1.5f;

        } else if (difficulty == Difficulty.Casual) {
            scaleFactor = 1;
            distanceFactor = 1;
        }
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
            yield return new WaitForSeconds(UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax));
            SpawnFlotsam();
        }
    }

    private void SpawnFlotsam()
    {
        if (flotsamPrefabs.Length == 0) return;

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
            GameObject newFlotsam = Instantiate(flotsamPrefab, spawnPosition, Quaternion.identity);
            newFlotsam.transform.localScale *= scaleFactor; // Scale the flotsam prefab
        }
        else
        {
            // If occupied, retry
            SpawnFlotsam();
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
        Vector3 halfExtents = Vector3.Scale(flotsamCollider.size * 0.6f * distanceFactor, flotsamPrefab.transform.localScale);

        // Project the position to the XZ plane
        Vector3 projectedPosition = new Vector3(position.x, 0, position.z);

        // Check for collisions with existing flotsam (using a box overlap check)
        Collider[] colliders = Physics.OverlapBox(projectedPosition, halfExtents, Quaternion.identity, flotsamLayer);
        return colliders.Length > 0;
    }
}
