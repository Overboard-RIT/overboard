using UnityEngine;
using System;

public class BoundsManager : MonoBehaviour
{
    private float margin = 3f;

    private Vector3 boundsMin;
    private Vector3 boundsMax;
    public GameObject ocean;
    // public GameObject raft;

    public float Margin
    {
        get => margin;
        set
        {
            margin = value;
            RepositionOcean();
            // RepositionRaft();
        }
    }

    public Vector3 BoundsMin
    {
        get => boundsMin;
        set
        {
            boundsMin = value;
            RepositionOcean();
            // RepositionRaft();
        }
    }
    public Vector3 BoundsMax
    {
        get => boundsMax;
        set
        {
            boundsMax = value;
            RepositionOcean();
            // RepositionRaft();
        }
    }

    public bool CheckInBounds(Vector3 position)
    {
        if (position.x < boundsMin.x - margin || position.x > boundsMax.x + margin)
        {
            return false;
        }
        if (position.z < boundsMin.z - margin || position.z > boundsMax.z + margin)
        {
            return false;
        }
        return true;
    }

    private void RepositionOcean() {
        Vector3 newPosition = new Vector3(
            (boundsMin.x + boundsMax.x) / 2f,
            ocean.transform.position.y,
            (boundsMin.z + boundsMax.z) / 2f
        );
        ocean.transform.position = newPosition;
        ocean.transform.localScale = new Vector3(
            (boundsMax.z - boundsMin.z) * 0.1f  * 1080f / 1920f + margin * 0.1f,
            ocean.transform.localScale.y,
            (boundsMax.z - boundsMin.z) * 0.1f + margin * 0.1f
        );
    }

    // private void RepositionRaft() {
    //     Vector3 newPosition = new Vector3(
    //         (boundsMin.x + boundsMax.x) / 2f,
    //         ocean.transform.position.y,
    //         (boundsMin.z + boundsMax.z) / 2f
    //     );
    //     raft.transform.position = newPosition;
    // }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector3 center = (boundsMin + boundsMax) / 2f;
        Vector3 size = new Vector3(
            boundsMax.x - boundsMin.x,
            10f,
            boundsMax.z - boundsMin.z
        );
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.green;
        Vector3 sizePlusMargin = new Vector3(
            boundsMax.x - boundsMin.x + margin * 2f,
            10f + margin * 2f,
            boundsMax.z - boundsMin.z + margin * 2f
        );
        Gizmos.DrawWireCube(center, sizePlusMargin);
    }
}