using UnityEngine;

public class Vault : MonoBehaviour
{
    [Header("Link the vault door here")]
    public VaultDoor door; // 🔹 sleep hier de VaultDoor in de Inspector

    [HideInInspector] public bool inside = false; // true als speler bij een gesloten vault staat

    [Header("Cash Spawner Settings")]
    public GameObject cashPrefab; // prefab van de cash
    public int maxCashCount = 5;   // maximaal aantal cash items dat kan spawnen
    [Range(0f, 1f)]
    public float spawnChance = 0.5f; // kans dat cash überhaupt verschijnt bij start
    [Range(0f, 1f)]
    public float extraCashChance = 0.4f; // kans op elk extra cash item

    private void Start()
    {
        TrySpawnCash();
    }

    private void TrySpawnCash()
    {
        if (cashPrefab == null) return;

        // Check of we überhaupt cash willen spawnen
        if (Random.value > spawnChance) return;

        // Begin altijd met 1 cash
        int cashToSpawn = 1;

        // Extra cash items
        for (int i = 1; i < maxCashCount; i++)
        {
            if (Random.value <= extraCashChance)
            {
                cashToSpawn++;
            }
            else
            {
                break; // stopt als de kans faalt
            }
        }

        // Spawn de cash
        for (int i = 0; i < cashToSpawn; i++)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.x += Random.Range(-1f, 1f);
            spawnPos.y += 0.5f;
            spawnPos.z += Random.Range(-1f, 1f);

            Instantiate(cashPrefab, spawnPos, Quaternion.identity, transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player"))
        {
            if (!door.IsOpen())
                inside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player"))
        {
            inside = !door.IsOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
        }
    }
}
