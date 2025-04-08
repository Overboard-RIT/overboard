using UnityEngine;
using System.Collections;

public class BootyConfig : MonoBehaviour
{

    public int coinsToSpawn = 30;
    public GameObject coinPrefab;
    private GameObject coinInstance;
    public GameObject gemPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(1.5f);
        if (coinPrefab != null)
        {
            coinInstance = Instantiate(coinPrefab, transform.position + new Vector3(1, 5, 0), Quaternion.Euler(90f, 0f, 0f));
        }
    }
}
