using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void OnDrawGizmos() {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        float radius = GetComponent<CapsuleCollider>().radius;
        Vector3 center = transform.position + GetComponent<CapsuleCollider>().center;
        Gizmos.DrawSphere(center, radius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }
}
