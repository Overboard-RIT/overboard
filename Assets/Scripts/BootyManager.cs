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

    //midpoint between two rafts
    private Vector3 midpoint;

    // determine if a gem is ready to be spawned
    private bool readyToSpawn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // for testing: fake midpoint so the gem spawns immediately
        // midpoint = new Vector3(0, 1, 0); // set height so it's visible
        // SpawnGem(); // spawn immediately
    }


    // Update is called once per frame
    void Update()
    {
        // check to ensure that the midpoint between two rafts isn't exactly 0,0,0
        // this is sorta spaghetti and serves as an alternative to checking for a null value,
        // as a Vector3 can't be null (defaults zero). If zero, and RNG rolls correctly,
        // and we're ready to spawn, spawn a gem at the midpoint
        if((midpoint != Vector3.zero) && (UnityEngine.Random.Range(0,4) == 1) && readyToSpawn)
        {
            Debug.Log(midpoint);
            SpawnGem();
        }
    }

    // this receives the positions of two flotsams, to determine the midpoint
    public void SendPositions(GameObject flotsam1, Vector3 flotsam2Pos)
    {
        Vector3 flotsam1Pos = flotsam1.transform.position;
        midpoint = (flotsam1Pos + flotsam2Pos) / 2f;

    }

    // spawns the gem!
    private void SpawnGem()
    {
        Debug.Log("spawngem");
        Instantiate(gem, midpoint, Quaternion.identity);
        StartCoroutine(StartCooldown());

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
