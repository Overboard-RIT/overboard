using UnityEngine;

public class FlotsamCollider : MonoBehaviour
{
    public bool PlayerContact { get; set; } = false;

    private bool leftFootContact = false;
    private bool rightFootContact = false;

    public void EndGame()
    {
        Destroy(this);
    }

    void OnTriggerEnter(Collider trigger)
    {
        PlayerContact = true;
        GetComponent<FloatingBehavior>().Bounce();

        if (trigger.gameObject.tag == "LeftFoot")
        {
            leftFootContact = true;
            return;
        }
        if (trigger.gameObject.tag == "RightFoot")
        {
            rightFootContact = true;
            return;
        }
    }

    void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "LeftFoot")
        {
            leftFootContact = false;
        }
        if (trigger.gameObject.tag == "RightFoot")
        {
            rightFootContact = false;
        }
        if (!leftFootContact && !rightFootContact)
        {
            PlayerContact = false;
            GetComponent<FloatingBehavior>().Bounce();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
        Vector3 size = GetComponent<BoxCollider>().size;
        Vector3 center = GetComponent<BoxCollider>().center;
        Gizmos.DrawCube(center, size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
    }
}
