using System;
using UnityEngine;

public class FloatingBehavior : MonoBehaviour
{

    public enum PlatformType
    {
        Barrel,
        Raft,
        Donut,
        Crate
    }

    public PlatformType platformType; // Public field to select the platform type in the Inspector

    private float amplitude = 0.1f; // Amplitude of the floating effect

    public float rotationSpeed = 10f; // Speed of slow rotation

    private Vector3 startPosition;

    void Start()
    {
        rotationSpeed += UnityEngine.Random.Range(0, 5);

        if (UnityEngine.Random.value < 0.5f)
        {
            rotationSpeed *= -1;
        }

        switch (platformType)
        {
            case PlatformType.Barrel:
                amplitude = 10f;
                if (UnityEngine.Random.value < 0.5f)
                {
                    transform.Rotate(0f, 0f, 90f, Space.World);
                }
                break;
            case PlatformType.Raft:
                amplitude = 6f;
                break;
            case PlatformType.Donut:
                amplitude = 4f;
                break;
            case PlatformType.Crate:
                if (UnityEngine.Random.value < 0.5f)
                {
                    transform.Rotate(0f, 0f, 90f, Space.World);
                }
                amplitude = 12f;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        // Rotate slightly
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);

        // Add sinusoidal movement to the X rotation
        float sinRotationX = Mathf.Sin(Time.time) * amplitude;
        transform.rotation = Quaternion.Euler(sinRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
