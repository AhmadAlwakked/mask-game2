using UnityEngine;

public class Vault : MonoBehaviour
{
    [Header("Link the vault door here")]
    public VaultDoor door;

    [HideInInspector] public bool inside = false;

    [Header("Spawn Prefabs")]
    public GameObject cashPrefab;
    public GameObject batteryPrefab;

    [Header("Spawn Chances")]
    [Range(0f, 1f)] public float cashSpawnChance = 0.6f;
    [Range(0f, 1f)] public float batterySpawnChance = 0.3f;

    [Header("Cash Amount Settings")]
    public int maxCashCount = 5;
    [Range(0f, 1f)] public float extraCashChance = 0.4f;

    [Header("Vault Collider (for spawning)")]
    public SphereCollider spawnArea; // sleep hier je Sphere Collider in

    private void Start()
    {
        TrySpawnCash();
        TrySpawnBattery();
    }

    void TrySpawnCash()
    {
        if (cashPrefab == null || spawnArea == null) return;
        if (Random.value > cashSpawnChance) return;

        int count = 1;

        for (int i = 1; i < maxCashCount; i++)
        {
            if (Random.value <= extraCashChance)
                count++;
            else
                break;
        }

        for (int i = 0; i < count; i++)
            SpawnObject(cashPrefab);
    }

    void TrySpawnBattery()
    {
        if (batteryPrefab == null || spawnArea == null) return;
        if (Random.value > batterySpawnChance) return;

        SpawnObject(batteryPrefab);
    }

    void SpawnObject(GameObject prefab)
    {
        // Random positie binnen de sphere collider
        Vector3 randomPos = Random.insideUnitSphere * spawnArea.radius;
        randomPos += spawnArea.center; // center offset
        randomPos += transform.position; // vault position

        // Zorg dat object niet onder de vloer zit
        randomPos.y = Mathf.Max(randomPos.y, transform.position.y + 0.5f);

        // Random rotatie
        Quaternion randomRot = Quaternion.Euler(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)
        );

        Instantiate(prefab, randomPos, randomRot, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player") && !door.IsOpen())
            inside = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player"))
            inside = !door.IsOpen();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inside = false;
    }
}
