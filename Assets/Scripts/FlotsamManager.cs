using System;
using System.Collections;
using UnityEngine;

public class FlotsamManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Casual; // Difficulty levels
    [Range(0.0f, 1.0f)]
    public float scaleFactor = 1f; // Scale factor for flotsam size
    [Range(1.0f, 2.0f)]
    public float distanceFactor = 1f; // Distance factor for flotsam spawn distance
    [Header("Flotsam Settings")]
    public GameObject[] flotsamPrefabs; // Array of flotsam prefabs
    public GameObject playerPlatformPrefab; // Prefab for the player platform
    public GameObject casualDifficultyPrefab; // Prefab for casual difficulty
    public GameObject expertDifficultyPrefab; // Prefab for expert difficulty
    public int startingFlotsamIndex = 2;
    public float spawnIntervalMin = 3f; // Time between spawns
    public float spawnIntervalMax = 7f;
    public float spawnRadius = 15f; // Radius around the player to spawn flotsam
    public float offRadiusChance = 0.1f; // Chance for flotsam to spawn outside the radius
    public float offRadiusMaxDistance = 30f; // Max distance for off-radius spawn
    public LayerMask flotsamLayer; // Layer to check for existing flotsam
    public float spawnY = -5f; // Initial spawn height (below water)
    public Transform playerTransform; // Reference to the player's transform

    public Vector3 MinGlobalBoundary { get; set; }
    public Vector3 MaxGlobalBoundary { get; set; }

    public enum Difficulty
    {
        Casual,
        Expert
    }

    private GameObject playerPlatform;
    private GameObject casualDifficulty;
    private GameObject expertDifficulty;
    public GameObject startingPlatform;

    
    void OnDrawGizmos() {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1, 0, 1));
        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
        Gizmos.DrawSphere(playerTransform.position, spawnRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
        Gizmos.DrawWireSphere(playerTransform.position, offRadiusMaxDistance);
    }

    void Start()
    {
        // boundsManager.BoundsMin = MinGlobalBoundary;
        // boundsManager.BoundsMax = MaxGlobalBoundary;
    }

    // public void StartGame()
    // {
    //     if (difficulty == Difficulty.Expert)
    //     {
    //         offRadiusChance *= 1.3f; // Increase chance for expert difficulty
    //         spawnIntervalMax *= 1.5f;

    //     }
    //     else if (difficulty == Difficulty.Casual)
    //     {
    //         scaleFactor = 1;
    //         distanceFactor = 1;
    //     }

    //     SpawnStartingFlotsam();
    // }

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
        Debug.Log("i am spawning blocks n shit");
        while (true)
        {
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
    private void SpawnOnboardingFlotsam()
    {
        // gameManager.GetComponent<VoiceTriggers>().OnOnboardingStarted();
        Vector3 spawnPosition = new Vector3(
            Mathf.Lerp(MinGlobalBoundary.x, MaxGlobalBoundary.x, 0.8f),
            -3f,
            Mathf.Lerp(MinGlobalBoundary.z, MaxGlobalBoundary.z, 0.2f)
        );
        playerPlatform = Instantiate(playerPlatformPrefab, spawnPosition, Quaternion.Euler(90f, 90f, 0));
        playerPlatform.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        playerPlatform.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
        playerPlatform.GetComponent<UIPlatform>().PlatformEntered.AddListener(ShowDifficulty);
        playerPlatform.GetComponent<UIPlatform>().PlatformExited.AddListener(() =>
        {
            if (
                casualDifficulty.GetComponent<UIPlatform>().enterTimerStartedAt == 0 &&
                expertDifficulty.GetComponent<UIPlatform>().enterTimerStartedAt == 0
            )
            {
                HideDifficulty();
            }
        });
    }

    private void SpawnDifficultyFlotsams()
    {
        gameManager.GetComponent<VoiceTriggers>().OnPromptDifficulty();
        Vector3 spawnPosition1 = new Vector3(
            Mathf.Lerp(MinGlobalBoundary.x, MaxGlobalBoundary.x, 0.5f),
            -3f,
            Mathf.Lerp(MinGlobalBoundary.z, MaxGlobalBoundary.z, 0.13f)
        );
        casualDifficulty = Instantiate(casualDifficultyPrefab, spawnPosition1, Quaternion.Euler(90f, 90f, 0));
        casualDifficulty.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        casualDifficulty.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
        casualDifficulty.GetComponent<UIPlatform>().platformStandDuration = 1.5f;
        casualDifficulty.GetComponent<UIPlatform>().PlatformEntered.AddListener(() =>
        {
            difficulty = Difficulty.Casual;
            gameManager.GetComponent<VoiceTriggers>().OnOnboardingEnded();
            expertDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
            playerPlatform.GetComponent<FlotsamLifecycle>().EndGame();
            casualDifficulty.GetComponent<UIPlatform>().PlatformEntered.RemoveAllListeners();
            casualDifficulty.GetComponent<UIPlatform>().PlatformExited.RemoveAllListeners();
            casualDifficulty.GetComponent<UIPlatform>().PlatformExited.AddListener(() =>
            {
                casualDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
            });
            SpawnStartingFlotsam();
        });

        Vector3 spawnPosition2 = new Vector3(
            Mathf.Lerp(MinGlobalBoundary.x, MaxGlobalBoundary.x, 0.5f),
            -3f,
            Mathf.Lerp(MinGlobalBoundary.z, MaxGlobalBoundary.z, 0.27f)
        );
        expertDifficulty = Instantiate(expertDifficultyPrefab, spawnPosition2, Quaternion.Euler(90f, 90f, 0));
        expertDifficulty.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        expertDifficulty.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
        expertDifficulty.GetComponent<UIPlatform>().platformStandDuration = 1.5f;
        expertDifficulty.GetComponent<UIPlatform>().PlatformEntered.AddListener(() =>
        {
            difficulty = Difficulty.Expert;
            gameManager.GetComponent<VoiceTriggers>().OnOnboardingEnded();
            casualDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
            playerPlatform.GetComponent<FlotsamLifecycle>().EndGame();
            expertDifficulty.GetComponent<UIPlatform>().PlatformEntered.RemoveAllListeners();
            expertDifficulty.GetComponent<UIPlatform>().PlatformExited.RemoveAllListeners();
            expertDifficulty.GetComponent<UIPlatform>().PlatformExited.AddListener(() =>
            {
                expertDifficulty.GetComponent<FlotsamLifecycle>().EndGame();
            });
            SpawnStartingFlotsam();
        });
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
        Vector3 spawnPosition = 0.5f * (MinGlobalBoundary + MaxGlobalBoundary);
        spawnPosition.y = -3f;
        GameObject flotsamPrefab = flotsamPrefabs[startingFlotsamIndex];
        startingPlatform = Instantiate(flotsamPrefab, spawnPosition, Quaternion.identity);
        startingPlatform.GetComponent<FlotsamLifecycle>().ableToSpawnCoin = false;
        startingPlatform.GetComponent<FlotsamLifecycle>().sinkableByItself = false;
        startingPlatform.GetComponent<FlotsamLifecycle>().surfaceDuration += 4;
        startingPlatform.AddComponent<UIPlatform>();
        startingPlatform.GetComponent<UIPlatform>().platformStandDuration = 3f;
        startingPlatform.GetComponent<UIPlatform>().PlatformEntered.AddListener(() =>
        {
            gameManager.StartCountdown();
        });
    }

    // Check if the spawn position is within the global boundaries
    private bool IsPositionWithinGlobalBoundary(Vector3 position)
    {
        // Ensure that the position is within the x and z boundaries (ignoring y-axis)
        if (position.x >= MinGlobalBoundary.x && position.x <= MaxGlobalBoundary.x &&
            position.z >= MinGlobalBoundary.z && position.z <= MaxGlobalBoundary.z)
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
        Vector3 projectedPosition = new Vector3(position.x, 0.5f, position.z);

        // Check for collisions with existing flotsam (using a box overlap check)
        Collider[] colliders = Physics.OverlapBox(projectedPosition, halfExtents, Quaternion.identity, flotsamLayer);
        return colliders.Length > 0;
    }
}
