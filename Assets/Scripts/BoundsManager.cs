using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    public float margin = 3f;

    public Vector3 boundsMin;
    public Vector3 boundsMax;
    public GameObject spawnManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FlotsamManager flotsamManager = spawnManager.GetComponent<FlotsamManager>();
        flotsamManager.MinGlobalBoundary = boundsMin;
        flotsamManager.MaxGlobalBoundary = boundsMax;
    }

    public bool CheckInBounds(Vector3 position) {
        if (position.x < boundsMin.x - margin || position.x > boundsMax.x + margin) {
            return false;
        }
        if (position.z < boundsMin.z - margin || position.z > boundsMax.z + margin) {
            return false;
        }
        return true;
    }
}
