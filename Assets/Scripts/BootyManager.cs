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

    // determine if a gem is ready to be spawned
    private bool readyToSpawn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flotsamManager = GetComponent<FlotsamManager>();
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
        // check to ensure that the midpoint between two rafts isn't exactly 0,0,0
        // this is sorta spaghetti and serves as an alternative to checking for a null value,
        // as a Vector3 can't be null (defaults zero). If zero, and RNG rolls correctly,
        // and we're ready to spawn, spawn a gem at the midpoint
        // Debug.Log("midpoint: " + midpoint + " " + "ready to spawn:" + readyToSpawn);
        // if((midpoint != Vector3.zero) && (UnityEngine.Random.Range(0,4) == 1) && readyToSpawn)
        if ((midpoint != Vector3.zero) && readyToSpawn && (UnityEngine.Random.Range(0, 4) == 1))
        {
            // Debug.Log(midpoint);
            StartCoroutine(SpawnGem());
            readyToSpawn = false;
            // Debug.Log("Gem: readyToSpawn = false");
        }
    }

    // this receives the positions of two flotsams, to determine the midpoint
    public void SendPositions(GameObject flotsam1, Vector3 flotsam2Pos)
    {
        Vector3 flotsam1Pos = flotsam1.transform.position;
        midpoint = 0.5f * (flotsam1Pos + flotsam2Pos);
        midpoint.y = 3f;

    }

    // spawns the gem!
    private IEnumerator SpawnGem()
    {
        yield return new WaitForSeconds(1.5f); // wait for platform to rise

        //Debug.Log("spawngem");
        Instantiate(gem, midpoint, Quaternion.Euler(90, 0, 0));
        StartCoroutine(StartCooldown());

    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(5);

        readyToSpawn = true;
        //Debug.Log("Gem: readyToSpawn = true");
    }

}
