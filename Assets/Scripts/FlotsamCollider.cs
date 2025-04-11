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
}
