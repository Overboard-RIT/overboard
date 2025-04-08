using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class BootyManager : MonoBehaviour
{

    public GameObject gem;

    private Vector3 midpoint;

    private bool readyToSpawn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((midpoint != Vector3.zero) && (UnityEngine.Random.Range(0,4) == 1) && readyToSpawn)
        {
            Debug.Log(midpoint);
            SpawnGem();
        }
    }

    public void SendPositions(GameObject flotsam1, Vector3 flotsam2Pos)
    {
        Vector3 flotsam1Pos = flotsam1.transform.position;
        midpoint = (flotsam1Pos + flotsam2Pos) / 2f;

    }

    private void SpawnGem()
    {
        Debug.Log("spawngem");
        Instantiate(gem, midpoint, Quaternion.identity);
        StartCooldown();
    }

    private IEnumerator StartCooldown()
    {
        readyToSpawn = false;
        Debug.Log("Gem: readyToSpawn = false");

        yield return new WaitForSeconds(5);

        readyToSpawn = true;
        Debug.Log("Gem: readyToSpawn = true");
    }

}
