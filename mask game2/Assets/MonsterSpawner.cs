using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Prefab")]
    public GameObject monsterPrefab; // Het monster prefab dat gespawnt wordt

    [Header("Spawn Settings")]
    public int monsterCount = 3; // Hoeveel monsters er in totaal gespawnt moeten worden
    public bool spawnOnStart = true; // Spawn meteen bij start?

    private Transform[] spawnPoints; // Wordt automatisch gevuld met tag "SpawnpointV"

    void Awake()
    {
        // Zoek automatisch alle spawnpoints
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnpointV");
        spawnPoints = new Transform[spawns.Length];
        for (int i = 0; i < spawns.Length; i++)
            spawnPoints[i] = spawns[i].transform;
    }

    void Start()
    {
        if (spawnOnStart)
            SpawnMonsters();
    }

    public void SpawnMonsters()
    {
        if (monsterPrefab == null)
        {
            Debug.LogWarning("Monster prefab is niet ingesteld!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Geen spawnpoints gevonden! Zorg dat ze de tag 'SpawnpointV' hebben.");
            return;
        }

        // Gebruik een lijst om al gebruikte spawnpoints te tracken
        List<Transform> availableSpawns = new List<Transform>(spawnPoints);

        for (int i = 0; i < monsterCount; i++)
        {
            if (availableSpawns.Count == 0)
                break; // Geen vrije spawnpoints meer

            // Kies een random spawnpoint
            int index = Random.Range(0, availableSpawns.Count);
            Transform spawn = availableSpawns[index];

            // Spawn het monster
            GameObject monster = Instantiate(monsterPrefab, spawn.position, spawn.rotation);

            // Optioneel: geef de monster een parent (bijv. voor organisatie)
            monster.transform.parent = this.transform;

            // Verwijder spawnpoint uit lijst zodat het niet dubbel wordt gebruikt
            availableSpawns.RemoveAt(index);
        }
    }
}
