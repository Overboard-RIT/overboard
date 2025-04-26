using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;


// Andrew Black
// 4/8/25
// BootyManager handles the spawning of Gems. It could be adpated for handling the coin spawns as well
// hence the term 'booty' (pirate booty). 

public class BootyManager : MonoBehaviour
{
    public GameObject gem;
    private FlotsamManager flotsamManager;
    //midpoint between two rafts
    private Vector3 midpoint;
    public float minimumDistanceToSpawn = 5f; // Minimum distance to spawn the gem
    public float cooldown = 5f;

    // determine if a gem is ready to be spawned
    private bool readyToSpawn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flotsamManager = GetComponent<FlotsamManager>();
        enabled = false;
        StartCoroutine(StartCooldown());
        // for testing: fake midpoint so the gem spawns immediately
        // midpoint = new Vector3(0, 1, 0); // set height so it's visible
        // SpawnGem(); // spawn immediately
    }


    // Update is called once per frame
    void Update()
    {
        if (flotsamManager.GameDifficulty == FlotsamManager.Difficulty.Casual)
        {
            return;
        }

        if (!readyToSpawn)
        {
            return;
        }

        SpawnGem();
    }

    // this receives the positions of two flotsams, to determine the midpoint
    public void SendPositions(GameObject flotsam1, Vector3 flotsam2Pos)
    {
        Vector3 flotsam1Pos = flotsam1.transform.position;
        midpoint = 0.5f * (flotsam1Pos + flotsam2Pos);
        midpoint.y = 3f;

    }

    public void SpawnGem()
    {
        Debug.Log("Spawning gem...");
        // GameObject[] flotsams = Array.FindAll(GameObject.FindGameObjectsWithTag("Flotsam"), flotsam => (flotsam.GetComponent<FlotsamLifecycle>().currentState == FlotsamLifecycle.FlotsamState.Floating));
        GameObject[] flotsams = GameObject.FindGameObjectsWithTag("Flotsam");
        if (flotsams.Length < 2)
        {
            Debug.Log("Not enough flotsams to spawn a gem.");
            StartCoroutine(StartCooldown());
            return;
        }
        ;

        Vector3 spawnLocation = Vector3.zero;
        while (spawnLocation == Vector3.zero)
        {
            // Get a random flotsam
            GameObject flotsam = flotsams[UnityEngine.Random.Range(0, flotsams.Length)];
            foreach (GameObject otherFlotsam in flotsams)
            {
                if (otherFlotsam == flotsam) continue;
                if (Vector3.Distance(flotsam.transform.position, otherFlotsam.transform.position) > minimumDistanceToSpawn)
                {
                    spawnLocation = (flotsam.transform.position + otherFlotsam.transform.position) / 2;
                    spawnLocation.y = 3f; // Set height so it's visible
                    Instantiate(gem, spawnLocation, Quaternion.Euler(90, 0, 0));
                    StartCoroutine(StartCooldown());
                    return;
                }
            }

            Debug.Log("No valid spawn location found. Retrying...");
        }
    }

    

    // // spawns the gem!
    // private IEnumerator SpawnGem()
    // {
    //     yield return new WaitForSeconds(1.5f); // wait for platform to rise

    //     //Debug.Log("spawngem");
    //     StartCoroutine(StartCooldown());

    // }

    private IEnumerator StartCooldown()
    {
        readyToSpawn = false;

        yield return new WaitForSeconds(cooldown);

        readyToSpawn = true;
        //Debug.Log("Gem: readyToSpawn = true");
    }

}
