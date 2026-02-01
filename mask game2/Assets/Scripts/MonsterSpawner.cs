using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Prefabs")]
    public GameObject[] monsterPrefabs; // array van monster prefabs

    [Header("Spawn Settings")]
    public bool spawnOnStart = true; // spawn meteen bij start?

    private Transform[] spawnPoints; // automatisch gevuld met tag "SpawnpointV"

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
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogWarning("Geen monster prefabs ingesteld!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Geen spawnpoints gevonden! Zorg dat ze de tag 'SpawnpointV' hebben.");
            return;
        }

        // Gebruik een lijst om vrije spawnpoints te tracken
        List<Transform> availableSpawns = new List<Transform>(spawnPoints);

        // Spawn elk monster 1x
        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            if (availableSpawns.Count == 0)
                break; // geen vrije spawnpoints meer

            // Kies een random spawnpoint
            int index = Random.Range(0, availableSpawns.Count);
            Transform spawn = availableSpawns[index];

            // Spawn het monster
            GameObject monster = Instantiate(monsterPrefabs[i], spawn.position, spawn.rotation);

            // Optioneel: zet parent voor organisatie
            monster.transform.parent = this.transform;

            // Verwijder spawnpoint zodat geen 2 monsters hetzelfde punt krijgen
            availableSpawns.RemoveAt(index);
        }
    }
}
