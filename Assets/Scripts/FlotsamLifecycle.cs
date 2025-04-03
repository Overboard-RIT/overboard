using UnityEngine;
using System.Collections;
using System;

public class FlotsamBehavior : MonoBehaviour
{
    public float floatSpeed = 1f;      // Speed of rising to surface
    private float sinkSpeed = 0.0f;     // Speed of sinking
    private float surfaceDuration = 5f; // Time staying on surface
    public float warningTime = 2f;     // Time before warning appears
    public float destroyDepth = -5f;   // Depth at which object is destroyed
    public float maxSinkSpeed = 2f;
    public float sinkAcceleration = 0.5f;
    public bool ableToSpawnCoin = true;
    public GameManager gameManager;     // Reference to GameManager script

    public GameObject warningSymbolPrefab; // Assign in Inspector (a prefab)
    private GameObject warningSymbol;      // The instantiated warning symbol
    public GameObject coinPrefab;
    private GameObject coinInstance;

    private enum FlotsamState { Rising, Floating, Sinking }
    private FlotsamState currentState = FlotsamState.Rising;

    void Start()
    {
        surfaceDuration = UnityEngine.Random.Range(8f, 14f);
        StartCoroutine(FloatToSurface());
    }

    private IEnumerator FloatToSurface()
    {
        while (transform.position.y < 0f)
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        currentState = FlotsamState.Floating;

        if (ableToSpawnCoin && UnityEngine.Random.Range(0, 2) == 1)
        {
            SpawnCoin();
        }

        StartCoroutine(StayOnSurface());
    }

    private IEnumerator StayOnSurface()
    {
        while (!ableToSpawnCoin && !gameManager.gameStarted) {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(surfaceDuration - warningTime); // Time before warning appears
        SpawnWarningSymbol(); // Spawn warning before sinking

        yield return new WaitForSeconds(warningTime); // Remaining time on surface
        currentState = FlotsamState.Sinking;

        StartCoroutine(SinkBelowWater());
    }

    private IEnumerator SinkBelowWater()
    {
        sinkSpeed = 0f; // Start at 0 and accelerate downward

        while (transform.position.y > destroyDepth)
        {
            sinkSpeed = Mathf.Min(sinkSpeed + sinkAcceleration * Time.deltaTime, maxSinkSpeed);
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Destroy flotsam
    }

    private void SpawnWarningSymbol()
    {
        if (warningSymbolPrefab != null)
        {
            warningSymbol = Instantiate(warningSymbolPrefab, transform.position + Vector3.up * 7f, Quaternion.Euler(90f, -90f, 0f));
        }
    }

    private void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            coinInstance = Instantiate(coinPrefab, transform.position + Vector3.up * 4f, Quaternion.Euler(90f, 0f, 0f));
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
