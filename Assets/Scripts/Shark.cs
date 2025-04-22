using UnityEngine;

public class Shark : MonoBehaviour
{
    public Vector3 playerPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPosition;
    }
}
