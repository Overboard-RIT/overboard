using UnityEngine;
using System.Collections.Generic;

public class Denoiser : MonoBehaviour
{
    public int windowSize = 5;
    private Queue<Vector3> leftFootPositions = new Queue<Vector3>();
    private Queue<Vector3> rightFootPositions = new Queue<Vector3>();
    // private Queue<Vector3> centerPositions = new Queue<Vector3>();

    public int playerNumber;

    public GameObject leftFoot; 
    public GameObject rightFoot;
    public PlayerPositionDetection playerPositionDetection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        List<PlayerPositionDetection.PlayerPosition> allPositions = playerPositionDetection.GetActivePositions();
        if (allPositions.Count < playerNumber + 1) return;
        PlayerPositionDetection.PlayerPosition positions = allPositions[playerNumber];
        SetNextPosition(positions.leftFoot, positions.rightFoot, positions.center);
        UpdatePositions();
    }

    private void SetNextPosition(Vector3 leftFoot, Vector3 rightFoot, Vector3 center)
    {
        leftFootPositions.Enqueue(leftFoot);
        rightFootPositions.Enqueue(rightFoot);
        // centerPositions.Enqueue(center);

        if (leftFootPositions.Count > windowSize)
        {
            leftFootPositions.Dequeue();
        }
        if (rightFootPositions.Count > windowSize)
        {
            rightFootPositions.Dequeue();
        }
        // if (centerPositions.Count > windowSize)
        // {
        //     centerPositions.Dequeue();
        // }
    }

    private void UpdatePositions() {
        Vector3 leftFootPosition = leftFoot.transform.position;
        Vector3 rightFootPosition = rightFoot.transform.position;
        // Vector3 centerPosition = center.transform.position;

        Vector3 leftFootAverage = new Vector3(0, 0, 0);
        Vector3 rightFootAverage = new Vector3(0, 0, 0);
        // Vector3 centerAverage = new Vector3(0, 0, 0);

        foreach (Vector3 position in leftFootPositions)
        {
            leftFootAverage += position;
        }
        foreach (Vector3 position in rightFootPositions)
        {
            rightFootAverage += position;
        }
        // foreach (Vector3 position in centerPositions)
        // {
        //     centerAverage += position;
        // }

        leftFootAverage /= leftFootPositions.Count;
        rightFootAverage /= rightFootPositions.Count;
        // centerAverage /= centerPositions.Count;

        leftFoot.transform.position = leftFootAverage;
        rightFoot.transform.position = rightFootAverage;
        // center.transform.position = centerAverage;
    }
}
