using System;
using UnityEngine;

public class FloatingBehavior : MonoBehaviour
{

    public enum PlatformType
    {
        Barrel,
        Raft,
        Donut,
        Crate,
        UIPlanks
    }
    [Header("Floating Settings")]

    public PlatformType platformType; // Public field to select the platform type in the Inspector

    private float amplitude = 0.1f; // Amplitude of the floating effect

    public float rotationSpeed = 10f; // Speed of slow rotation
    private bool isRollingBarrel = false;
    private Vector3 barrelRollingDirection = Vector3.zero;

    [NonSerialized]
    public Vector3 startPosition;

    [Header("Bounce Settings")]
    public bool bounce = false;
    [Header("Position")]
    [Range(0, 1)]
    public float bouncePositionDuration = 1f;
    [Range(0, 10)]
    public float bouncePositionStartingAmplitude = 0.2f;
    [Range(0, 1)]
    public float bouncePositionPeriod = 0.75f;
    [Header("Rotation")]
    public float bounceRotationDuration = 1.5f;
    [Range(0, 1)]
    public float bounceRotationStartingAmplitude = 0.02f;
    [Range(0, 1)]
    public float bounceRotationPeriod = 0.5f;

    [NonSerialized]
    public float startedBouncingAt = 0f;

    [NonSerialized]
    public bool isBouncing = false;

    [Header("Shake Settings")]
    public bool shake = false;
    public float shakeAmplitude = 0.02f;
    public float shakePeriod = 0.15f;
    [NonSerialized]
    public bool isShaking = false;

    void OnValidate()
    {
        if (bounce)
        {
            bounce = false;
            isBouncing = true;
            startedBouncingAt = Time.time;
        }

        if (shake)
        {
            shake = false;
            isShaking = true;
        }
    }

    public void Bounce()
    {
        if (isBouncing || GetComponent<FlotsamLifecycle>().currentState != FlotsamLifecycle.FlotsamState.Floating)
        {
            return;
        }
        isBouncing = true;
        startedBouncingAt = Time.time;
    }

    public void Shake()
    {
        isShaking = true;
    }

    public void EndGame()
    {
        Destroy(this);
    }

    void Start()
    {
        if (rotationSpeed != 0)
        {
            rotationSpeed += UnityEngine.Random.Range(0, 5);

            if (UnityEngine.Random.value < 0.5f)
            {
                rotationSpeed *= -1;
            }
        }

        switch (platformType)
        {
            case PlatformType.Barrel:
                amplitude = 10f;
                if (UnityEngine.Random.value < 0.5f)
                {
                    transform.Rotate(
                        0f,
                        UnityEngine.Random.Range(0, 360f),
                        90f,
                    Space.World);
                    barrelRollingDirection = transform.forward;
                    isRollingBarrel = true;
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
        if (isRollingBarrel)
        {
            transform.position += barrelRollingDirection * Time.deltaTime;
            transform.Rotate(0f, -5 * rotationSpeed * Time.deltaTime, 0f, Space.Self);
        }
        else
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
            // Add sinusoidal movement to the X rotation
            float sinRotationX = Mathf.Sin(Time.time) * amplitude;
            transform.rotation = Quaternion.Euler(sinRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }


        // Add bounce effect
        if (isBouncing)
        {
            float t = Time.time - startedBouncingAt;

            if (t > bouncePositionDuration && t > bounceRotationDuration)
            {
                isBouncing = false;
                return;
            }

            if (t > bouncePositionDuration)
            {
                transform.position = startPosition; // Reset position after bounce
            }
            else
            {
                float bouncePosition = BouncePositionFunction(t);
                transform.position = startPosition + new Vector3(0f, bouncePosition, 0f);
            }

            if (t < bounceRotationDuration)
            {
                float bounceRotation = BounceRotationFunction(t);
                transform.rotation = Quaternion.Euler(
                    transform.rotation.eulerAngles.x + bounceRotation,
                    transform.rotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z
                );
            }
        }

        if (isShaking)
        {
            float t = Time.time;
            float shake = ShakeFunction(t);
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x + shake,
                transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z
            );
        }
    }

    private float BouncePositionFunction(float t)
    {
        float A = bouncePositionStartingAmplitude;
        float T = bouncePositionPeriod;
        float D = bouncePositionDuration;

        float falloff = -A * (1f - t / D);
        float frequency = 2f * Mathf.PI / T;
        float bounce = falloff * Mathf.Sin(frequency * t);
        return bounce;
    }

    private float BounceRotationFunction(float t)
    {
        float A = bounceRotationStartingAmplitude;
        float T = bounceRotationPeriod;
        float D = bounceRotationDuration;

        float falloff = -A * (1f - t / D);
        float frequency = 2f * Mathf.PI / T;
        float bounce = falloff * Mathf.Sin(frequency * t);
        return bounce * 360f;
    }

    private float ShakeFunction(float t)
    {
        float A = shakeAmplitude;
        float T = shakePeriod;

        float frequency = 2f * Mathf.PI / T;
        float shake = A * Mathf.Sin(frequency * t);
        return shake * 360f;
    }
}
