using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerPositionDetection playerPositionDetection;

    public float xOff = -0.74f;
    public float zOff = 3.32f;

    private bool gameStarted = false;

    private Vector3 positionFromKinect;

    // void Update()
    // {
    //     // Handle movement based on WASD input
    //     float moveX = Input.GetAxis("Horizontal");
    //     float moveZ = Input.GetAxis("Vertical");

    //     // Create movement vector relative to the player rotation
    //     if (moveX != 0 || moveZ != 0)
    //     {
    //         Vector3 movement = (transform.right * moveX + transform.forward * moveZ).normalized * moveSpeed;
    //         // rb.linearVelocity = new Vector3(movement.x, 0, movement.z);
    //         transform.position += movement * Time.deltaTime;
    //     }
    // }

    void Start()
    {
        positionFromKinect = new Vector3(0, 0, 0);
        gameStarted = false;
    }

    void Update()
    {
    //     PlayerPositionDetection.PlayerPosition positions = playerPositionDetection.GetPlayerPosition();

    //     if (tag == "LeftFoot")
    //     {
    //         positionFromKinect = playerPositionDetection.GetPlayerPosition().leftFoot;
    //     }
    //     else if (tag == "RightFoot")
    //     {
    //         positionFromKinect = playerPositionDetection.GetPlayerPosition().rightFoot;
    //     }
    //     else
    //     {
    //         positionFromKinect = positions.center;
    //     }

    //     if (positionFromKinect.x != 0 || positionFromKinect.y != 0 || positionFromKinect.z != 0)
    //     {
    //         positionFromKinect.x += xOff;
    //         positionFromKinect.z += zOff;
    //         transform.position = positionFromKinect;
    //         if (!gameStarted)
    //         {
    //             gameStarted = true;
    //         }
    //     }
    }
}
