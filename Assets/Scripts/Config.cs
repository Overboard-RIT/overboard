using UnityEngine;
using System.Collections.Generic;

public class Config : MonoBehaviour
{
    [Header("Sockets")]
    public FlotsamManager flotsamManager;
    public BoundsManager boundsManager;
    public WaterTrigger waterTrigger;
    public GameTimer gameTimer;
    public List<Denoiser> denoisers;

    [Header("Controls")]
    [SerializeField]
    private Vector3 minBounds = new Vector3(-10, 0, -45);
    [SerializeField]
    private Vector3 maxBounds = new Vector3(10, 0, -10);
    public bool centerCamera = true;
    [SerializeField]
    [Range(0, 10)]
    private float margin = 3f;
    [SerializeField]
    private float timerStartsAt = 60.99f;
    [SerializeField]
    [Range(1, 30)]
    private int denoiseWindowSize = 30;
    [SerializeField]
    [Range(0, 1)]
    private float overboardGracePeriod = 0.25f;
    [SerializeField]
    [Range(0, 5)]
    private float overboardCooldown = 3f;
    [SerializeField]
    [Range(3, 7)]
    private float spawnIntervalMin = 3f;
    [SerializeField]
    [Range(4, 10)]
    private float spawnIntervalMax = 7f;
    [SerializeField]
    [Range(10, 20)]
    private float spawnRadius = 15f;
    [SerializeField]
    [Range(0, 1)]
    private float offRadiusChance = 0.1f;
    [SerializeField]
    [Range(20, 50)]
    private float offRadiusMaxDistance = 30f;
    [SerializeField]
    private Vector3 cameraPosition;

    public Vector3 MinBounds
    {
        get => minBounds;
        set
        {
            minBounds = new Vector3(value.x, 0, value.z);

            if (minBounds.x > maxBounds.x)
            {
                minBounds.x = maxBounds.x;
                maxBounds.x = value.x;
            }

            if (minBounds.z > maxBounds.z)
            {
                minBounds.z = maxBounds.z;
                maxBounds.z = value.z;
            }

            flotsamManager.MinGlobalBoundary = minBounds;
            boundsManager.BoundsMin = minBounds;

            if (!centerCamera) return;
            CameraPosition = new Vector3(
                0.5f * (minBounds.x + maxBounds.x),
                Camera.main.transform.position.y,
                0.5f * (minBounds.z + maxBounds.z)
            );
        }
    }

    public Vector3 MaxBounds
    {
        get => maxBounds;
        set
        {
            maxBounds = new Vector3(value.x, 0, value.z);

            if (maxBounds.x < minBounds.x)
            {
                maxBounds.x = minBounds.x;
                maxBounds.x = value.x;
            }

            if (maxBounds.z < minBounds.z)
            {
                maxBounds.z = minBounds.z;
                maxBounds.z = value.z;
            }

            flotsamManager.MaxGlobalBoundary = maxBounds;
            boundsManager.BoundsMax = maxBounds;

            if (!centerCamera) return;
            CameraPosition = new Vector3(
                0.5f * (minBounds.x + maxBounds.x),
                Camera.main.transform.position.y,
                0.5f * (minBounds.z + maxBounds.z)
            );
        }
    }

    public float Margin
    {
        get => margin;
        set
        {
            margin = value;
            boundsManager.Margin = margin;
        }
    }

    public float TimerStartsAt
    {
        get => timerStartsAt;
        set
        {
            timerStartsAt = value;
            gameTimer.timeRemaining = timerStartsAt;
        }
    }

    public int DenoiseWindowSize
    {
        get => denoiseWindowSize;
        set
        {
            denoiseWindowSize = value;
            foreach (Denoiser denoiser in denoisers)
            {
                denoiser.windowSize = denoiseWindowSize;
            }
        }
    }

    public float OverboardGracePeriod
    {
        get => overboardGracePeriod;
        set
        {
            overboardGracePeriod = value;
            waterTrigger.gracePeriod = overboardGracePeriod;
        }
    }

    public float OverboardCooldown
    {
        get => overboardCooldown;
        set
        {
            overboardCooldown = value;
            waterTrigger.penaltyCooldown = overboardCooldown;
        }
    }

    public float SpawnIntervalMin
    {
        get => spawnIntervalMin;
        set
        {
            spawnIntervalMin = value;
            flotsamManager.spawnIntervalMin = spawnIntervalMin;
        }
    }

    public float SpawnIntervalMax
    {
        get => spawnIntervalMax;
        set
        {
            spawnIntervalMax = value;
            flotsamManager.spawnIntervalMax = spawnIntervalMax;
        }
    }

    public float SpawnRadius
    {
        get => spawnRadius;
        set
        {
            spawnRadius = value;
            flotsamManager.spawnRadius = spawnRadius;
        }
    }

    public float OffRadiusChance
    {
        get => offRadiusChance;
        set
        {
            offRadiusChance = value;
            flotsamManager.offRadiusChance = offRadiusChance;
        }
    }

    public float OffRadiusMaxDistance
    {
        get => offRadiusMaxDistance;
        set
        {
            offRadiusMaxDistance = value;
            flotsamManager.offRadiusMaxDistance = offRadiusMaxDistance;
        }
    }

    public Vector3 CameraPosition
    {
        get => cameraPosition;
        set
        {
            cameraPosition = value;
            Camera.main.transform.position = cameraPosition;
        }
    }

    void OnValidate()
    {
        MinBounds = minBounds;
        MaxBounds = maxBounds;
        Margin = margin;
        TimerStartsAt = timerStartsAt;
        DenoiseWindowSize = denoiseWindowSize;
        OverboardGracePeriod = overboardGracePeriod;
        OverboardCooldown = overboardCooldown;
        SpawnIntervalMin = spawnIntervalMin;
        SpawnIntervalMax = spawnIntervalMax;
        SpawnRadius = spawnRadius;
        OffRadiusChance = offRadiusChance;
        OffRadiusMaxDistance = offRadiusMaxDistance;
        CameraPosition = cameraPosition;
    }


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        flotsamManager = FindObjectsByType<FlotsamManager>(FindObjectsSortMode.None)[0];
        boundsManager = FindObjectsByType<BoundsManager>(FindObjectsSortMode.None)[0];
        waterTrigger = FindObjectsByType<WaterTrigger>(FindObjectsSortMode.None)[0];
        gameTimer = FindObjectsByType<GameTimer>(FindObjectsSortMode.None)[0];
        denoisers = new List<Denoiser>(FindObjectsByType<Denoiser>(FindObjectsSortMode.None));
        OnValidate();
    }
}
