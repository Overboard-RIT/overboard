using UnityEngine;
using System.Collections;
using System;

public class FlotsamLifecycle : MonoBehaviour
{
    public float floatSpeed = 1f;      // Speed of rising to surface
    private float sinkSpeed = 0.0f;     // Speed of sinking
    private float surfaceDuration = 5f; // Time staying on surface
    public float warningTime = 2f;     // Time before warning appears
    public float destroyDepth = -5f;   // Depth at which object is destroyed
    public float maxSinkSpeed = 2f;
    public float sinkAcceleration = 0.5f;
    public bool ableToSpawnCoin = true;
    public GameManager gameManager;    // Reference to GameManager script
    public GameObject bubbles;         // Assign in Inspector (a prefab)
    public GameObject warningSymbolPrefab; // Assign in Inspector (a prefab)
    private GameObject warningSymbol;      // The instantiated warning symbol
    public GameObject coinPrefab;
    private GameObject coinInstance;

    public enum FlotsamState { Rising, Floating, Sinking }
    [NonSerialized]
    public FlotsamState currentState = FlotsamState.Rising;

    void Start()
    {
        gameManager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();
        surfaceDuration = UnityEngine.Random.Range(5f, 8f);
        StartCoroutine(FloatToSurface());

        if (ableToSpawnCoin && UnityEngine.Random.Range(0, 4) != 1)
        {
            StartCoroutine(SpawnCoin());
        }
    }

    public void EndGame()
    {
        Destroy(warningSymbol);
        GetComponent<FlotsamCollider>().EndGame();
        GetComponent<FloatingBehavior>().EndGame();
        StopAllCoroutines();
        StartCoroutine(SinkBelowWater());
    }

    private IEnumerator FloatToSurface()
    {
        while (transform.position.y < 0.4f)
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
        currentState = FlotsamState.Floating;
        GetComponent<FloatingBehavior>().startPosition = transform.position;

        StartCoroutine(StayOnSurface());
    }

    private IEnumerator StayOnSurface()
    {
        // while (!ableToSpawnCoin && !gameManager.gameStarted)
        while (!gameManager.gameStarted)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(surfaceDuration - warningTime); // Time before warning appears
        SpawnWarningSymbol(); // Spawn warning before sinking
        GetComponent<FloatingBehavior>().Shake();

        yield return new WaitForSeconds(warningTime); // Remaining time on surface
        currentState = FlotsamState.Sinking;

        StartCoroutine(SinkBelowWater());
    }

    private IEnumerator SinkBelowWater()
    {
        if (GetComponent<UIPlatform>() != null)
        {
            Destroy(GetComponent<UIPlatform>());
        }
        GameObject newBubbles = Instantiate(bubbles, transform.position, Quaternion.Euler(90f, 0f, 0f));
        sinkSpeed = 0f; // Start at 0 and accelerate downward

        while (transform.position.y > destroyDepth)
        {
            sinkSpeed = Mathf.Min(sinkSpeed + sinkAcceleration * Time.deltaTime, maxSinkSpeed);
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(newBubbles); // Destroy bubbles
        Destroy(gameObject); // Destroy flotsam
    }

    private void SpawnWarningSymbol()
    {
        if (warningSymbolPrefab != null)
        {
            warningSymbol = Instantiate(warningSymbolPrefab, transform.position + new Vector3(-1, 7, 0), Quaternion.Euler(90f, -90f, 0f));
        }
    }

    private IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(1.5f);
        if (coinPrefab != null)
        {
            coinInstance = Instantiate(coinPrefab, transform.position + new Vector3(1, 5, 0), Quaternion.Euler(90f, 0f, 0f));
        }
    }

    private void DestroyCoin()
    {
        if (coinInstance != null)
        {
            Destroy(coinInstance);
        }
    }
}
