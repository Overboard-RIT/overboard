using System;
using System.Collections;
using UnityEngine;

public class FlotsamManager : MonoBehaviour
{
    public GameObject[] flotsamPrefabs; // Array of flotsam prefabs
    public int uiPlanksIndex = 4;
    public int startingFlotsamIndex = 2;
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

    private GameObject casualDifficulty;
    private GameObject expertDifficulty;

    void OnValidate()
    {
        MinGlobalBoundary = minGlobalBoundary;
        MaxGlobalBoundary = maxGlobalBoundary;
    }
    private void Start()
    {
        // boundsManager.BoundsMin = minGlobalBoundary;
        // boundsManager.BoundsMax = maxGlobalBoundary;
    }

    public void StartGame()
    {
        SpawnStartingFlotsam();
    }

    public void StartOnboarding()
    {
        SpawnOnboardingFlotsam();
    }

    public void ShowDifficulty()
    {
        SpawnDifficultyFlotsams();
    }

    public void HideDifficulty()
    {
        DestroyDifficultyFlotsams();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnFlotsamRoutine());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnFlotsamRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax));
            yield return StartCoroutine(SpawnFlotsam());
        }
    }

    private IEnumerator SpawnFlotsam()
    {
        if (flotsamPrefabs.Length != 0)
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
            }
            else
            {
                // If occupied, retry
                yield return new WaitForSeconds(0.05f); // Wait a bit before retrying
                StartCoroutine(SpawnFlotsam());
            }

        }

    }

    private void SpawnOnboardingFlotsam()
    {
        Vector3 spawnPosition = new Vector3(
            Mathf.Lerp(minGlobalBoundary.x, maxGlobalBoundary.x, 0.8f),
            -3f,
            Mathf.Lerp(minGlobalBoundary.z, maxGlobalBoundary.z, 0.5f)
        );
        GameObject flotsamPrefab = flotsamPrefabs[uiPlanksIndex];
        GameObject newFlotsam = Instantiate(flotsamPrefab, spawnPosition, Quaternion.Euler(90f, 90f, 0));
        newFlotsam.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        newFlotsam.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
        newFlotsam.GetComponent<UIPlatform>().PlatformEntered.AddListener(ShowDifficulty);
        newFlotsam.GetComponent<UIPlatform>().PlatformExited.AddListener(HideDifficulty);
    }

    private void SpawnDifficultyFlotsams()
    {
        Vector3 spawnPosition1 = new Vector3(
            Mathf.Lerp(minGlobalBoundary.x, maxGlobalBoundary.x, 0.5f),
            -3f,
            Mathf.Lerp(minGlobalBoundary.z, maxGlobalBoundary.z, 0.43f)
        );
        GameObject flotsamPrefab = flotsamPrefabs[uiPlanksIndex];
        casualDifficulty = Instantiate(flotsamPrefab, spawnPosition1, Quaternion.Euler(90f, 90f, 0));
        casualDifficulty.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        casualDifficulty.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;

        Vector3 spawnPosition2 = new Vector3(
            Mathf.Lerp(minGlobalBoundary.x, maxGlobalBoundary.x, 0.5f),
            -3f,
            Mathf.Lerp(minGlobalBoundary.z, maxGlobalBoundary.z, 0.57f)
        );
        expertDifficulty = Instantiate(flotsamPrefab, spawnPosition2, Quaternion.Euler(90f, 90f, 0));
        expertDifficulty.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        expertDifficulty.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
    }

    private void DestroyDifficultyFlotsams()
    {
        if (casualDifficulty != null)
        {
            casualDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
        }
        if (expertDifficulty != null)
        {
            expertDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
        }
    }

    private void SpawnStartingFlotsam()
    {
        Vector3 spawnPosition = 0.5f * (minGlobalBoundary + maxGlobalBoundary);
        spawnPosition.y = -3f;
        GameObject flotsamPrefab = flotsamPrefabs[startingFlotsamIndex];
        GameObject newFlotsam = Instantiate(flotsamPrefab, spawnPosition, Quaternion.identity);
        newFlotsam.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
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
        Vector3 projectedPosition = new Vector3(position.x, 0, position.z);

        // Check for collisions with existing flotsam (using a box overlap check)
        Collider[] colliders = Physics.OverlapBox(projectedPosition, halfExtents, Quaternion.identity, flotsamLayer);
        return colliders.Length > 0;
    }
}
